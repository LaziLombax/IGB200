using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerReef : PlayerController
{
    [Space(10)]
    [Header("Reef Movement Variables")]
    public Vector3 moveInput;

    private void FixedUpdate()
    {
        if (gameHandler.gameEnded) return;
        MovePlayer();
        SpeedControl();

    }


    private void StateHandler()
    {
        if (gotHit)
        {
            state = MovementState.damaged;
        }
        else
        {
            state = MovementState.moving;
        }
    }

    public override void MovePlayer()
    {

        Vector3 screenMove;
        if (inputHandler.GetSpeedInput())
        {
            screenMove = new Vector3(0, 0, gameHandler.stageSpeed * gameHandler.speedUp * Time.fixedDeltaTime);
        }
        else
        {
            screenMove = new Vector3(0, 0, gameHandler.stageSpeed * Time.fixedDeltaTime);
        }
        rb.MovePosition(rb.position + screenMove);
        Vector3 force = moveInput * moveSpeed * 10;
        rb.AddForce(force, ForceMode.Force);
    }

    public override void PlayerInput()
    {
        Vector2 playerMovement = inputHandler.GetPlayerMovement();
        if (playerMovement.y > gameHandler.stageTop - transform.position.y) 
        {
            playerMovement.y = 0;
        }
        if (inputHandler.GetSpeedInput())
        {
            gameHandler.isSpeed = true;
        }
        else
        {
            gameHandler.isSpeed = false;
        }
        moveInput = new Vector3(0f, playerMovement.y, playerMovement.x);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3( 0f, rb.velocity.y, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(0f, limitedVel.y, limitedVel.z);
        }
    }
}
