using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    public float swingTimer;
    public float swingTimerMax;
    public bool canMelee;

    public float swingDuration;
    public float maxSwingDuration;
    public bool canDealDamage;

    // public SaveFile saveFile;
    // public GameManager gameManager;

    //public bool isEnemy;

    private void Update()
    {
        if(swingTimer > 0)
            swingTimer -= Time.deltaTime;
        if(swingTimer < 0)
        {
            swingTimer = 0;
            canMelee = true;
            hitList.Clear();
        }

        if (swingDuration > 0)
        {
            canDealDamage = true;
            swingDuration -= Time.deltaTime;
        }
            
        if(swingDuration < 0)
        {
            swingDuration = 0;
            canDealDamage = false;
        }
    }

    public void Swing()
    {
        canMelee = false;
        swingTimer = swingTimerMax;
        swingDuration = maxSwingDuration;
    }

    private List<Stats> hitList = new List<Stats>();

    public void ClearHitList()
    {
        canDealDamage = false;
        hitList.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root != owner && other.gameObject.layer != 0)// && other.gameObject.layer != gameObject.layer)
        {
            //Debug.Log(other);
            Stats s = other.transform.root.GetComponent<Stats>();

            if (s != null && canDealDamage && !hitList.Contains(s))
            {
                s.TakeDmg(damage);
                hitList.Add(s);
            }
        }
    }
}
