using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject[] weaponPos;
    public float speed = 2000f;
    public float maxTime = 5f;
    public float minTime = 1f;
    GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        InvokeRepeating("Shoot", 5, Random.Range(minTime, maxTime + 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        if(target != null)
        {
            int rng = Random.Range(0, 2);
            // Erschaffe ein zugewiesenes GameObject and der Waffenposition
            GameObject g = Instantiate(bullet, weaponPos[rng].transform.position, Quaternion.identity);
            weaponPos[rng].SetActive(true);
            // Rotiert Geschoss in richtung des geklickten Punktes
            g.transform.LookAt(target.transform.position);
            // Gibt dem Geschoss eine Kraft mit dem es in Richtung des Punktes fliegt
            g.GetComponent<Rigidbody>().AddForce(g.transform.forward * speed);
            weaponPos[rng].SetActive(false);
        }

    }
}
