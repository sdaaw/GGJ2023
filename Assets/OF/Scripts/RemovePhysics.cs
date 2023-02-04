using UnityEngine;

public class RemovePhysics : MonoBehaviour
{
    public float liveTime = 5;
    private float timer;


    private void Awake()
    {
        timer = liveTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().isKinematic = true;
            if (GetComponent<Collider>())
                GetComponent<Collider>().enabled = false;
        }
    }
}