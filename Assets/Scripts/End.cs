using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public bool win;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameHandler.Instance.player)
        {
            GameHandler.Instance.CompleteLevel(win);
        }
    }
}
