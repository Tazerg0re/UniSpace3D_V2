using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // GameObject an das die Position der Waffe beschreibt (Hier das Muzzle Object, das die Schussanimation abspiel) 
    public GameObject weaponPos;
    // Das Geschoss das erschaffen werden soll
    public GameObject bullet;
    // Geschwindigkeit des Geschosses
    public float speed = 2000f;
    // Rate of fire - Wie of hintereinander geschossen wird, wenn die Maus gedrückt gehalten wird
    public float rof = 0.1f;
    // Timer für die Schussfrequenz
    float timer = 0f;


    // FixedUpdate ist unabhängig von der FPS des Anwenders
    void FixedUpdate()
    {
        // Wenn linke Maustaste gedrückt wird
        if (Input.GetMouseButton(0))
        {
            // Aufrufen der Schuss Methode
            Shoot();
        }

    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            weaponPos.SetActive(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            weaponPos.SetActive(false);
        }
    }

    void Shoot()
    {
        // Erschaffen eines Strahls zur Positionsbestimmung des Mauszeigers
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Wenn der Strahl ein GameObject trifft
        if (Physics.Raycast(ray, out hit))
        {
            //// Gibt den Namen des geklickten GameObjects zurück (für Debug)            
            //Debug.Log(hit.transform.name);
            //// Zeichnet einen blauen Strahl zwischen der Waffe und dem getroffenem Punkt (für Debug)
            //Debug.DrawLine(weaponPos.transform.position, new Vector3(hit.point.x, hit.point.y, 500), Color.blue, 2.5f);

            // Timer addiert die vergangene Zeit seit dem letzten Frame
            timer += Time.deltaTime;

            // Wenn genug Zeit vergangen ist, kann ein neues Geschoss erzeugt werden
            if (timer >= rof)
            {
                // Erschaffe ein zugewiesenes GameObject and der Waffenposition
                GameObject g = Instantiate(bullet, weaponPos.transform.position, Quaternion.identity);
                // Rotiert Geschoss in richtung des geklickten Punktes
                g.transform.LookAt(hit.point);
                // Gibt dem Geschoss eine Kraft mit dem es in Richtung des Punktes fliegt
                g.GetComponent<Rigidbody>().AddForce(g.transform.forward * speed);
                // Zeit seit dem letzten Schuss wird zurückgesetzt
                timer = 0;
            }
        }



    }
}
