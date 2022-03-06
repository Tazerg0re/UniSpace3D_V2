using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Wie viele Leben hat der Gegner
    public float health;
    
    // Animation die bei Zerstörung des Objekts gespielt wird
    public GameObject destructionAnim;
   
    // Wie viele Gegner sind gerade im Spiel
    static int enemiesAlive = 0;
    
    // Um wie viel erhöht sich der Score bei Kill
    public int scoreOnKill = 500;
    
    // Array das die Möglichen Gegenstände enthält, die bei Zerstörung fallen gelassen  werden können
    public GameObject[] pickUp;
    
    // Chance mit der die jeweiligen GEgenstände fallen Gelassen werden
    [Range(0, 100)]
    public int healthDropChance = 25;
    [Range(0, 100)]
    public int coinDropChance = 75;

    // Start is called before the first frame update
    void Start()
    {
        // Gegnerzähler wird hochgesetzt
        enemiesAlive++;
    }

    // Wenn eine Kollision erkannt wird
    private void OnCollisionEnter(Collision collision)
    {
        // Wenn die Kollision mit einen GameObject mit dem Tag "Bullet" erfolgt
        if (collision.transform.tag == "Bullet")
        {
            // aus dem GeschossObjekt wird ausgelesen, wie viel Schaden dieses verursacht
            float dmg = collision.gameObject.GetComponent<BulletController>().damage;
            
            // Schaden wird von den derzeitigen Leben abgezogen
            health -= dmg;
            
            // CheckDeath() Methode wird aufgerufen
            CheckDeath();
        }

        // Wenn die Kollision mit einen GameObject mit dem Tag "Asteroid" erfolgt
        if (collision.transform.tag == "Asteroid")
        {
            // aus dem Objekt wird ausgelesen, wie viel Schaden dieses verursacht
            float dmg = collision.gameObject.GetComponentInParent<AsteroidController>().damage;
           
            // Schaden wird von den derzeitigen Leben abgezogen
            health -= dmg;

            // CheckDeath() Methode wird aufgerufen
            CheckDeath();
        }
    }

    void CheckDeath()
    {
        // Wenn 0 oder weniger Leben
        if (health <= 0)
        {
            // Animation für Explosion wird erschaffen
            GameObject ded = Instantiate(destructionAnim, transform.position, Quaternion.identity);

            // Anzahl der Gegner im Spiel wird um 1 reduziert
            enemiesAlive--;

            // DropPickUp() Methode wird aufgerufen
            DropPickUp();

            // Erhöhe den Score um den angegebenen Wert
            GameManager.singleton.SetScore(GameManager.singleton.GetScore() + scoreOnKill);

            // Zerstört das Gegner Objekt
            Destroy(gameObject);

            // Zerstört die Explosionsanimation nach 5 sekunden
            Destroy(ded, 5);

            // Auf welchem Spawn-Punkt war dieses Objekt
            int index = gameObject.GetComponent<EnemyMovementController>().GetThisWayPointIndex();

            // Gibt den Spawn-Punkt an dem dieses Objekt war wieder frei
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SpawnController>().SetIsTaken(index, false);
        }
    }

    // Methode sorgt dafür, dass Gegenstände bei Zerstörung fallen gelassen werden können
    void DropPickUp()
    {
        // Zufällige Zahl zwischen 1 und 100
        int rndHealth = Random.Range(1, 100);

        // Wenn die Zufällige Zahl unterhalb der DropChance liegt
        if (rndHealth <= healthDropChance)
        {
            // Erzeuge das Objekt vom Index 0 des pickUp Arrays an der Position und mit der Rotation von diesem Objekt
            Instantiate(pickUp[0], transform.position, Quaternion.identity);
            return;
        }

        // Zufällige Zahl zwischen 1 und 100
        int rndCoin = Random.Range(1, 100);

        // Wenn die Zufällige Zahl unterhalb der DropChance liegt
        if (rndCoin <= coinDropChance)
        {
            // Erzeuge das Objekt vom Index 1 des pickUp Arrays an der Position und mit der Rotation von diesem Objekt
            GameObject coin = Instantiate(pickUp[1], transform.position, transform.rotation);
            
            // Rotiere den Coin um 90° um die x-Achse, damit er "aufrecht" steht
            coin.transform.Rotate(new Vector3(90, 0, 0));
            return;
        }

    }

    // Methode zum Abrufen, wie viele Gegner im Spiel sind
    public int GetEnemiesAlive()
    {
        return enemiesAlive;
    }
}
