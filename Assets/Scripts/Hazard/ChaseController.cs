using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseController : MonoBehaviour
{
    [Space(10)]
    [Header("Variables")]
    public ChaseNav myChaser;
    private void OnTriggerEnter(Collider other)
    {
        myChaser.myState = ChaseNav.ChaseState.Chase;
        myChaser.target = other.transform;
    }
    private void OnTriggerExit(Collider other)
    {
        myChaser.myState = ChaseNav.ChaseState.Start;
        myChaser.target = null;
    }
}
