using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnController : MonoBehaviour
{
    public GameObject enemyPrefab;    
    public GameObject[] spawnPoints;
    public GameObject[] wayPoints;
    public bool[] isTaken;
    public float minSpawnTime = 5f;
    public float maxSpawnTime = 10f;    

    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {        
        
    }

    private void Update()
    {
        float interval = Random.Range(minSpawnTime, maxSpawnTime);
        int enmiesAlive = enemyPrefab.GetComponent<EnemyController>().GetEnemiesAlive();
        for (int i = 0; i < isTaken.Length; i++)
        {
            if (!isTaken[i] && timer >= interval)
            {
                SpawnEnemy();
                timer = 0;
            }            
        }
        timer += Time.deltaTime;
    }

    void SpawnEnemy()
    {
        int rnd = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[rnd].transform.position, transform.rotation);
    }

    public void SetIsTaken(int index, bool b)
    {
        isTaken[index] = b;
    }
}


