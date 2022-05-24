using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlaneWeaponsController : MonoBehaviour
{
    public int BulletId;
    public int EnemyPoolItemId = 0;

    public float FireRate;
    public float ScanDelay = 1;
    public float LineOfSight = 0.75f;
    public int Damage;
    public GameObject SelectedAi;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScanAndFire());
    }
    IEnumerator ScanAndFire()
    {
        while (true)
        {
            for (int i = 0; i < Pool.Instance.GetPoolItemList(EnemyPoolItemId).Count; i++)
            {
                if (Vector3.Dot((Pool.Instance.GetPoolItemList(EnemyPoolItemId)[i].transform.position - transform.position).normalized, transform.forward) > LineOfSight)
                {
                    SelectedAi = Pool.Instance.GetPoolItemList(EnemyPoolItemId)[i];

                    while (SelectedAi != null
                        && Pool.Instance.GetPoolItemList(EnemyPoolItemId).Contains(SelectedAi)
                        && Vector3.Dot((SelectedAi.transform.position - transform.position).normalized, transform.position) > LineOfSight)
                    {
                        if (SelectedAi.GetComponent<Health>())
                        {
                            SelectedAi.GetComponent<Health>().Damage(Damage);
                        }
                        yield return new WaitForSeconds(FireRate);
                    }
                }
            }
            yield return new WaitForSeconds(ScanDelay);
        }
    }

}
