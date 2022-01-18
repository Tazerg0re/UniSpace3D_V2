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
    List<int> list = new List<int>();


    // Start is called before the first frame update
    void Start()
    {        
        target = GameObject.FindGameObjectWithTag("Player");
        FillList();
        ChooseWaypoint();
        //wayPoints = new Transform[target.transform.GetChild(4).transform.childCount];
        //for (int i = 0; i < target.transform.GetChild(4).transform.childCount; i++)
        //{           
        //    wayPoints[i] = target.transform.GetChild(4).transform.GetChild(i);
        //}
        //wayPoint = wayPoints[gameObject.GetComponent<EnemyController>().GetEnemiesAlive()];
    }

    public void FillList()
    {
        for(int i = 0; i < target.GetComponentInChildren<SpawnController>().wayPoints.Length; i++)
        {
            list.Add(i);
        }
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
            int x = GetNonRepeatRandom();
            if (!target.GetComponentInChildren<SpawnController>().isTaken[x])
            {
                thisWayPoint = target.GetComponentInChildren<SpawnController>().wayPoints[x];
                target.GetComponentInChildren<SpawnController>().SetIsTaken(x, true);
                thisWayPointIndex = x;
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

    int GetNonRepeatRandom()
    {
        if (list.Count == 0)
        {
            return -1; // Maybe you want to refill
        }
        int rand = Random.Range(0, list.Count);
        int value = list[rand];
        list.RemoveAt(rand);
        return value;
    }
}
