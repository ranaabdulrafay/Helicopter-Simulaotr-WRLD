using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlaneWeaponController : MonoBehaviour
{
    public int BulletId;
    public int EnemyPoolItemId = 0;

    public float FireRate;
    public float ScanDelay = 1;
    public float LineOfSight = 0.75f;
    public float Range;
    public int Damage;

    Collider[] Enemies;
    public LayerMask EnemyLayer;
    public Transform SelectedAi;
    public Health SelectedAiHealth;

    public Transform Muzzle;
    public ParticleSystem MuzzleEffect;
    public ParticleSystem BulletEffect;
    public AudioSource ShootSoundSrc;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScanAndFire());
    }
    IEnumerator ScanAndFire()
    {
        WaitForSeconds wait = new WaitForSeconds(ScanDelay);
        while (true)
        {
            Enemies = Physics.OverlapSphere(gameObject.transform.position, Range, EnemyLayer);
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i].GetComponentInParent<Health>() != null
                    && Vector3.Dot((Enemies[i].transform.position - transform.position).normalized, transform.forward) > LineOfSight)
                {
                    SelectedAi = Enemies[i].transform;
                    SelectedAiHealth = Enemies[i].transform.GetComponentInParent<Health>();
                    yield return Fire();
                }
            }
            yield return wait;
        }
    }
    IEnumerator Fire()
    {
        WaitForSeconds wait = new WaitForSeconds(FireRate);
        while (SelectedAi != null
            && SelectedAi.gameObject.activeInHierarchy
            && Vector3.Distance(transform.position, SelectedAi.gameObject.transform.position) < Range
            && Vector3.Dot((SelectedAi.transform.position - transform.position).normalized, transform.forward) > LineOfSight)
        {
            Debug.Log("LOS "+ Vector3.Dot((SelectedAi.transform.position - transform.position).normalized, transform.forward));
            if (Muzzle != null)
                Muzzle.transform.LookAt(SelectedAi);
            if (MuzzleEffect != null)
                MuzzleEffect.Play();
            if (BulletEffect != null)
                BulletEffect.Play();
            if (ShootSoundSrc != null)
                ShootSoundSrc.Play();
            if (SelectedAiHealth)
            {
                Debug.Log("damage");
                SelectedAiHealth.Damage(Damage);
            }
            yield return wait;
        }
        SelectedAi = null;
        SelectedAiHealth = null;
        if (MuzzleEffect != null)
            MuzzleEffect.Stop();
        if (BulletEffect != null)
            BulletEffect.Stop();
        if (ShootSoundSrc != null)
            ShootSoundSrc.Stop();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
