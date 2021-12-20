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
        // Geschoss wird nach einer Gewissen Zeit automatisch zerstört
        Destroy(gameObject, timeToLive);
        Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
    }

    // Wenn eine Kollision mit einem anderen Object erfolgt
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.name == transform.root.name)
        {
            return;
        }

        // Spiele Treffer Effekt an der Position der Kollision
        GameObject h = Instantiate(hit, transform.position, Quaternion.identity);
        // Zerschtöre das Geschoss Object
        Destroy(gameObject);
        // Zerstöre  den Effekt nach 5 Sekunden
        Destroy(h, 5);
    }
}
