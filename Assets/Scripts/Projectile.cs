using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    // to check what layers it collides with
    public LayerMask collisionMask;
    float speed = 15;
    float damage = 1;
	
    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

	// Update is called once per frame
	void Update () {
        // Collision detection
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        // move forward
        transform.Translate(Vector3.forward * moveDistance);
	}

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //QueryTriggerInteraction (checks if to collide with triggers)
        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit) {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }
}
