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
        health = 50f;
        maxHealth = 50f;

        BigGoober.GetComponent<BossGoober>().Buildings.Add(gameObject);
    }

    public void TakeDamage(float amount)
    {
        GetComponentInChildren<Fracture>().ComputeFracture();//move below!!!!!!
        health -= amount;
        if(health < 0)
        {
            BigGoober.GetComponent<BossGoober>().Buildings.Remove(gameObject);
            //GetComponentInChildren<Fracture>().ComputeFracture();

        }
    }
}
