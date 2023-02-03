using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Citizen : MonoBehaviour
{

    public float movementSpeed;
    public float rootingSpeed;

    public GameObject player;

    private bool _isCelebrating;

    private Renderer _rend;
    private Material _material;


    private Color _targetColor;

    private void Start()
    {
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
            if (_isCelebrating) Celebrate(1f);
        }
    }




    private void FixedUpdate()
    {
        if(_isCelebrating)
        {
            Celebrate(1f);
        }
    }

    public void Celebrate(float buffAmount)
    {


        //transform.LookAt(player.transform);

        float colorSpeed = 1f;

        _material.color = Color.Lerp(_material.color, _targetColor, Mathf.PingPong(Time.time, colorSpeed));
        if (_material.color == _targetColor)
        {
            _targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 0.2f), Random.Range(0f, 1f));
        }
    }
}
