using UnityEngine;

public class RemovePhysics : MonoBehaviour
{
    public float liveTime = 3;
    private float timer;
    private float _fadeTimer;
    public Material sliceMaterial;
    public Material sliceMaterialInside;

    private float shrinkSpeed;
    private Material m0, m1;

    private void Awake()
    {
        timer = liveTime;
    }

    private void Start()
    {
        shrinkSpeed = Random.Range(0.05f, 0.1f);
    }

    private void FixedUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {

            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().isKinematic = true;
            if (GetComponent<Collider>())
                GetComponent<Collider>().enabled = false;
            _fadeTimer += 1 * Time.deltaTime;
            if(_fadeTimer >= 1)
            {
                transform.localScale = new Vector3(transform.localScale.x - shrinkSpeed, transform.localScale.y - shrinkSpeed, transform.localScale.z - shrinkSpeed);
                if (transform.localScale.x <= 0)
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}