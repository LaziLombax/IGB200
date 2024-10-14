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
    public bool isMoving, rotating;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public Animator animator;

    private Queue<(Vector3, Quaternion)> inputQueue = new Queue<(Vector3, Quaternion)>(); // Queue to store input

    private void Start()
    {
        currentPos = transform.position;
        plannedPos = currentPos;
        initialPosition = transform.position; // Store the player's starting position
        animator.speed = 1f;
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
        if (Vector3.Distance(transform.position, targetPosition) <= 0.6f)
            canBuffer = true;
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, t);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            rb.MovePosition(targetPosition); // Snap to the target position
            isMoving = false; // Stop moving once the target is reached
            canBuffer = false;
            currentPos = targetPosition;
            initialPosition = targetPosition;
            //plannedPos = currentPos;
        }

        // Rotate player smoothly
        StartCoroutine(rotateObject(gameObject, targetRotation, rotationTime));
    }
    private Vector3 currentPos;
    public Vector3 plannedPos;
    public override void PlayerInput()
    {
        if (inputQueue.Count >= 1 && isMoving) return; // Avoid overfilling the queue
        if (plannedPos != currentPos)
        {
            if (!canBuffer) return;
            currentPos = plannedPos;
        }
        if (inputHandler.BeachMoveForward())
        {
            EnqueueMove(plannedPos + Vector3.forward * moveDistance, Quaternion.Euler(0, 0, 0));
            plannedPos = currentPos + Vector3.forward * moveDistance;
        }
        if (inputHandler.BeachMoveLeft())
        {
            if (currentPos.x - horizontalSpaces < -9) return;
            EnqueueMove(plannedPos + Vector3.left * horizontalSpaces, Quaternion.Euler(0, -90, 0));
            plannedPos = currentPos + Vector3.left * horizontalSpaces;
        }
        if (inputHandler.BeachMoveRight())
        {
            if (currentPos.x + horizontalSpaces > 9) return;
            EnqueueMove(plannedPos + Vector3.right * horizontalSpaces, Quaternion.Euler(0, 90, 0));
            plannedPos = currentPos + Vector3.right * horizontalSpaces;
        }
        if (inputHandler.BeachMoveDown())
        {
            if (currentPos.z - moveDistance < 0) return;
            EnqueueMove(plannedPos + Vector3.back * moveDistance, Quaternion.Euler(0, 180, 0));
            plannedPos = currentPos + Vector3.back * moveDistance;
        }
        if(plannedPos == transform.position && !isMoving)
            plannedPos = transform.position;
        if(inputQueue.Count == 2)
            Debug.Log(inputQueue.Count.ToString());
    }
    private void OnDrawGizmosSelected()
    {

        //Gizmos.DrawSphere(currentPos, 1);
       // Gizmos.DrawSphere(targetPosition, 1);
       // Gizmos.DrawSphere(initialPosition, 1);
    }
    private void SetMove(Vector3 newPosition, Quaternion newRotation)
    {
        //plannedPos = Vector3.zero;
        initialPosition = transform.position;
        animator.SetTrigger("Jump");
        targetPosition = newPosition;
        targetRotation = newRotation;
        moveAudio.Play();
        isMoving = true; // Player starts moving
    }

    private void EnqueueMove(Vector3 newPosition, Quaternion newRotation)
    {
        inputQueue.Enqueue((newPosition, newRotation)); // Store input in the queue
    }

    public void ResetPlayerPosition()
    {
        plannedPos = initialPosition; // Reset to original position
        targetPosition = initialPosition; // Reset the target position as well
         // Stop the player from moving
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
        currentPos = spawnPoint;
        plannedPos = spawnPoint;
        isMoving = false;
        inputQueue.Clear(); // Clear the input queue
    }
}