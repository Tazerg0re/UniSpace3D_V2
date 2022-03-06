using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Geschwindigkeit
    public float speed;

    // Rotationsgeschwindigkeit
    public float rotSpeed;

    // Wert des Verbandskasten
    public int healthValue = 25;

    // Wert der Münze
    public int coinValue = 250;

    // Ist das Objekt eine Münze
    public bool isCoin;

    // Start is called before the first frame update
    void Start()
    {
        // Zerstört das Objekt nach 20 Sekunden
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        // Wenn das Objekt eine Münze ist
        if (isCoin)
        {
            // Rotiere das Objekt mit der Rotationsgeschwindigkeit über Zeit um die "Vorwärts-Achse"
            transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }
        else
        {
            // Rotiere das Objekt mit der Rotationsgeschwindigkeit über Zeit um die "Unten-Achse"
            transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
        }
        
        // Bewegt das Objekt unter Einbeziehung der Geschwindigkeit in negativer Richtung auf der z-Achse
        transform.position = transform.position + new Vector3(0, 0, -1 * speed);
    }

    // Wenn eine Kollision mit der Trefferbox registriert wird
    private void OnCollisionEnter(Collision collision)
    {
        // Wenn das Objekt mit dem Kollidiert wird den "Player"-Tag besitzt
        if (collision.transform.tag == "Player")
        {
            // Spielt den Soundeffekt aus dem Objekt mit dem "Audio"-Tag
            GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>().Play();
        }
    }
}
