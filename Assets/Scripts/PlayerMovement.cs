using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Rigidbody des Objekts
    Rigidbody rb;
    
    // Variablen für Horizontal, Vertikal und z-Achse
    float h, v, z;

    // Geschwindigkeit
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Weist rb den Rigidbody zu
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Weist h den Input für die Horizontal-Achse zu (Standart durch A und D Tasten)
        h = Input.GetAxis("Horizontal");

        // Weist h den Input für die Vertikal-Achse zu (Standart durch W und S Tasten)
        z = Input.GetAxis("Vertical");

        // Weist h den Input für die z-Achse zu
        v = GetUpDown();


        // Neuer Vektor in die Richtung des Input
        Vector3 dir = new Vector3(h, v, z);

        // Neuer Kurs des Spielerobjekts unter Einbezug des Input und der Geschwindigkeit
        rb.velocity = dir.normalized * speed;
    }

    private float GetUpDown()
    {
        // Wenn Leertaste gedrückt wird
        if (Input.GetKey(KeyCode.Space))
        {
            return 1f;
        }// Wenn linke Strg-Taste gedrückt wird
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            return -1f;
        }
        else // Wenn kein Input Erfolgt
        {
            return 0;
        }
    }
}
