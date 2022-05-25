using System.Collections;
using UnityEngine;
using Wrld;
using Wrld.Common.Maths;
using Wrld.Space;
using Wrld.Space.Positioners;

public class FlyObjectOverMap : MonoBehaviour
{
    float movementSpeed = 100.0f;
    float turnSpeed = 90.0f;
    float movementAngle = 0.0f;

    public GeographicTransform coordinateFrame;
    public Transform box;
    public LatLong targetPosition = new LatLong(37.802, -122.406);

    [Header("Camera")]
    public float distance = 1700;
    public float followSpeed = 0.1f;
    void OnEnable()
    {
        coordinateFrame.SetPosition(targetPosition);
    }

    void Update()
    {
        // Update movement angle from input
        movementAngle += Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        coordinateFrame.SetHeading(movementAngle);

        // Update target position from input
        var latitudeDelta = Mathf.Cos(Mathf.Deg2Rad * movementAngle) * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        var longitudeDelta = Mathf.Sin(Mathf.Deg2Rad * movementAngle) * Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        targetPosition.SetLatitude(targetPosition.GetLatitude() + (latitudeDelta * 0.00006f));
        targetPosition.SetLongitude(targetPosition.GetLongitude() + (longitudeDelta * 0.00006f));

        // Command GeographicTransform to move using lat-long
        coordinateFrame.SetPosition(targetPosition);

        //Api.Instance.CameraApi.MoveTo(targetPosition, distanceFromInterest: distance, headingDegrees: 0, tiltDegrees: 45);
        Api.Instance.CameraApi.AnimateTo(targetPosition, distanceFromInterest: distance, transitionDuration: Time.deltaTime*followSpeed, jumpIfFarAway: true);
        CheckForBuildingUnder(targetPosition);
    }

    void CheckForBuildingUnder(LatLong latLong)
    {
        var ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(latLong);
        LatLongAltitude buildingIntersectionPoint;
        var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out buildingIntersectionPoint);
        if (didIntersectBuilding)
        {
            Debug.Log("building found");
        }
        else
        {
            Debug.Log("No building found");
        }
    }
}

