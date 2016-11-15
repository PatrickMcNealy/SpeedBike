using UnityEngine;
using System.Collections;

public class DeathWall : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerControl>().kill();
        Debug.Log("DEATHWALL HIT");
    }

}
