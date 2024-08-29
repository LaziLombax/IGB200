using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerBeach>())
        {
            PlayerBeach player = other.gameObject.GetComponent<PlayerBeach>();
            player.hitBarrier = true;
            player.moveDir = player.lastPosition;
        }
    }
}
