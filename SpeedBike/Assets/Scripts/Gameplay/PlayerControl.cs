using UnityEngine;
using System.Collections;
using System;

public class PlayerControl : MonoBehaviour {

    const float GRAVITY = -2f;
    public float targetVelocity { get; private set; } 

    // Use this for initialization
    Rigidbody rb;
    int lane = 0;

    bool heldUp = false;
    bool heldDown = false;
    bool heldSpace = false;

    float currentZPos = 7.5f;
    float endZPos = 7.5f;

    bool grounded = true;

    public bool alive { get; private set; }

    float minSwipeDistY;
    private Vector2 topPos;
    private Vector2 botPos;
    int lastDirection = 0;

    void Start()
    {
        minSwipeDistY = Screen.height / 10f;
        alive = true;
         rb = GetComponent<Rigidbody>();
        targetVelocity = 50;
    }



    // Update is called once per frame
    void Update()
    {
        int swipeDirection = 0;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastDirection = 0;
                    topPos = touch.position;
                    botPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    float swipeDistUp = touch.position.y - botPos.y;
                    float swipeDistDown = touch.position.y - topPos.y;

                    if (swipeDistUp > 0)
                    {
                        topPos = touch.position;
                        if (lastDirection != 1 && swipeDistUp > minSwipeDistY)
                        {
                            lastDirection = 1;
                            swipeDirection = 1;
                        }
                        if(swipeDistUp > minSwipeDistY * 4f)
                        {
                            swipeDirection = 2;
                        }
                    }

                    if (swipeDistDown < 0)
                    {
                        botPos = touch.position;
                        if (lastDirection != -1 && swipeDistDown < -1 * minSwipeDistY)
                        {
                            lastDirection = -1;
                            swipeDirection = -1;
                        }
                        if (swipeDistDown < minSwipeDistY * -4f)
                        {
                            swipeDirection = -2;
                        }
                    }

                    //if (swipeDistVertical > minSwipeDistY)
                    //{
                    //    float swipeValue = Mathf.Sign(touch.position.y - startPos.y);

                    //    if (swipeValue > 0)
                    //    {

                    //        
                    //    }

                    //    else if (swipeValue < 0)
                    //    {
                    //        lastDirection = -1;
                    //        swipeDirection = -1;
                    //    }
                    //}
                    break;
            }
        }



        targetVelocity += 0.02f;

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
            if (Input.GetKey("up") || swipeDirection == 1)
            {
                if (!heldUp)
                {
                    shift(-1);
                    heldUp = true;
                }
            }
            else { heldUp = false; }
            
            if (Input.GetKey("down") || swipeDirection == -1)
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

            if(swipeDirection == 2)
            {
                shift(-3);
            }
            else if(swipeDirection == -2)
            {
                shift(3);
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

            if (rb.velocity.x < targetVelocity)
            {
                rb.velocity = new Vector3(rb.velocity.x + 0.5f, rb.velocity.y, rb.velocity.z);
            }
            else if (rb.velocity.x > targetVelocity)
            {
                if (grounded)
                {
                    rb.velocity = new Vector3(rb.velocity.x - 1f, rb.velocity.y, rb.velocity.z);
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x - 0.3f, rb.velocity.y, rb.velocity.z);
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
        rb.position = new Vector3(rb.position.x, rb.position.y + 2.5f, rb.position.z);
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

    public void slow(float power)
    {
        rb.velocity = new Vector3(rb.velocity.x - power, rb.velocity.y, rb.velocity.z);
    }
    #endregion


}
