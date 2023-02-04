using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    [Serializable]
    public struct SpawnBoundary
    {
        public float x;
        public float y;
        public float xx;
        public float yy;
    }

    public List<GameObject> Citizens = new List<GameObject>();
    public List<GameObject> Goobers = new List<GameObject>();
    public List<GameObject> Buildings = new List<GameObject>();

    public int citizenCount;
    public int gooberCount;

    public List<GameObject> gooberPrefabs;
    public List<GameObject> citizenPrefabs;

    public float gooberSpawnInterval;

    [SerializeField]
    public SpawnBoundary spawnBoundaries;

    public static GameManager Instance;

    public GameObject player;





    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        GameObject a = Instantiate(player, new Vector3(0, 1, 0), Quaternion.identity);
        FindObjectOfType<Camera>().GetComponent<CameraFollow>().m_follow = a.transform;
        SpawnCitizens();
    }

    public void FindBuildings()
    {
    }

    void Update()
    {
        
    }

    public void SpawnCitizens()
    {
        for(int i = 0; i < citizenCount; i++)
        {
            Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(spawnBoundaries.x, spawnBoundaries.xx), 1, UnityEngine.Random.Range(spawnBoundaries.y, spawnBoundaries.yy));
            /*while(IsInsideBuilding(spawnPos))
            {
                spawnPos = new Vector3(UnityEngine.Random.Range(spawnBoundaries.x, spawnBoundaries.xx), 1, UnityEngine.Random.Range(spawnBoundaries.y, spawnBoundaries.yy));
            }*/
            GameObject a = Instantiate(citizenPrefabs[UnityEngine.Random.Range(0, citizenPrefabs.Count)], spawnPos, Quaternion.identity);
        }
    }

    private bool IsInsideBuilding(Vector3 pos)
    {
        float radius = 5f; //size of the building pretty much
        foreach(GameObject b in Buildings)
        {
            if(Vector3.Distance(b.transform.position, pos) < radius)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator SpawnGoober()
    {
        yield return new WaitForSeconds(gooberSpawnInterval);
        GameObject a = Instantiate(gooberPrefabs[UnityEngine.Random.Range(0, gooberPrefabs.Count)], Vector3.zero, UnityEngine.Random.rotation);
    }
}
