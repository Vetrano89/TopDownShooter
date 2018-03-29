using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    public float moveSpeed = 5;

    //reference to camera
    Camera viewCamera;
    PlayerController controller;

	// Use this for initialization
	void Start () {
        controller = GetComponent<PlayerController> ();
        viewCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        //GetAxisRaw instead of GetAxis
        // avoids smoothing.  Was moving the character after input was let go
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;

        // moveVelocity exists in PlayerController;
        controller.Move(moveVelocity);
        // creates a ray that goes from the camera through
        // the screen position and on to infinity
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        // now need to find ground Plane
        // create a plane progrmatically
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        // Planes have a Raycast method
        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 point = ray.GetPoint(rayDistance);
            // draw line with debug
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }
    }
}
