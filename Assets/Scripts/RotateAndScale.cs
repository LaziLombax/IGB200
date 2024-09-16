using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndScale : MonoBehaviour
{
    private Vector3 scaleMax;
    private Vector3 scaleMin;
    private Vector3 rotationToUse;
    
    public float rotationSpeed = 2f;

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
    }
}
