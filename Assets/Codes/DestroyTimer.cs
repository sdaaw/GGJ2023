using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float timerToDestroy = 0;

    // Update is called once per frame
    void Update()
    {
        timerToDestroy -= Time.deltaTime;

        if(timerToDestroy <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
