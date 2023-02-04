using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _transform;
    private Camera _cam;
    // private SpriteRenderer _spriteRenderer;
    public bool xOnly;
    public bool yOnly;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _cam = FindObjectOfType<Camera>();
        //_spriteRenderer = GetComponent<SpriteRenderer>();
        //_spriteRenderer.material.renderQueue = 4000;
    }

    private void Update()
    {

        if(xOnly)
        {
            _transform.LookAt(new Vector3(_cam.transform.position.x, _transform.position.y, _cam.transform.position.z));
        }
        else if(yOnly)
        {
            _transform.LookAt(new Vector3(_transform.position.x, _cam.transform.position.y, _transform.position.z));
        }
        else
        {
            _transform.LookAt(_cam.transform.position);
        }
    }
}
