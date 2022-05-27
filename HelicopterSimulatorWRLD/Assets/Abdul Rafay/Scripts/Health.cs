using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image HealthBar;
    public float Maxhealth = 100;
    float health;
    bool isDead = false;
    public int DeadEffect;
    AiSpawner mySpawner;

    public UnityEvent OnDead;
    void UpdateUi()
    {
        if (HealthBar)
            HealthBar.fillAmount = health / Maxhealth;
    }
    public void Damage(float DmgAmount)
    {
        if (isDead)
            return;
        if (health - DmgAmount <= 0)
        {
            health = 0;
            Dead();
        }
        else
            health -= DmgAmount;

        UpdateUi();
    }

    void Dead()
    {
        isDead = true;
        Pool.Instance.DeSpawn(transform);
        Pool.Instance.Spawn(DeadEffect, transform.GetChild(0).position);
        if (mySpawner)
            mySpawner.ReportDeath();
        if (OnDead != null)
            OnDead.Invoke();
    }

    public void Initialize(AiSpawner _spawner)
    {
        mySpawner = _spawner;
        isDead = false;
        health = Maxhealth;
        HealthBar.fillAmount = 1;
    }
    void OnMouseDown()
    {
        Debug.Log("called " + health);
        Damage(50);
    }
}
