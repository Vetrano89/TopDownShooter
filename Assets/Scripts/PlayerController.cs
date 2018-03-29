using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    //public variables
    Vector3 velocity;
    Rigidbody myRigidBody;

	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody>();
	}
	
    public void Move(Vector3 _velocity) {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint) {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        // rotate object to look at a point
        transform.LookAt(heightCorrectedPoint);
    }

    // needs to be executed at small regular steps
    // so it doesn't go through any objects
    // slow frame rates could cause this issue
    public void FixedUpdate() {
        // add current position and velcoity.  multiplied by
        // the time inbetween the calls to fixed update
        myRigidBody.MovePosition(myRigidBody.position + velocity * Time.fixedDeltaTime);
    }
}
