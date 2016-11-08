using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {

    const float GRAVITY = -2f;
    float targetVelocity = 40;

    // Use this for initialization
    Rigidbody rb;
    int lane = 0;

    bool heldUp = false;
    bool heldDown = false;
    bool heldSpace = false;

    float currentZPos = 7.5f;
    float endZPos = 7.5f;

    bool grounded = true;

    bool alive = true;

    void Start()
    {
         rb = GetComponent<Rigidbody>();
    }



    // Update is called once per frame
    void Update()
    {
        targetVelocity += 0.01f;

        //SHIFT LANES
        float tempDiff = endZPos - currentZPos;
        tempDiff= tempDiff / 3f;
        currentZPos = currentZPos + tempDiff;


        //PHYSICS HERE
        if (grounded)
        {
            transform.position = new Vector3(transform.position.x, 3.2f, transform.position.z);
            rb.rotation = new Quaternion(0, 0, 0, 1f);
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);

        }
        else
        {
            //GRAVITY
            float testZ = rb.transform.rotation.eulerAngles.z;
            float downForce = GRAVITY + (float)Math.Sin(2*(rb.transform.rotation.eulerAngles.z*Math.PI)/180f);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + downForce, rb.velocity.z);
            
            if (transform.position.y <= 3.1f && rb.velocity.y < 0)
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
                        rb.velocity = new Vector3(rb.velocity.x, 40, rb.velocity.z);
                    }
                }
            }
            else
            {
                heldSpace = false;
            }

            if (grounded)
            {
                if (rb.velocity.x < targetVelocity)
                {
                    rb.velocity = new Vector3(rb.velocity.x + 0.2f, rb.velocity.y, rb.velocity.z);
                }
                else if (rb.velocity.x > targetVelocity)
                {
                    rb.velocity = new Vector3(rb.velocity.x - 0.2f, rb.velocity.y, rb.velocity.z);
                }
            }
        }
        




        //FORCE Z AXIS POSITION
        rb.position = new Vector3(rb.position.x, rb.position.y, currentZPos);


        

    }


    private void brake()
    {
        if (rb.velocity.x >= 1)
        {
            if (grounded)
            {
                rb.velocity = new Vector3(rb.velocity.x - 2, rb.velocity.y, rb.velocity.z);
            }
        }
    }

    private void accelerate()
    {
        if (grounded)
        {
            float speedChange = (100f - rb.velocity.x) / 20f;

            rb.velocity = new Vector3(rb.velocity.x + speedChange, rb.velocity.y, rb.velocity.z);
        }
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
                endZPos = 7.5f;
                break;
            case 1:
                endZPos = 2.5f;
                break;
            case 2:
                endZPos = -2.5f;
                break;
            case 3:
                endZPos = -7.5f;
                break;
        }
    }

    private void rotate(float spin)
    {
        rb.angularVelocity = new Vector3(rb.angularVelocity.x, rb.angularVelocity.y, rb.angularVelocity.z + spin);
    }


    #region forgein calls
    public void ramp(int launchAngle)
    {
        grounded = false;
        rb.velocity = new Vector3(rb.velocity.x, 40, rb.velocity.z);
        rb.rotation = Quaternion.Euler(0, 0, launchAngle);  //new Quaternion(0f, 0f, 0.25f, 0.968246f);
        rb.angularVelocity = new Vector3(0, 0, -1f);
    }
    public void kill()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        alive = false;
    }
    public void boost(float power)
    {
        rb.velocity = new Vector3(rb.velocity.x + power, rb.velocity.y, rb.velocity.z);
    }
    #endregion


}
