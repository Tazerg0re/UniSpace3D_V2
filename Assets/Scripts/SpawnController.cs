using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnController : MonoBehaviour
{
    // Gegneronjekt
    public GameObject enemyPrefab;

    // Array mit den vorhandenen Spawnpoints
    public GameObject[] spawnPoints;

    // Array mit den vorhandenen Wegpunkten
    public GameObject[] wayPoints;

    // Array ob ein Punkt bereits belegt ist
    public bool[] isTaken;

    // Minimalzeit für die Erzeugung eines neuen Gegners
    public float minSpawnTime = 5f;

    // Maximalzeit für die Erzeugung eines neuen Gegners
    public float maxSpawnTime = 10f;    

    // Timer
    float timer = 0f;

    // Wird jeden Frame aufgerufen
    private void Update()
    {
        // Zufällige Zahl zwischen der Min- und Maximalzeit
        float interval = Random.Range(minSpawnTime, maxSpawnTime);

        // Ruft ab, wie viele Gegner sich im Spiel befinden
        int enmiesAlive = enemyPrefab.GetComponent<EnemyController>().GetEnemiesAlive();

        // Für jedes Element in isTaken
        for (int i = 0; i < isTaken.Length; i++)
        {
            // Wenn das Element am Index i nicht belegt ist und der Timer das Intervall erreicht hat
            if (!isTaken[i] && timer >= interval)
            {
                // Aufrufen der SpawnEnemy() Methode
                SpawnEnemy();

                // Timer wird zurückgesetzt
                timer = 0;
            }            
        }

        // Timer wird mit der vergangenen Zeit seit dem letzten Frame aufaddiert
        timer += Time.deltaTime;
    }

    // Erzeugt einen Gegner an einem zufälligen Spawnpoint
    void SpawnEnemy()
    {
        int rnd = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[rnd].transform.position, transform.rotation);
    }

    // Setzt den Wegbunkt am Übergebenen Index auf belegt/unbelegt
    public void SetIsTaken(int index, bool b)
    {
        isTaken[index] = b;
    }
}


