using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    // Geschwindigkeit
    public float speed = 1f;
    
    // Rotationsgeschwidigkeit
    public float rotationSpeed = 1f;

    // Zielobjekt
    GameObject target;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    // Wegpunkt für dieses Objekt
    private GameObject thisWayPoint;

    // Index des Wegpunktes, den dieses Objekt belegt
    private int thisWayPointIndex;
    private float distance;
    List<int> list = new List<int>();


    // Start is called before the first frame update
    void Start()
    {   
        // Objekt mit dem "Player"-Tag wird als Ziel festgelegt 
        target = GameObject.FindGameObjectWithTag("Player");

        // FillList() Methode aufgerufen
        FillList();

        //ChooseWaypoint() Methode aufgerufen
        ChooseWaypoint();
    }

    // Füllt die Liste
    public void FillList()
    {
        // Für jeden vorhandenen Wegpunkt
        for(int i = 0; i < target.GetComponentInChildren<SpawnController>().wayPoints.Length; i++)
        {
            // Wegpunkt wird zur Liste hinzugefügt
            list.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Solange es ein Ziel gibt
        if(target != null)
        {
            // Aufrufen der FlyToWayPoint() Methode
            FlyToWayPoint();

            // Finde den Vektor von diesem Objekt zum Ziel
            _direction = (target.transform.position - transform.position).normalized;

            // Rotation um auf das Ziel zu schauen
            _lookRotation = Quaternion.LookRotation(_direction);

            // Rotiert dieses Objekt über Zeit mithilfe der Rotationsgeschwidigkeit, bis zu dem Punkt an dem auf das Ziel geschaut wird
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    // Methode zum finden eines freien Wegpunktes
    void ChooseWaypoint()
    {   
        // Für alle vorhandenen Wegpunkte
        for (int i = 0; i < target.GetComponentInChildren<SpawnController>().wayPoints.Length; i++)
        {
            // Erzeugt eine zufällige Zahl, die noch nicht vorher erzeugt wurde
            int x = GetNonRepeatRandom();

            // Wenn der Wegpunkt an dem Index dieser zufälligen Zahl noch nicht belegt ist
            if (!target.GetComponentInChildren<SpawnController>().isTaken[x])
            {
                // Der Wegpunkt wird als Wegpunkt für dieses Objekt gesetzt
                thisWayPoint = target.GetComponentInChildren<SpawnController>().wayPoints[x];

                // Setzt den gewählten Wegpunkt als belegt
                target.GetComponentInChildren<SpawnController>().SetIsTaken(x, true);

                // Speichert den Index des gewählten Wegpunktes
                thisWayPointIndex = x;
                return;
            }
        }
    }

    // Methode zum Bewegen vom Punkt der Erzeugung dieses Objektes zum Wegpunkt
    void FlyToWayPoint()
    {
        // Distanz zwischen dem Punkt der Erzeugung dieses Objektes und dem gewählten Wegpunkt
        distance = Vector3.Distance(thisWayPoint.transform.position, transform.position);
        
        // Interpolierte Bewegung zum Wegpunkt unter Einbeziehung der Geschwindigkeit und Distanz
        transform.position = Vector3.Lerp(transform.position, thisWayPoint.transform.position, speed * distance / 1000);
    }

    // Methode zum Abrufen des Index des gewählten Wegpunktes
    public int GetThisWayPointIndex()
    {
        return thisWayPointIndex;
    }

    // Methode zum erzeugen einer Zufälligen Zahl, die vorher noch nicht erzeugt wurde
    int GetNonRepeatRandom()
    {
        // Wenn die Liste leer ist, wird Zufallszahl zurückgegeben
        if (list.Count == 0)
        {
            return -1; // Maybe you want to refill
        }

        // Zufällige Zahl zwischen 0 und der Anzahl an Elementen in der Liste (Anzahl der vorhandenen Wegpunkte) wird erzeugt
        int rand = Random.Range(0, list.Count);

        // Element am Index der zufälligen Zahl wird in value gespeichert
        int value = list[rand];

        // Element am Index der zufälligen Zahl wird aus der Liste entfernt
        list.RemoveAt(rand);

        // value wird zurückgegeben
        return value;
    }
}
