using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Wie viele Leben hat der Gegner
    public float health;
    // Animation die bei Zerstörung gespielt wird
    public GameObject destructionAnim;
    // Wie viele Gegner sind gerade im Spiel
    static int enemiesAlive = 0;
    // Um wie viel erhöht sich der Score bei Kill
    public int scoreOnKill = 500;
    public GameObject[] pickUp;
    [Range(0, 100)]
    public int healthDropChance = 25;
    [Range(0, 100)]
    public int coinDropChance = 75;

    // Start is called before the first frame update
    void Start()
    {
        enemiesAlive++;
        Debug.Log(enemiesAlive);
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

            // Wenn 0 oder weniger Leben
            if (health <= 0)
            {
                // Animation für Explosion wird erschaffen
                GameObject ded = Instantiate(destructionAnim, transform.position, Quaternion.identity);

                // Anzahl der Gegner im Spiel wird um 1 reduziert
                enemiesAlive--;

                DropPickUp();

                // Erhöhe den Score um 100
                GameManager.singleton.SetScore(GameManager.singleton.GetScore() + scoreOnKill);

                // Zerstört das Gegner Objekt
                Destroy(gameObject);
                               
                // Zerstört die Explosionsanimation nach 5 sekunden
                Destroy(ded, 5);
                int index = gameObject.GetComponent<EnemyMovementController>().GetThisWayPointIndex();
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SpawnController>().SetIsTaken(index, false);
            }
        }
    }

    void DropPickUp()
    {
        int rndHealth = Random.Range(1, 100);
        if(rndHealth <= healthDropChance)
        {
            Instantiate(pickUp[0], transform.position, Quaternion.identity);
            return;
        }

        int rndCoin = Random.Range(1, 100);
        if (rndCoin <= coinDropChance)
        {
            GameObject coin = Instantiate(pickUp[1], transform.position, transform.rotation);
            coin.transform.Rotate(new Vector3(90, 0, 0));
            return;
        }

    }

    public int GetEnemiesAlive()
    {
        return enemiesAlive;
    }
}
