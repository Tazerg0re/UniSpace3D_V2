using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    float h, v, z;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        v = GetUpDown();



        Vector3 dir = new Vector3(h, v, z);
        rb.velocity = dir.normalized * speed;
    }

    private float GetUpDown()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return 1f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            return -1f;
        }
        else
        {
            return 0;
        }
    }
}
