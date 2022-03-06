using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    // Das Geschossobjekt, dass erzeugt werden soll
    public GameObject bullet;

    // Array mit den Objekten, von denen aus geschossen werden kann
    public GameObject[] weaponPos;

    // Geschwindigkeit der Geschosse
    public float speed = 2000f;

    // Maximalzeit zwischen Schüssen
    public float maxTime = 5f;

    // Minimalzeit zwischen Schüssen
    public float minTime = 1f;

    // Objekt auf das geschossen werden soll
    GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        // Objekt mit dem "Player"-Tag wird als Ziel festgelegt
        target = GameObject.FindGameObjectWithTag("Player");

        // Ruft wiederholt die Shoot() Methodeauf. Erstmals nach 5 Sekunden und dann nach einer zufälligen Zeit zwischen der Min- und Maximalzeit
        InvokeRepeating("Shoot", 5, Random.Range(minTime, maxTime));
    }

    void Shoot()
    {
        // Wenn es ein Ziel gibt
        if(target != null)
        {
            // Erstellt zufällige Zahl von 0 bis zur Anzahl der angegebenen Waffen (0 in der Random.Range Methode ist inklusive und weaponPos.Length ist exklusive)
            int rng = Random.Range(0, weaponPos.Length);

            // Erstellt ein Geschossobjekt an der Position der Waffe mit dem zufälligen Index
            GameObject g = Instantiate(bullet, weaponPos[rng].transform.position, Quaternion.identity);

            // Aktiviert das Objekt am angegebenen Index (Spielt Schussanimation)
            weaponPos[rng].SetActive(true);

            // Rotiert Geschoss in richtung des Ziels
            g.transform.LookAt(target.transform.position);

            // Gibt dem Geschoss eine Kraft mit dem es in Richtung des Ziels fliegt
            g.GetComponent<Rigidbody>().AddForce(g.transform.forward * speed);

            // Startet Coroutine Wait() und übergibt ihr rng (WaitForSeconds kann nur in Coroutinen verwendet werden)
            StartCoroutine(Wait(rng));           
        }

    }

    IEnumerator Wait(int rng)
    {
        // Warte 0,5 sekunden
        yield return new WaitForSeconds(0.5f);

        // Deaktiviert das Waffenobjekt wieder
        weaponPos[rng].SetActive(false);
    }
}
