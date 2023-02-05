using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGoober : MonoBehaviour
{


    public List<Transform> WaypointList = new List<Transform>();
    public float movementSpeed;
    public List<GameObject> Buildings = new List<GameObject>();

    private int _currentWaypointIndex;

    private Animator m_anim;


    public Vector3 spawnPosition;

    private SpriteRenderer _sr;

    [SerializeField]
    private float _switchDistance;
    void Start()
    {
        m_anim = GetComponentInChildren<Animator>();
        spawnPosition = WaypointList[0].transform.position;
        _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.flipX = false;
    }
    void FixedUpdate()
    {
        MoveToWaypoint(_currentWaypointIndex);
    }

    public void AttackBuilding(GameObject b)
    {

    }

    public void MoveToWaypoint(int index)
    {
        //transform.LookAt(WaypointList[index]);
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 4.5f, transform.position.z), 
            WaypointList[index].position, movementSpeed * Time.deltaTime);
        //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        if (Vector3.Distance(WaypointList[_currentWaypointIndex].transform.position, transform.position) <= _switchDistance)
        {
            if(_currentWaypointIndex + 1 >= WaypointList.Count)
            {
                _sr.flipX = !_sr.flipX;
                WaypointList.Reverse();
                _currentWaypointIndex = 0;
            } else
            {
                _currentWaypointIndex++;
            }
        }
    }
}
