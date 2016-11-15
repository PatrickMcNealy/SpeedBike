using UnityEngine;
using System.Collections;

public class RoadScript : MonoBehaviour {
    

    public GameDirector director;
	
	// Update is called once per frame
	void Update () {
	    if(director.bikePos > this.transform.position.x + 100)
        {
             Destroy(gameObject);
        }
	}
}
