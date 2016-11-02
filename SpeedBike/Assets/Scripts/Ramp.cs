using UnityEngine;
using System.Collections;

public class Ramp : MonoBehaviour {


    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerControl>().ramp();
        Debug.Log("RAMP HIT");
    }

}
