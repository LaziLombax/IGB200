using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBeach : PlayerController
{
    [Header("Beach Movement Variables")]
    public float moveTime = 0.1f;
    public float moveDistance;
    public float horizontalSpaces;
    public float rotationTime;
    public AudioSource moveAudio;
    public bool canBuffer;
    public float bufferTime;
    private bool isMoving, rotating;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Queue<(Vector3, Quaternion)> inputQueue = new Queue<(Vector3, Quaternion)>(); // Queue to store input

    private void Start()
    {
        initialPosition = transform.position; // Store the player's starting position

        moveAudio = gameHandler.gameAudioData.AddNewAudioSourceFromStandard("Player", gameObject, "Beach Move");
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
        if (isMoving)
        {
            MovePlayer();
        }
        else if (inputQueue.Count > 0)
        {
            // Process next input from the queue once movement is finished
            var (nextPosition, nextRotation) = inputQueue.Dequeue();
            SetMove(nextPosition, nextRotation);
        }
    }

    public override void MovePlayer()
    {
        float t = Mathf.SmoothStep(0f, 1f, moveTime * moveSpeed * Time.fixedDeltaTime); // Easing the movement
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, t);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            rb.MovePosition(targetPosition); // Snap to the target position
            isMoving = false; // Stop moving once the target is reached
            canBuffer = false;
            initialPosition = targetPosition;
        }

        // Rotate player smoothly
        StartCoroutine(rotateObject(gameObject, targetRotation, rotationTime));
    }

    public override void PlayerInput()
    {
        if (isMoving && inputQueue.Count > 1) return; // Avoid overfilling the queue

        Vector3 currentPos = targetPosition;
        //initialPosition = targetPosition;
        if (inputHandler.BeachMoveForward())
        {
            EnqueueMove(currentPos + Vector3.forward * moveDistance, Quaternion.Euler(0, 0, 0));
        }
        if (inputHandler.BeachMoveLeft())
        {
            if (currentPos.x - horizontalSpaces < -9) return;
            EnqueueMove(currentPos + Vector3.left * horizontalSpaces, Quaternion.Euler(0, -90, 0));
        } 
        if (inputHandler.BeachMoveRight())
        {
            if (currentPos.x + horizontalSpaces > 9) return;
            EnqueueMove(currentPos + Vector3.right * horizontalSpaces, Quaternion.Euler(0, 90, 0));
        } 
        if (inputHandler.BeachMoveDown())
        {
            if (currentPos.z - moveDistance < 0) return;
            EnqueueMove(currentPos + Vector3.back * moveDistance, Quaternion.Euler(0, 180, 0));
        }
    }

    private void SetMove(Vector3 newPosition, Quaternion newRotation)
    {
        targetPosition = newPosition;
        targetRotation = newRotation;
        moveAudio.Play();
        isMoving = true; // Player starts moving
    }

    private void EnqueueMove(Vector3 newPosition, Quaternion newRotation)
    {
        inputQueue.Enqueue((newPosition, newRotation)); // Store input in the queue
    }

    // Collision detection for rocks
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rock")) // Assuming rocks have the tag "Rock"
        {
            ResetPlayerPosition(); // Reset to initial position when colliding with rocks
        }
    }
    public void ResetPlayerPosition()
    {
        transform.position = initialPosition; // Reset to original position
        targetPosition = initialPosition; // Reset the target position as well
        isMoving = false; // Stop the player from moving
        inputQueue.Clear(); // Clear the input queue
    }

    IEnumerator rotateObject(GameObject obj, Quaternion newRot, float duration)
    {
        if (rotating) yield break;

        rotating = true;
        Quaternion startRot = obj.transform.rotation;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            obj.transform.rotation = Quaternion.Lerp(startRot, newRot, timeElapsed / duration);
            yield return null;
        }

        rotating = false;
    }

    public override void SlowEffect() { /* Not Implemented */ }
    public override void SlowTimer() { /* Not Implemented */ }

    public override void Respawn()
    {
        transform.position = spawnPoint;
        targetPosition = spawnPoint;
    }
}