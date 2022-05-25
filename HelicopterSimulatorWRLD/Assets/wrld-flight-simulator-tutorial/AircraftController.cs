using System;
using UnityEngine;

public class AircraftController : MonoBehaviour {
    // Constant forward thrust from the aircraft engines.
    public float forwardThrustForce = 40.0f;

    // Turning force as a multiple of the thrust force.
    public float turnForceMultiplier = 5000.0f;

    // Maximum speed in metres / second.
    public float maxSpeed = 400.0f;

    private Vector3 controlForce;
    private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
        // Find the RigidBody component and save a reference to it.
        rigidBody = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
        // Calculate the control force to apply from the inputs.
        // The forward (z) component is always applied to keep the aircraft moving forward.
        controlForce.Set(
            Input.GetAxis("Horizontal") * turnForceMultiplier, 
            Input.GetAxis("Vertical") * turnForceMultiplier, 
            1.0f
        );
        controlForce = controlForce.normalized * forwardThrustForce;
    }

    void FixedUpdate()
    {
        // Apply the braking force to apply to limit the maximum speed.
        float excessSpeed = Math.Max(0, rigidBody.velocity.magnitude - maxSpeed);        
        Vector3 brakeForce = rigidBody.velocity.normalized * excessSpeed;    
        rigidBody.AddForce(-brakeForce, ForceMode.Force);   

        // Apply the control force to move the aircraft in the desired direction.
        rigidBody.AddRelativeForce(controlForce, ForceMode.Force);

        // Rotate the aircraft to face in the direction that it is flying in.
        transform.forward = rigidBody.velocity;
    }
}
    