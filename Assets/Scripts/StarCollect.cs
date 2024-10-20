using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameHandler.Instance.currentLevelData.levelGold += 5;
            Destroy(gameObject);
        } else if (other.gameObject.tag == "Obstacles")
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Obstacles")
        {
            //Destroy(gameObject);
        }
    }
}
