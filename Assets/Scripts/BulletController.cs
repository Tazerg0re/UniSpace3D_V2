using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Wie lange bleibt das geschoss im Spiel befor es zerstört wird
    public float timeToLive = 5f;
    
    // Effekt, wenn ein Treffer erfolgt
    public GameObject hit;
    
    // Schaden pro Treffer
    public float damage = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Geschoss wird nach der festgelegten Zeit automatisch zerstört
        Destroy(gameObject, timeToLive);      
    }

    // Wenn eine Kollision mit einem anderen Objekt erfolgt
    private void OnCollisionEnter(Collision collision)
    {
        // Ignoriert die Kollision von dem Geschoss mit der Hitbox des Raumschiffes, dass es abschießt
        if (collision.transform.name == transform.root.name)
        {
            return;
        }

        // Spiele Treffer Effekt (kleine Explosion) an der Position der Kollision
        GameObject h = Instantiate(hit, transform.position, Quaternion.identity);
        
        // Zerschtöre das Geschoss Object
        Destroy(gameObject);
        
        // Zerstöre  den Effekt nach 5 Sekunden
        Destroy(h, 5);
    }
}
