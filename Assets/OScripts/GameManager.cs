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

    public int citizenCount;
    public int gooberCount;

    public List<GameObject> gooberPrefabs;
    public List<GameObject> citizenPrefabs;

    public float gooberSpawnInterval;

    [SerializeField]
    public SpawnBoundary spawnBoundaries;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnCitizens()
    {
        for(int i = 0; i < citizenCount; i++)
        {
            GameObject a = Instantiate(citizenPrefabs[UnityEngine.Random.Range(0, citizenPrefabs.Count)], Vector3.zero, Quaternion.identity);
        }
    }

    IEnumerator SpawnGoober()
    {
        yield return new WaitForSeconds(gooberSpawnInterval);
        GameObject a = Instantiate(gooberPrefabs[UnityEngine.Random.Range(0, gooberPrefabs.Count)], Vector3.zero, Quaternion.identity);
    }

    public bool CheckForBuilding(Vector3 position)
    {
        float radius;
        return false;
    } 
}
