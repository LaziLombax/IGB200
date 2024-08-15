using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBeach : PlayerController
{
    [Space(10)]
    [Header("Beach Movement Variables")]
    public bool isMoving;
    public Vector3 moveDir;
    public float moveTime = 0.1f;
    public float moveDistance;

    private void Start()
    {
        moveDir = transform.position;
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            MovePlayer();
        }   
    }
    public override void MovePlayer()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, moveDir, moveTime * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, moveDir) <= 0.01f)
        {
            rb.MovePosition(moveDir);
            isMoving = false;
        }
    }

    public override void PlayerInput()
    {
        
        if (inputHandler.BeachMoveForward() && !isMoving)
        {
            isMoving = true;
            moveDir += Vector3.forward * moveDistance;
        }

        if (inputHandler.BeachMoveLeft() && !isMoving)
        {
            if (moveDir.x - moveDistance < gameHandler.rowWidth *-1) return;
            isMoving = true;
            moveDir += Vector3.left * moveDistance;
        }
        if (inputHandler.BeachMoveRight() && !isMoving)
        {
            if (moveDir.x + moveDistance > gameHandler.rowWidth) return;
            isMoving = true;
            moveDir += Vector3.right * moveDistance;
        }
    }
}
