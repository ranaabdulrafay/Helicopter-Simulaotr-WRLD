using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wrld;
using Wrld.Space;

public class AiSpawner : MonoBehaviour
{
    public EnemyPlaces enemyPlaces;
    public int AIPrefab;
    int SpawnCount, MaxSpawn = 4;
    public float SpawnDelay;
    public PoiDataParser PoiDataParser;
    // Start is called before the first frame update
    void Start()
    {
        enemyPlaces.LoadDataFromFile();
    }
    public void PoisRecieved(bool status)
    {
        if (status)
        {
            StartCoroutine(StartSpawningPois());
        }
        else
        {
            StartCoroutine(StartSpawningHardCoded());
        }
    }
    IEnumerator StartSpawningHardCoded()
    {
        enemyPlaces.LoadDataFromFile();
        SpawnCount = 0;
        while (true)
        {
            if (SpawnCount < MaxSpawn)
            {
                Spawn(enemyPlaces.pointsVctr[Random.Range(0, enemyPlaces.pointsVctr.Count)]);
                SpawnCount++;
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
    IEnumerator StartSpawningPois()
    {
        SpawnCount = 0;
        Vector2 SpawnPos = Vector2.zero;
        while (SpawnCount < PoiDataParser.AllPois.Count)
        {
            if (SpawnCount < MaxSpawn)
            {
                int index = Random.Range(0, PoiDataParser.AllPois.Count);
                SpawnPos.Set(float.Parse(PoiDataParser.AllPois[index].lat), float.Parse(PoiDataParser.AllPois[index].lon));

                Spawn(SpawnPos);
                SpawnCount++;
            }
            yield return new WaitForSeconds(SpawnDelay);
        }
    }
    void Spawn(Vector2 ll)
    {
        LatLong loglat = new LatLong(ll.x, ll.y);
        var ray = Api.Instance.SpacesApi.LatLongToVerticallyDownRay(loglat);
        LatLongAltitude buildingIntersectionPoint;
        var didIntersectBuilding = Api.Instance.BuildingsApi.TryFindIntersectionWithBuilding(ray, out buildingIntersectionPoint);
        if (didIntersectBuilding)
        {
            //Debug.Log("building found");

            var boxAnchor = Pool.Instance.Spawn(AIPrefab);
            boxAnchor.GetComponent<GeographicTransform>().SetPosition(buildingIntersectionPoint.GetLatLong());
            boxAnchor.GetComponentInParent<Health>().Initialize(this);
            var box = boxAnchor.transform.GetChild(0);
            box.localPosition = Vector3.up * (float)buildingIntersectionPoint.GetAltitude();
        }
        else
        {
            //Debug.Log("No building found");
        }
    }
    public void ReportDeath()
    {
        SpawnCount = SpawnCount - 1 < 0 ? 0 : SpawnCount - 1;
    }

}
