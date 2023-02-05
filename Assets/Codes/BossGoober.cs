using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


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

    private int _randomRoll;

    private bool _attackBuilding;

    private GameObject _closestBuilding;

    public float totalBuildingHealth;
    public float maxTotalBuildingHealth;
    public TMP_Text buildingHealthText;

    public Slider buildingHealthBar;


    void Start()
    {
        m_anim = GetComponentInChildren<Animator>();
        spawnPosition = WaypointList[0].transform.position;
        _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.flipX = false;
        StartCoroutine(RollRandomBuildingAttack());
        StartCoroutine(UpdateMaxHealth());
    }

   IEnumerator UpdateMaxHealth()
    {
        yield return new WaitForSeconds(0.1f);
        maxTotalBuildingHealth = Buildings.Count * Buildings[0].GetComponent<Building>().maxHealth;
        UpdateBuildingStatus();
    }
    void FixedUpdate()
    {
        if(!GameManager.Instance.isGameFinished) MoveToWaypoint(_currentWaypointIndex);
    }

    IEnumerator AttackBuilding(GameObject b)
    {
        m_anim.SetTrigger("Attack");
        _attackBuilding = false;
        yield return new WaitForSeconds(0.5f);
        b.GetComponent<Building>().TakeDamage(60f);
        UpdateBuildingStatus();

    }

    public void UpdateBuildingStatus()
    {
        float newHp = 0;
        foreach (GameObject building in Buildings)
        {
            newHp += building.GetComponent<Building>().health;
        }
        totalBuildingHealth = newHp;
        buildingHealthText.text = totalBuildingHealth + "/" + maxTotalBuildingHealth;
        UpdateHealthImage();
    }

    public void UpdateHealthImage()
    {
        //int hp = (int)m_stats.health;
        if (buildingHealthBar)
            buildingHealthBar.value = totalBuildingHealth / maxTotalBuildingHealth;
        //playerHp.sprite = playerHps[hp];
    }


    IEnumerator RollRandomBuildingAttack()
    {
        yield return new WaitForSeconds(0.1f);
        _randomRoll = Random.Range(0, 100);
        if(_randomRoll > 40 && !_attackBuilding)
        {
            _closestBuilding = GetClosestBuilding();
            _attackBuilding = true;
        }
        yield return new WaitForSeconds(4f);
        StartCoroutine(RollRandomBuildingAttack());
    }

    public GameObject GetClosestBuilding()
    {
        float dist = 999f;
        GameObject bb = null;
        foreach (GameObject b in Buildings)
        {
            float d = Vector3.Distance(b.transform.position, transform.position);
            if (d < dist)
            {
                dist = d;
                bb = b;
            }
        }
        return bb;
    }
    public void MoveToWaypoint(int index)
    {
        //transform.LookAt(WaypointList[index]);
        if (_attackBuilding)
        {
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 4.5f, transform.position.z), _closestBuilding.transform.position, movementSpeed / 2 * Time.deltaTime);
            if(Vector3.Distance(_closestBuilding.transform.position, transform.position) <= 6f)
            {
                StartCoroutine(AttackBuilding(_closestBuilding));
            }
        } else
        {
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 4.5f, transform.position.z),
                                                        WaypointList[index].position, movementSpeed * Time.deltaTime);
            //transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            if (Vector3.Distance(WaypointList[_currentWaypointIndex].transform.position, transform.position) <= _switchDistance)
            {
                if (_currentWaypointIndex + 1 >= WaypointList.Count)
                {
                    _sr.flipX = !_sr.flipX;
                    WaypointList.Reverse();
                    _currentWaypointIndex = 0;
                }
                else
                {
                    _currentWaypointIndex++;
                }
            }
        }
    }
}
