using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawn : MonoBehaviour
{
    // Das Objekt, das die Asteroiden enthält
    public GameObject asteroid;

    // Minimalzeit nach der ein neuer Asteroid erzeugt werden kann
    public float minSpawnTime = 5f;

    // Maximalzeit, bis ein neuer Asteroid erzeugt werden kann
    public float maxSpawnTime = 10f;

    // Timer um die Zeit zu messen
    float timer = 0f;

    // Bereich auf der X-Achse, an dem der Asteroid erzeugt werden  kann
    public float offsetX = 80f;

    // Bereich auf der X-Achse, an dem der Asteroid erzeugt werden  kann
    public float offsetY = 60f;

    // Update is called once per frame
    void Update()
    {
        // Eine zufällige Zahl zwischen der min- und maximalzeit wird erzeugt
        float interval = Random.Range(minSpawnTime, maxSpawnTime);    
        
        // Prüfe ob die Zufällige Zeit schon vergangen ist, wenn ja, Rufe die Methode "SpawnAsteroid()" auf un setzte den Timer zurück
            if (timer >= interval)
            {
                SpawnAsteroid();
                timer = 0;
            }

        // Addiert die vergangene Zeit seit dem letzten Frame auf
        timer += Time.deltaTime;
    }

    // Methode zum erzeugen der Asteroiden
    void SpawnAsteroid()
    {
        // Ein neuer Vector3, der eine zufällige Position innerhalb des vorher festgelegten  offsets enthält
        Vector3 spawnOffset = new Vector3(Random.Range(-offsetX, offsetX), Random.Range(-offsetY, offsetY), 0);

        // Asteroidobjekt wird an der Position des Objektes mit diesem Skript inklusive des offsets erzeugt
        Instantiate(asteroid, transform.position + spawnOffset, transform.rotation);
    }
}
