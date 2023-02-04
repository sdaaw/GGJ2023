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

    private float _cheerDistance;

    private Vector3 _targetPosition;
    public float cheerTime;

    private float _cheerTimer;

    public GameObject particleObject;

    private ParticleSystem celebrationParticle;

    public enum CitizenState
    {
        Celebrating,
        Walking,
        WalkingToPos
    }

    private CitizenState _cState;

    private Stats _playerStats;





    private void Start()
    {
        _playerStats = GameManager.Instance.player.GetComponent<Stats>();
        celebrationParticle = particleObject.GetComponent<ParticleSystem>();
        _cheerDistance = Random.Range(1f, 2f);
        _cState = CitizenState.Walking;
        _rootingSpeed = Random.Range(0.05f, 0.2f);
        transform.rotation = GetRandomDirection();
        _rend = GetComponent<Renderer>();
        _material = GetComponent<Renderer>().material;
        _targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKeyUp(KeyCode.Space))
        {
            _isCelebrating = !_isCelebrating;
            if (_isCelebrating)
            {
                _cState = CitizenState.Celebrating;
                Celebrate();
            } else
            {
                _cState = CitizenState.Walking;
                _material.color = Color.white;
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            }
        }*/

        if(Input.GetKeyUp(KeyCode.R))
        {
            _targetPosition = GameManager.Instance.player.transform.position;
            print(_targetPosition);
            _cState = CitizenState.WalkingToPos;
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
                //Debug.DrawRay(transform.position, transform.TransformDirection(transform.forward) * fwdRay.distance, Color.red);
                transform.rotation = GetRandomDirection();
            }
        }
        if(transform.position.x <= GameManager.Instance.spawnBoundaries.x || 
            transform.position.x >= GameManager.Instance.spawnBoundaries.xx || 
            transform.position.z <= GameManager.Instance.spawnBoundaries.y ||
            transform.position.z >= GameManager.Instance.spawnBoundaries.yy)
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


        if(_cState == CitizenState.Celebrating)
        {
            Celebrate();
        }
        if(_cState == CitizenState.Walking)
        {
            MoveRandom();
        }
        if(_cState == CitizenState.WalkingToPos)
        {
            MoveTowardsPoint(_targetPosition);
        }

    }

    public void MoveTowardsPoint(Vector3 pos)
    {
        transform.LookAt(new Vector3(pos.x, transform.position.y, transform.position.x));
        transform.position += transform.forward * (movementSpeed * 3f) * Time.deltaTime;
        RaycastHit fwdRay;
        if (Physics.Raycast(transform.position, transform.forward, out fwdRay, 2f))
        {
            if (fwdRay.transform != this)
            {
                _cState = CitizenState.Celebrating;
            }
        }
        if(Vector3.Distance(transform.position, pos) < _cheerDistance)
        {
            _cState = CitizenState.Celebrating;
        }
    }

    public void Celebrate()
    {
        if(Random.Range(0, 1000) > 995)
        {
            celebrationParticle.Play();
        }
        _playerStats.GainExperience(1); //arbitrary number of experience gained
        float yVal = Mathf.Max(1, Mathf.Sin(Time.time / _rootingSpeed) * 1.5f);
        transform.position = new Vector3(transform.position.x, yVal, transform.position.z);
        //transform.LookAt(player.transform);

        float colorSpeed = 1f;

        _material.color = Color.Lerp(_material.color, _targetColor, Mathf.PingPong(Time.time, colorSpeed));
        if (_material.color == _targetColor)
        {
            _targetColor = new Color(Random.Range(0.5f, 1f), Random.Range(0f, 0.2f), Random.Range(0.5f, 1f));
        }

        _cheerTimer += 1 * Time.deltaTime;
        if (_cheerTimer >= cheerTime)
        {
            _material.color = Color.white;
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            _cState = CitizenState.Walking;
            _cheerTimer = 0;
        }

        ChatPopup chat = GetComponentInChildren<ChatPopup>();
        if (chat != null)
        {
            chat.DisplayMessage();
            // chat.StartCoroutine(chat.DisplayMessageTime(chat.GetRandomMessage(), 1));
        }


    }
}
