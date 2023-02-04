using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform _transform;
    private Camera _cam;
    private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _cam = FindObjectOfType<Camera>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material.renderQueue = 4000;
    }

    private void Update()
    {
        _transform.LookAt(_cam.transform.position);
    }
}
