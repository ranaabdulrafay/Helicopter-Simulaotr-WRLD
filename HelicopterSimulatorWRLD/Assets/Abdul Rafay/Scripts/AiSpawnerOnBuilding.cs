using System.Collections;
using Wrld;
using Wrld.Space;
using UnityEngine;

public class AiSpawnerOnBuilding : MonoBehaviour
{
    LatLong targetPosition;
    public GeographicTransform geographicTransform;
    public void SetPosition(Vector3 position)
    {
        targetPosition = LatLong.FromECEF(position);
        geographicTransform.SetPosition(targetPosition);
    }
    void Awake()
    {

    }
    private void OnEnable()
    {
    }
    void Update()
    {
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
