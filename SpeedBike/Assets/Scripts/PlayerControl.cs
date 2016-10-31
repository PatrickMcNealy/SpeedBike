using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {

    // Use this for initialization
    Rigidbody rb;
    int lane = 0;

    void Start()
    {
         rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("right"))
        {
            this.accelerate();
        }
        else if (Input.GetKey("left"))
        {
            this.brake();
        }

        if (Input.GetKey("up"))
        {
            shift(-1);
        }

        if (Input.GetKey("down"))
        {
            shift(1);
        }
    }

    private void brake()
    {
        if (rb.velocity.x >= 1) //ALSO IF GROUNDED
        {
            rb.velocity = new Vector3(rb.velocity.x - 2, rb.velocity.y, rb.velocity.z);
        }
    }

    private void accelerate()
    {
        rb.velocity = new Vector3(rb.velocity.x + 1, rb.velocity.y, rb.velocity.z);
    }

    private void shift(int i)
    {
        lane += i;
        if(lane < 0)
        {
            lane = 0;
        }
        else if(lane > 3)
        {
            lane = 3;
        }

        rb.constraints = RigidbodyConstraints.None;

        switch (lane)
        {
            case 0:
                rb.position = new Vector3(rb.position.x, rb.position.y, 7.5f);
                break;
            case 1:
                rb.position = new Vector3(rb.position.x, rb.position.y, 2.5f);
                break;
            case 2:
                rb.position = new Vector3(rb.position.x, rb.position.y, -2.5f);
                break;
            case 3:
                rb.position = new Vector3(rb.position.x, rb.position.y, -7.5f);
                break;
        }

        rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }
}
