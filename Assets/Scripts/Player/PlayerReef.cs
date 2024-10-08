using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerReef : PlayerController
{
    [Space(10)]
    [Header("Reef Movement Variables")]
    public Vector3 moveInput;
    public GameObject oilOverlay;
    private AudioSource oilSplat;
    public float fadeInkTimer;

    private void Start()
    {
        oilSplat = gameHandler.gameAudioData.AddNewAudioSourceFromStandard("Player", gameObject, "Splat");
        SetHat();
    }
    private void FixedUpdate()
    {
        if (gameHandler.gameEnded) return;
        if (!gameHandler.timerOn)
        {
            if (gameHandler.uiHandler.hintBox.activeInHierarchy)
            {
                if (inputHandler.PressAnyKey())
                {
                    gameHandler.uiHandler.hintBox.SetActive(false);
                    gameHandler.uiHandler.isPaused = false;
                    gameHandler.timerOn = true;
                }
            }
            return;
        }
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
        float currentSpeed = moveSpeed;
        if (isSlowed)
        {
            oilOverlay.SetActive(true);
        }
        if (isSlowed)
        {
            if (fadeInkTimer <= 1f)
            {
                fadeInkTimer += 4 * Time.deltaTime;
            }
            oilOverlay.GetComponent<MeshRenderer>().material.SetFloat("_Mask_Height", Mathf.Lerp(0f, 0.5f, fadeInkTimer));
            currentSpeed = moveSpeed * 0.5f;
        }
        else
        {
            if (fadeInkTimer >= 0f)
            {
                fadeInkTimer -= 2 * Time.deltaTime;
            }
            oilOverlay.GetComponent<MeshRenderer>().material.SetFloat("_Mask_Height", Mathf.Lerp(0f, 0.5f, fadeInkTimer));
        }
        Vector3 force = moveInput * currentSpeed * 10;
        rb.AddForce(force, ForceMode.Force);
    }

    public override void PlayerInput()
    {
        Vector2 playerMovement = inputHandler.GetPlayerMovement();
        if (playerMovement.y > gameHandler.stageTop - transform.position.y) 
        {
            playerMovement.y = 0;
        }
        if (transform.position.z - gameHandler.reefStage.transform.position.z <= -18 && playerMovement.x < 0)
        {
            playerMovement.x = 0;
        }
        if (transform.position.z - gameHandler.reefStage.transform.position.z >= 8 && playerMovement.x > 0)
        {
            playerMovement.x = 0;
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

    public override void SlowTimer()
    {
        isSlowed = false;
    }

    public override void SlowEffect()
    {
        if (!isSlowed)
        {
            oilSplat.Play();
            Invoke(nameof(SlowTimer), slowTimer);
        }
        isSlowed = true;
    }
    public override void Respawn()
    {
    }
}
