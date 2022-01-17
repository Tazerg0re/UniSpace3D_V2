using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    public float speed = 1f;    
    public float rotationSpeed = 1f;
    GameObject target;
    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;
    private GameObject thisWayPoint;
    private int thisWayPointIndex;
    private float distance;


    // Start is called before the first frame update
    void Start()
    {        
        target = GameObject.FindGameObjectWithTag("Player");
        ChooseWaypoint();
        //wayPoints = new Transform[target.transform.GetChild(4).transform.childCount];
        //for (int i = 0; i < target.transform.GetChild(4).transform.childCount; i++)
        //{           
        //    wayPoints[i] = target.transform.GetChild(4).transform.GetChild(i);
        //}
        //wayPoint = wayPoints[gameObject.GetComponent<EnemyController>().GetEnemiesAlive()];
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            FlyToWayPoint();

            //find the vector pointing from our position to the target
            _direction = (target.transform.position - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void ChooseWaypoint()
    {       
        for (int i = 0; i < target.GetComponentInChildren<SpawnController>().wayPoints.Length; i++)
        {
            if (!target.GetComponentInChildren<SpawnController>().isTaken[i])
            {
                thisWayPoint = target.GetComponentInChildren<SpawnController>().wayPoints[i];
                target.GetComponentInChildren<SpawnController>().SetIsTaken(i, true);
                thisWayPointIndex = i;
                return;
            }
        }
    }
    void FlyToWayPoint()
    {
        distance = Vector3.Distance(thisWayPoint.transform.position, transform.position);    
        transform.position = Vector3.Lerp(transform.position, thisWayPoint.transform.position, speed * distance / 1000);
    }

    public int GetThisWayPointIndex()
    {
        return thisWayPointIndex;
    }
}
