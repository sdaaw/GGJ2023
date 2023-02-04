using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnExplosion : MonoBehaviour
{
    public GameObject explosion;

    private Stats _stats;

    private void Awake()
    {
        _stats = GetComponent<Stats>();
    }

    private void Update()
    {
        if(_stats && _stats.health <= 0)
        {
            Explode();
            Destroy(this.gameObject);
        }
    }


    public void Explode()
    {
        Instantiate(explosion, this.transform.position, this.transform.rotation);
    }
}
