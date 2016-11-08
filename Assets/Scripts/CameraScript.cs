using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public GameObject bike;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(bike.transform.position.x + 35, this.transform.position.y, this.transform.position.z);
	}
}
