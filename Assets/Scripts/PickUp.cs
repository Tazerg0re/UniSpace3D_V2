using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    public int healthValue = 25;
    public int coinValue = 250;
    public bool isCoin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoin)
        {
            transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.down * rotSpeed * Time.deltaTime);
        }
        
        transform.position = transform.position + new Vector3(0, 0, -1 * speed);
    }
}
