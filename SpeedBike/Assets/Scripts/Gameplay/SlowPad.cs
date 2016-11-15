using UnityEngine;
using System.Collections;

public class SlowPad : MonoBehaviour {

    public int slowPower;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerControl>().slow(slowPower);
        Debug.Log("SLOW HIT");
    }
}
