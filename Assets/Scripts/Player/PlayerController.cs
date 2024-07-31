using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity
{
    [Space(10)]
    [Header("Player Movement Variables")]
    public float moveSpeed = 20f;
    public Rigidbody rb;

    //etc.



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
}
