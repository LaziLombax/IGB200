using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Boat : MonoBehaviour
{
    private Vector3 TargetSpot;
    private Vector3 NetSpot;
    public GameObject Net;
    public float BoatSpeed;
    public float NetSpeed;
    private bool positionreached;
    private bool netpull;
    public float NetTime;
    public float HoldTime;
    public float HoldPlayerAbove;
    public GameObject Player;
    GameObject newNet;
    public LineRenderer lr;
    void Start(){
        float newz = Mathf.Abs(Random.Range(Player.gameObject.transform.position.z-20,gameObject.transform.position.z-20));
        TargetSpot = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, newz);
        NetSpot = new Vector3(gameObject.transform.position.x, Random.Range(-5f, 3f), gameObject.transform.position.z);
        newNet = Instantiate(Net, NetSpot, Net.transform.rotation);
        newNet.transform.parent = gameObject.transform;
    }
    void Update()
    {
        if (!GameHandler.Instance.timerOn) return;
        if (!positionreached){
            transform.position = Vector3.MoveTowards(transform.position, TargetSpot, (BoatSpeed * Time.deltaTime));
            if (Vector3.Distance(transform.position, TargetSpot) < 0.001f){
                StartCoroutine(NetCheck());
                positionreached = true;
            }
        }
        if (netpull){
            newNet.transform.position = Vector3.MoveTowards(newNet.transform.position, transform.position, (NetSpeed * Time.deltaTime));
            if (Vector3.Distance(transform.position, transform.position) < 0.001f){
                netpull = false;
                StartCoroutine(HoldPlayer());
            }
        }
    }

    void LateUpdate()
    {
    }  

    IEnumerator NetCheck(){
        //hold player here
        yield return new WaitForSeconds(NetTime);
        netpull = true;
        //move player up
    }

    IEnumerator HoldPlayer(){
        yield return new WaitForSeconds(HoldTime);
        Destroy(gameObject, 10f);
    }
}
