using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameDirector : MonoBehaviour {

    Random rand = new Random();

    float score = 0;

    float prevX = -10f;

    public GameObject bike;
    public GameObject text;
    public GameObject textvel;

    public GameObject[] SetA;
    public GameObject[] SetB;

    int set;

    int trackEnd = 200;

    //Nick's variables for velocity, noted to find easily
    private float velocity;
    private string velocityText;

    public float bikePos { get; private set; }

	// Use this for initialization
	void Start () {
        set = 0;
    }
	
	// Update is called once per frame
	void Update () {
        #region SCORE+VELOCITY
        bikePos = bike.GetComponent<Transform>().position.x;

        if (bikePos > prevX)
        {
            float distance = bikePos - prevX;

            score += distance;
            score += distance * bike.GetComponent<Rigidbody>().velocity.x;
            
            prevX = bike.GetComponent<Transform>().position.x;


        }

        if (velocity >= 0)
        {
            //gets bikes current velocity
            velocity = bike.GetComponent<Rigidbody>().velocity.x;
        }

        long tempScore = (long)score;
        text.GetComponent<Text>().text = tempScore.ToString();

        //Multiple conversions to get ints and strings to play together and display current velocity
        int velocityNum = (int)velocity;
        velocityText = "MPH: " + velocityNum;
        textvel.GetComponent<Text>().text = velocityText.ToString();
        #endregion


        #region trackGeneration

        if (set == 0)
        {
            if (bike.GetComponent<PlayerControl>().targetVelocity > 80)
            {
                set = 1;
            }
        }

        while (bikePos > trackEnd - 150)
        {
            GameObject[] currentSet = null;
            switch (set)
            {
                case 0:
                    currentSet = SetA;
                    break;
                case 1:
                    currentSet = SetB;
                    break;
            }

            int track = Random.Range(0, currentSet.Length);//Max rand value is EXCLUSIVE.
            GameObject go = (GameObject)Instantiate(currentSet[track]);



            go.GetComponent<Transform>().position = new Vector3(trackEnd, 0f, 0f);
            trackEnd += 100;
            go.GetComponent<RoadScript>().director = this;
        }
        #endregion
    }
}
