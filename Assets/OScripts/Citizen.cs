using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{

    public float movementSpeed;
    private float _rootingSpeed;

    public GameObject player;

    private bool _isCelebrating;

    private Renderer _rend;
    private Material _material;


    private Color _targetColor;





    private void Start()
    {
        _rootingSpeed = Random.Range(0.05f, 0.2f);
        transform.rotation = GetRandomDirection();
        _rend = GetComponent<Renderer>();
        _material = GetComponent<Renderer>().material;
        _targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            _isCelebrating = !_isCelebrating;
            if (_isCelebrating)
            {
                Celebrate(1f);
            } else
            {
                _material.color = Color.white;
            }
        }
    }


    public void MoveRandom()
    {
        //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime, Space.Self);
        transform.position += transform.forward * movementSpeed * Time.deltaTime;
        RaycastHit fwdRay;
        if (Physics.Raycast(transform.position, transform.forward, out fwdRay, 2f))
        {
            if(fwdRay.transform != this)
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(transform.forward) * fwdRay.distance, Color.red);
                transform.rotation = GetRandomDirection();
            }
        }
        if(transform.position.x < GameManager.Instance.spawnBoundaries.x || 
            transform.position.x > GameManager.Instance.spawnBoundaries.xx || 
            transform.position.z < GameManager.Instance.spawnBoundaries.y ||
            transform.position.z > GameManager.Instance.spawnBoundaries.yy)
        {
            transform.rotation = GetRandomDirection();
        }
    }

    public Quaternion GetRandomDirection()
    {
        return new Quaternion(transform.rotation.x, Random.rotation.y, transform.rotation.z, Random.rotation.w);
    }


    private void FixedUpdate()
    {


        if(_isCelebrating)
        {

            Celebrate(1f);
        } else
        {
            MoveRandom();
        }
    }

    public void Celebrate(float buffAmount)
    {

        float yVal = Mathf.Max(1, Mathf.Sin(Time.time / _rootingSpeed) * 1.5f);
        print(yVal);
        transform.position = new Vector3(transform.position.x, yVal, transform.position.z);
        //transform.LookAt(player.transform);

        float colorSpeed = 1f;

        _material.color = Color.Lerp(_material.color, _targetColor, Mathf.PingPong(Time.time, colorSpeed));
        if (_material.color == _targetColor)
        {
            _targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 0.2f), Random.Range(0f, 1f));
        }

    }
}
