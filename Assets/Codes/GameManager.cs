using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using TMPro;

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

    private List<GameObject> GoobersAlive = new List<GameObject>();

    public int gooberLimit;

    public int citizenCount;
    public int gooberCount;
    public int waveNumber;

    public GameObject bigGoober;

    public List<GameObject> gooberPrefabs;
    public List<GameObject> citizenPrefabs;

    public float gooberSpawnInterval;

    [SerializeField]
    public SpawnBoundary spawnBoundaries;

    public static GameManager Instance;


    [SerializeField]
    private GameObject playerPrefab;


    public GameObject player;

    public Canvas PlayerUI;
    public GameObject playerTakeDamageEffect;
    public Slider playerHealthbar;

    public float score = 0;

    public TMP_Text scoreText;

    public TMP_Text finalScoreText;
    public Image gameEndPanel;

    public bool isGameFinished;
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
        player = Instantiate(playerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        FindObjectOfType<Camera>().GetComponent<CameraFollow>().m_follow = player.transform;
        SpawnCitizens();
        SpawnNextWave(gooberLimit);
        StartCoroutine(WaveObserver());
    }

    public void FindBuildings()
    {
    }

    void Update()
    {
        scoreText.text = "Score: " + score.ToString("F0");
    }

    private void RefreshScore()
    {

    }

    public void Celebrate()
    {
        foreach(GameObject c in Citizens)
        {
            c.GetComponent<Citizen>().StartCelebration();
        }
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
            Citizens.Add(a);
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

    IEnumerator WaveObserver()
    {
        if(GoobersAlive.Count == 0)
        {
            SpawnNextWave(gooberLimit);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(WaveObserver());
        //spawn after kill, max 3 at a time, after they are killed, spawn big one
    }

    public void SpawnNextWave(int amount)
    {
        waveNumber++;
        if(waveNumber >= 3)
        {
            //every 3 waves spawn big guy
            waveNumber = 0;
        } else
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject a = Instantiate(gooberPrefabs[UnityEngine.Random.Range(0, gooberPrefabs.Count)], new Vector3(UnityEngine.Random.Range(spawnBoundaries.x, spawnBoundaries.xx), 3, UnityEngine.Random.Range(spawnBoundaries.y, spawnBoundaries.yy)), Quaternion.identity);
                SpriteRenderer sr = a.GetComponentInChildren<SpriteRenderer>(); //a.transform.GetChild(1).GetComponent<SpriteRenderer>();
                sr.color = new Color(
                    sr.color.g - UnityEngine.Random.Range(0.1f, 0.3f),
                    sr.color.g - UnityEngine.Random.Range(0.1f, 0.3f),
                    sr.color.b - UnityEngine.Random.Range(0.1f, 0.3f));
                GoobersAlive.Add(a);
            }

        }
    }

    public void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject a = Instantiate(gooberPrefabs[UnityEngine.Random.Range(0, gooberPrefabs.Count)], new Vector3(UnityEngine.Random.Range(spawnBoundaries.x, spawnBoundaries.xx), 3, UnityEngine.Random.Range(spawnBoundaries.y, spawnBoundaries.yy)), Quaternion.identity);
            SpriteRenderer sr = a.GetComponentInChildren<SpriteRenderer>(); //a.transform.GetChild(1).GetComponent<SpriteRenderer>();
            sr.color = new Color(
                sr.color.g - UnityEngine.Random.Range(0.1f, 0.3f),
                sr.color.g - UnityEngine.Random.Range(0.1f, 0.3f),
                sr.color.b - UnityEngine.Random.Range(0.1f, 0.3f));
            GoobersAlive.Add(a);
        }
    }

    public void EndGame()
    {
        isGameFinished = true;
        gameEndPanel.gameObject.SetActive(true);
        scoreText.text = finalScoreText.ToString();
        foreach (GameObject c in Citizens)
        {
            c.GetComponent<Citizen>().StartCelebration();
        }
    }
}
