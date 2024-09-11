using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolChange : MonoBehaviour
{
    public Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
        RandomPatrol();
    }
    public void RandomPatrol()
    {
        Vector3 newPos = startPos;
        newPos.z += Random.Range(-3, 3);
        transform.position = newPos;
    }
}
