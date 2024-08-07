using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public int camerachange = 0;
    public GameObject Player;
    Vector3 offset;
    float camx;
    float camy;
    float camz;
    float camrot;

    void Start(){
        Player = GameObject.Find("TestPlayer");
    }
    void FixedUpdate(){
        Vector3 CamTargetPos = Player.transform.position + offset;
        transform.position = Vector3.Lerp(transform.position, CamTargetPos, 5.0f * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(camrot,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)){
            camerachange += 1;
        }

        switch(camerachange){
            case 0:
                offset = new Vector3(0,0,-10);
                camrot = 0;
                break;

            case 1:
                offset = new Vector3(0,10,-10);
                camrot = 30;
                break;
            
            case 2:
                offset = new Vector3(0,5,-10);
                camrot = 15;
                break;

            case 3:
                offset = new Vector3(0,20,-5);
                camrot = 60;
                break;

            case 4:
                offset = new Vector3(0,10,-5);
                camrot = 60;
                break;
            
            case 5:
                offset = new Vector3(0,5,-2);
                camrot = 60;
                break;

            case 6:
                offset = new Vector3(0,2,-5);
                camrot = 30;
                break;

            default:
                camerachange = 0;
                break;
        }
    }
}
