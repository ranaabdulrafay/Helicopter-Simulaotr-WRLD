using UnityEngine;

public class CameraController : MonoBehaviour {
    // Reference to the aircraft we're going to be following.
    public GameObject objectToFollow;

    // Position of the camera relative to the aircraft.
    // Here we have the camera behind and slightly above the aircraft.
    public Vector3 positionRelativeToObject = new Vector3(0, 50, -300);

    // How aggressively the camera follows the "ideal" position behind the object being followed.
    public float followBias = 0.05f;

	// Use this for initialization
	void Start () {
    }

    void FixedUpdate () {
        // Calculate where the camera should be in the world.
        var idealCameraPosition = (objectToFollow.transform.rotation * positionRelativeToObject) + objectToFollow.transform.position;

        // Calculate a velocity to move the camera towards the ideal position.
        var cameraVelocity = (idealCameraPosition - transform.position) * followBias;

        // Set the position of camera to the position of the aircraft plus an offset.
        transform.position += cameraVelocity;

        // Rotate the camera to look at the object being followed.
        transform.LookAt(objectToFollow.transform.position);
    }
}
