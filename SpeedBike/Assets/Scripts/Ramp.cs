using UnityEngine;
using System.Collections;

public class Ramp : MonoBehaviour {

    public int launchAngle;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerControl>().ramp(launchAngle);
        Debug.Log("RAMP HIT");
    }

}
