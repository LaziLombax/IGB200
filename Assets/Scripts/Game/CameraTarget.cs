using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    private Transform player;
    public float offset;

    private void Start()
    {
        player = PlayerController.Instance.transform;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position.z = player.position.z;
        transform.position = new Vector3 (transform.position.x,transform.position.y,player.position.z + offset);
    }
}
