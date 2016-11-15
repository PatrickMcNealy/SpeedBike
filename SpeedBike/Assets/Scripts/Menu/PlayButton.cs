using UnityEngine;
using System.Collections;

public class PlayButton : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
	    
	}

   public void OnClick()
    {
        Debug.Log("PLAY BUTTON IS CLICK.");
        Application.LoadLevel("Gameplay Scene");
    }
}
