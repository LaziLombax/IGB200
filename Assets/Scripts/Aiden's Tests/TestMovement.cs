using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestMovement : MonoBehaviour
{
    private Vector3 NewPos;
    void Start(){
        NewPos = transform.position;
    }
    void Update(){
        if (Input.GetKeyDown("left")){
            NewPos = gameObject.transform.position + new Vector3(-1, 0, 0);
            MovePlayer();
        }else if (Input.GetKeyDown("right")){
            NewPos = gameObject.transform.position + new Vector3(1, 0, 0);
            MovePlayer();
        }
        else if (Input.GetKeyDown("up")){
            NewPos = gameObject.transform.position + new Vector3(0, 0, 1);
            MovePlayer();
        }else if (Input.GetKeyDown("down")){
            NewPos = gameObject.transform.position + new Vector3(0, 0, -1);
            MovePlayer();
        }
    }

    private void MovePlayer(){
        Collider[] intersecting = Physics.OverlapSphere(NewPos, 0.01f, ~LayerMask.GetMask("Player"));
        if (intersecting.Length == 0){
            gameObject.transform.position = NewPos;
        }
    }
}
