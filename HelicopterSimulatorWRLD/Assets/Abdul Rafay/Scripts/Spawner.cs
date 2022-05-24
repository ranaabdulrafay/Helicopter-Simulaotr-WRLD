using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int AIPrefab;
    public Transform Player;
    public int MinRange,SpawnRange;
    public int SpawnCount, MaxSpawn = 4;
    public float LastSpawn, SpawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCount = 0;
        LastSpawn = Time.time;
        InvokeRepeating("Spawn", 2, SpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Spawn()
    {
        if (SpawnCount >= MaxSpawn)
        {
            LastSpawn = Time.time;
            return;
        }
        Vector3 pos = Player.position + (((Random.insideUnitSphere) * SpawnRange)+(Vector3.one * MinRange));
        GameObject obj = Pool.Instance.Spawn(AIPrefab, pos);
        obj.GetComponent<Health>().Initialize();
        SpawnCount++;
        LastSpawn = Time.time;
    }


}
