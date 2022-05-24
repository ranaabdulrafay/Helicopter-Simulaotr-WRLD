using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolItemIdentifier : MonoBehaviour
{
    public int PoolItemId;
}
public class Pool : GenericSingleton<Pool>
{
    [Serializable]
    public class PoolItem
    {
        public int Id;
        public GameObject Item;
        public int CountSpawn = 5;
        List<GameObject> SpawnedItems = new List<GameObject>();
        List<GameObject> SleepingItems = new List<GameObject>();
        public List<GameObject> GetSpawnedList()
        {
            return SpawnedItems;
        }

        public GameObject Create()
        {
            GameObject itm = Instantiate(Item);
            itm.AddComponent<PoolItemIdentifier>().PoolItemId = Id;
            return itm;
        }
        public void SpawnSleeping()
        {
            GameObject obj = Create();
            obj.SetActive(false);
            SleepingItems.Add(obj);
        }
        public GameObject Spawn()
        {
            GameObject obj;
            if (SleepingItems.Count > 0)
            {
                obj = SleepingItems[0];
                obj.SetActive(true);
                SpawnedItems.Add(obj);
                SleepingItems.RemoveAt(0);
                return obj;
            }
            else
            {
                obj = Create();
                SpawnedItems.Add(obj);
                return obj;
            }
        }
        public GameObject Spawn(Vector3 position)
        {
            GameObject obj;
            if (SleepingItems.Count > 0)
            {
                obj = SleepingItems[0];
                obj.transform.position = position;
                obj.SetActive(true);
                SpawnedItems.Add(obj);
                SleepingItems.RemoveAt(0);
                return obj;
            }
            else
            {
                obj = Create();
                SpawnedItems.Add(obj);
                obj.transform.position = position;
                return obj;
            }
        }
        public GameObject Spawn(Vector3 position, Quaternion Rotation)
        {
            GameObject obj;
            if (SleepingItems.Count > 0)
            {
                obj = SleepingItems[0];
                obj.transform.position = position;
                obj.transform.rotation = Rotation;
                obj.SetActive(true);
                SpawnedItems.Add(obj);
                SleepingItems.RemoveAt(0);
                return obj;
            }
            else
            {
                obj = Create();
                SpawnedItems.Add(obj);
                obj.transform.position = position;
                obj.transform.rotation = Rotation;
                return obj;
            }
        }

        public void DeSpawn(Transform ToDespawn)
        {
            GameObject itm = SpawnedItems.Find(x => x.transform == ToDespawn);
            if (itm)
            {
                itm.SetActive(false);
                SpawnedItems.Remove(itm);
                SleepingItems.Add(itm);
            }
            else
            {
                Destroy(itm);
            }
        }
    }
    public PoolItem[] PoolItems;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PoolItems.Length; i++)
        {
            for (int j = 0; j < PoolItems[i].CountSpawn; j++)
                PoolItems[i].SpawnSleeping();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(int id)
    {
        PoolItem poolItem = FindPoolItem(id);
        if (poolItem != null)
        {
            poolItem.Spawn();
        }
    }
    public GameObject Spawn(int id, Vector3 position)
    {
        PoolItem poolItem = FindPoolItem(id);
        if (poolItem != null)
        {
            return poolItem.Spawn(position);
        }
        return null;
    }
    public void Spawn(int id, Vector3 position, Quaternion rotation)
    {
        PoolItem poolItem = FindPoolItem(id);
        if (poolItem != null)
        {
            poolItem.Spawn(position, rotation);
        }
    }
    public void DeSpawn(Transform ToDespawn)
    {
        if (ToDespawn.GetComponent<PoolItemIdentifier>())
        {
            PoolItem poolItem = FindPoolItem(ToDespawn.GetComponent<PoolItemIdentifier>().PoolItemId);
            poolItem.DeSpawn(ToDespawn);
        }
        else
        {
            Destroy(ToDespawn);
        }
    }
    public List<GameObject> GetPoolItemList(int id)
    {
        PoolItem poolItem = FindPoolItem(id);
        if (poolItem != null)
        {
            return poolItem.GetSpawnedList();
        }
        return null;
    }

    PoolItem FindPoolItem(int id)
    {
        for (int i = 0; i < PoolItems.Length; i++)
        {
            if (PoolItems[i].Id == id)
                return PoolItems[i];
        }
        return null;
    }
}
