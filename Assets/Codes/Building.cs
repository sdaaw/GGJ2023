using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public GameObject BigGoober;

    public float health;
    public float maxHealth;


    void Start()
    {
        health = maxHealth;
        BigGoober.GetComponent<BossGoober>().Buildings.Add(gameObject);
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            SoundManager.PlayASource("Break");
            if(GetComponentInChildren<Fracture>())
            {
                GetComponentInChildren<Fracture>().ComputeFracture();
            }
            BigGoober.GetComponent<BossGoober>().Buildings.Remove(gameObject);
        }
    }
}
