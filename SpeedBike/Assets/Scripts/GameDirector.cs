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

    public GameObject Prefab0;
    public GameObject RoadA1;
    public GameObject RoadA2;

    int trackEnd = 200;
    
    public float bikePos { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        #region SCORE
        bikePos = bike.GetComponent<Transform>().position.x;

        if (bikePos > prevX)
        {
            float distance = bikePos - prevX;

            score += distance;
            score += distance * bike.GetComponent<Rigidbody>().velocity.x;
            
            prevX = bike.GetComponent<Transform>().position.x;
        }

        long tempScore = (long)score;
        text.GetComponent<Text>().text = tempScore.ToString();
        #endregion


        #region trackGeneration
        while (bikePos > trackEnd - 150)
        {
            int track = Random.Range(0, 3);//Max rand value is EXCLUSIVE.
            GameObject go = null;
            switch (track)
            {
                case 0:
                    go = (GameObject)Instantiate(Prefab0);
                    break;
                case 1:
                    go = (GameObject)Instantiate(RoadA1);
                    break;
                case 2:
                    go = (GameObject)Instantiate(RoadA2);
                    break;
            }
            go.GetComponent<Transform>().position = new Vector3(trackEnd, 0f, 0f);
            trackEnd += 100;
            go.GetComponent<RoadScript>().director = this;
        }
        #endregion
    }
}
