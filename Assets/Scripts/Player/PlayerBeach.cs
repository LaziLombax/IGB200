using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBeach : PlayerController
{
    [Space(10)]
    [Header("Beach Movement Variables")]
    public bool isMoving;
    public Vector3 moveDir;
    public float moveTime = 0.1f;
    public float moveDistance;
    public float horizontalSpaces;
    public float rotationTime;
    bool rotating = false;
    public bool hitBarrier;
    public Vector3 lastPosition;
    public AudioSource moveAudio;
    public List<Vector3> inputOrder = new List<Vector3>();
    public List<Vector3> rotationOrder = new List<Vector3>();

    [Header("Mobile Movement Variables")]
    public float swipeThreshold = 50f;
    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private Vector2 endTouchPosition;



    private void Start()
    {
        if (inputHandler != null)
            inputHandler.playerControls.Player.Swipe.performed += OnSwipePerformed;
        lastPosition = transform.position;
        moveAudio = gameHandler.gameAudioData.AddNewAudioSourceFromStandard("Player", gameObject, "Beach Move");
        moveDir = transform.position;
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
            if (gotHit)
            {
                inputOrder.RemoveAt(0);
                rotationOrder.RemoveAt(0);
                isMoving = false;
                return;
            }
            MovePlayerBeach(inputOrder[0]);
        }   
    }
    public void MovePlayerBeach(Vector3 nextPos)
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, nextPos, moveTime * moveSpeed * Time.fixedDeltaTime);
        Quaternion newRotation = Quaternion.Euler(rotationOrder[0]);
        rb.MovePosition(newPosition);
        if (gameObject.transform.rotation.y < 0)
        {
            StartCoroutine(rotateObject(gameObject, newRotation, 2 * rotationTime));
        }
        else if (gameObject.transform.rotation.y >= 0)
        {
            StartCoroutine(rotateObject(gameObject, newRotation, rotationTime));
        }

        if (Vector3.Distance(transform.position, newPosition) <= 0.01f)
        {
            lastPosition = nextPos;
            if (hitBarrier) hitBarrier = false;
            rb.MovePosition(nextPos);
            inputOrder.RemoveAt(0);
            rotationOrder.RemoveAt(0);
            isMoving = false;
        }
    }

    public override void PlayerInput()
    {
        if (inputOrder.Count > 2) return;
        Vector3 newPos = new Vector3();
        if (inputOrder.Count == 0)
        {
            newPos = transform.position;
        }
        else
        {
            isMoving = true;
            newPos = inputOrder.Last();
        }
        if (inputHandler.BeachMoveForward())
        {
            MoveUp(newPos);
        }

        if (inputHandler.BeachMoveLeft())
        {
            MoveLeft(newPos);
        }
        if (inputHandler.BeachMoveRight())
        {
            MoveRight(newPos);
        }

        if (inputHandler.BeachMoveDown())
        {
            MoveDown(newPos);
        }
    }

    public void MoveUp(Vector3 newPos)
    {
        newPos += Vector3.forward * (moveDistance + 1);
        inputOrder.Add(newPos);
        rotationOrder.Add(new Vector3(0, 0, 0));
        moveAudio.Play();

    }
    public void MoveDown(Vector3 newPos)
    {
        if (transform.position.z - 4f < 0f) return;
        newPos += Vector3.back * (moveDistance + 1);
        rotationOrder.Add(new Vector3(0, 180, 0));
        inputOrder.Add(newPos);
        moveAudio.Play();
    }
    public void MoveLeft(Vector3 newPos)
    {
        if (newPos.x - moveDistance < horizontalSpaces * moveDistance * -1) return;
        if (newPos.x - moveDistance < -12.0f) return;

        newPos += Vector3.left * moveDistance;
        rotationOrder.Add(new Vector3(0, -90, 0));
        inputOrder.Add(newPos);
        moveAudio.Play();
    }
    public void MoveRight(Vector3 newPos)
    {
        if (newPos.x + moveDistance > horizontalSpaces * moveDistance) return;
        if (newPos.x - moveDistance > 12.0f) return;

        newPos += Vector3.right * moveDistance;
        rotationOrder.Add(new Vector3(0, 90, 0));
        inputOrder.Add(newPos);
        moveAudio.Play();
    }


    IEnumerator rotateObject(GameObject gameObjectToMove, Quaternion newRot, float duration){
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Quaternion currentRot = gameObjectToMove.transform.rotation;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            gameObjectToMove.transform.rotation = Quaternion.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }

    public override void SlowEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void SlowTimer()
    {
        throw new System.NotImplementedException();
    }

    public override void MovePlayer()
    {
        throw new System.NotImplementedException();
    }


    private void OnSwipePerformed(InputAction.CallbackContext context)
    {
        // Detect touch start position
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            startTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        // When the finger is lifted
        if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
        {
            endTouchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            currentTouchPosition = endTouchPosition - startTouchPosition;

            Debug.Log(endTouchPosition.ToString());
            Debug.Log(currentTouchPosition.ToString());
            Debug.Log("Swiped");
            // Call the method to process the swipe
            ProcessSwipe(currentTouchPosition);
        }
    }

    private void ProcessSwipe(Vector2 swipeDelta)
    {
        if (inputOrder.Count > 2) return;
        Vector3 newPos = new Vector3();
        if (inputOrder.Count == 0)
        {
            newPos = transform.position;
        }
        else
        {
            isMoving = true;
            newPos = inputOrder.Last();
        }
        if (swipeDelta.magnitude >= swipeThreshold) // Only process if the swipe is large enough
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            // Determine swipe direction based on the larger axis movement
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    MoveRight(newPos);
                }
                else
                {
                    MoveLeft(newPos);
                }
            }
            else
            {
                if (y > 0)
                {
                    MoveUp(newPos);
                }
                else
                {
                    MoveDown(newPos);
                }
            }
        }
    }
}
