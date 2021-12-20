using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Wie viele Leben hat der Gegner
    public float health;
    // Animation die bei Zerstörung gespielt wird
    public GameObject destructionAnim;
    static int enemiesAlive = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Leben auf 100 gesetzt
        health = 100f;
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
                enemiesAlive--;

                // Zerstört das Gegner Objekt
                Destroy(gameObject);
                
                // Zerstört die Explosionsanimation nach 5 sekunden
                Destroy(ded, 5);
                int index = gameObject.GetComponent<EnemyMovementController>().GetThisWayPointIndex();
                GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SpawnController>().SetIsTaken(index, false);
            }
        }
    }

    public int GetEnemiesAlive()
    {
        return enemiesAlive;
    }
}
