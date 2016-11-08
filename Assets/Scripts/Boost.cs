using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {

    public int boostPower;

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerControl>().boost(boostPower);
        Debug.Log("BOOST HIT");
    }

}
