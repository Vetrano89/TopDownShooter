using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]

// LivingEntity is the baseclass, can be referenced as base (line 16)
public class Enemy : LivingEntity {

    public enum State {Idle, Chasing, Attacking};
    State currentState;

    //ref to navmesh agent
    NavMeshAgent pathfinder;
    Transform target;
    // ref to a material;
    Material skinMaterial;
    // ref to a color;
    Color originalColor;

    LivingEntity targetEntity;

    float attackDistanceThreshold = 0.5f;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

	// Use this for initialization
    // public override to literally override the base class Start method
	protected override void Start () {
        // call start method from base class (LivingEntity)
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        // set the material
        skinMaterial = GetComponent<Renderer>().material;
        // set material color
        originalColor = skinMaterial.color;

        // By default, Enemy is chasing (pathfinding is on)
        currentState = State.Chasing;
        target = GameObject.FindGameObjectWithTag("Player").transform;

        targetEntity = target.GetComponent<LivingEntity>();
        targetEntity.OnDeath += OnTargetDeath;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        StartCoroutine(UpdatePath());
	}

    void OnTargetDeath() {
        hasTarget = false;
        currentState = State.Idle;
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time > nextAttackTime) {
            // when we are just comparing two distances, don't use Vector.Distance
            float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;

            float attackDistanceThresholdFromCenter = attackDistanceThreshold + myCollisionRadius + targetCollisionRadius;
            if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThresholdFromCenter, 2)) {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
	}

    IEnumerator Attack() {

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * myCollisionRadius;

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        while (percent <= 1) {

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            // Linearly moves object from one position to the next
            // interpolation 0 means originalPosition, 1 is attackPosition
            // anything number between that is a position between that
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);


            // Skips a frame
            yield return null;
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathfinder.enabled = true;

    }

    IEnumerator UpdatePath() {
        float refreshRate = 0.2f;

        while (target != null) {
            if (currentState == State.Chasing) {
                // set target position to (target position - direction between enemy and target
                // multiplied by radius of the two collision bounds
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
                if (!dead) {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
