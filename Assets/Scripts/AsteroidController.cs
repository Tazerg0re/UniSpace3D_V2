using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    // Array mit den verschiedenen Asteroidentypen
    public GameObject[] asteroidType;
  
    // Geschwindigkeit der Asteroiden
    public float speed = 0.05f;
   
    // Vector3-Variable mit der Zielgröße, die der Asteroid am Ende haben soll
    public Vector3 targetScale = Vector3.one * 15f;
   
    // Startgröße der Asteroiden
    Vector3 startScale;
   
    // Über welchen Zeitraum der Asteroid auf die Zielgröße wächst
    public float ScaleDuration = 5f;
   
    // Nach welcher Zeit das Object aus dem Spiel entfernt werden soll
    public float destroyAfter = 15f;
   
    // Variable zur Zeiterfassung
    float t = 0;
  
    // Ziel, auf das die Asteroiden zufliegen sollen
    GameObject target;
  
    // Schaden, den eine Kollision mit einem Asteroiden zufügt
    public float damage = 200f;

    // Start is called before the first frame update
    void Start()
    {
        // Erzeuge ein Zufälligen Asteroiden aus dem Array, an der Position und mit der Orientierung des Objektes mit diesem Skript und erzeuge ihn als Kindobjekt
        Instantiate(asteroidType[Random.Range(0, asteroidType.Length - 1)], transform.position, transform.rotation, transform);
       
        // Setze die Startgröße auf die größe des erzeugten Asteoriden
        startScale = gameObject.GetComponentInChildren<Transform>().localScale;

        // Das Objekt mit dem Tag "Player" wird als Ziel genommen
        target = GameObject.FindGameObjectWithTag("Player");

        // Rotiert den Asteoriden in Richtung des Ziels
        transform.LookAt(target.transform);

        // Entfernt den Asteroiden nach der vorher bestimmten Zeit
        Destroy(gameObject, destroyAfter);
    }

    // Update is called once per frame
    void Update()
    {
        // Die Zeit seit dem letzten Frame wird aufaddiert und die Zeit dividiert, während der der Asteroid wachsen soll
        t += Time.deltaTime / ScaleDuration;

        // Solange die angegebene Zeit noch nicht erreicht ist, wird jeden Frame zwischen der Anfangs- und der Zielgröße interpoliert
        if (t < 1)
        {
            Vector3 newScale = Vector3.Lerp(startScale, targetScale, t);
            gameObject.GetComponentInChildren<Transform>().localScale = newScale;
        }

        // Die Position des Asteroiden wird jeden Frame nach vorne (in Zielrichtung), in der angegebenen Geschwidigkeit, verändert
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
