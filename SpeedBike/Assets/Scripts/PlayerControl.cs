using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {

    // Use this for initialization
    Rigidbody rb;
    int lane = 0;

    bool heldUp = false;
    bool heldDown = false;
    bool heldSpace = false;

    float currentZPos = 7.5f;

    bool grounded = true;

    bool alive = true;

    void Start()
    {
         rb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {

        //PHYSICS HERE

        if (grounded)
        {
            transform.position = new Vector3(transform.position.x, 4, transform.position.z);
        }
        else
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y - 1.2f, rb.velocity.z);
            if (transform.position.y <= 4)
            {
                grounded = true;
            }
        }


        if (alive)
        {
            if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            {
                this.accelerate();
            }
            else if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            {
                this.brake();
            }

            if (Input.GetKey("up"))
            {
                if (!heldUp)
                {
                    shift(-1);
                    heldUp = true;
                }
            }
            else { heldUp = false; }


            if (Input.GetKey("down"))
            {
                if (!heldDown)
                {
                    shift(1);
                    heldDown = true;
                }
            }
            else
            {
                heldDown = false;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                rotate(-0.1f);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rotate(0.1f);
            }


            //JUMP
            if (Input.GetKey(KeyCode.Space))
            {
                if (!heldSpace)
                {

                    heldSpace = true;
                    if (grounded)
                    {
                        grounded = false;
                        rb.velocity = new Vector3(rb.velocity.x, 30, rb.velocity.z);
                    }
                }
            }
            else
            {
                heldSpace = false;
            }
        }
        




        //FORCE Z AXIS POSITION
        rb.position = new Vector3(rb.position.x, rb.position.y, currentZPos);


        

    }

    public void ramp()
    {
        grounded = false;
        rb.velocity = new Vector3(rb.velocity.x, 50, rb.velocity.z);
    }
    public void kill()
    {
        rb.velocity = new Vector3(0,rb.velocity.y,rb.velocity.z);
        alive = false;
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
        float speedChange = (100f - rb.velocity.x) / 20f; 

        rb.velocity = new Vector3(rb.velocity.x + speedChange, rb.velocity.y, rb.velocity.z);
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
                currentZPos = 7.5f;
                break;
            case 1:
                currentZPos = 2.5f;
                break;
            case 2:
                currentZPos = -2.5f;
                break;
            case 3:
                currentZPos = -7.5f;
                break;
        }
    }

    private void rotate(float spin)
    {
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z + spin);
    }

}
