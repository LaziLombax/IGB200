using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform player;
    public GameObject reefStage;
    public float offset;
    public Stage currentStage;
    public enum Stage
    {
        beach,
        reef
    }
    private void Start()
    {
        if (GameHandler.Instance.reefStage != null)
            reefStage = GameHandler.Instance.reefStage;
        player = PlayerController.Instance.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (currentStage == Stage.beach)
        {
            //transform.position.z = player.position.z;
            transform.position = new Vector3(transform.position.x, transform.position.y, player.position.z + offset);
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, transform.position.y, GameHandler.Instance.player);
        }
    }
}
