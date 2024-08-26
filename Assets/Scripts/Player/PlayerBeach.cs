using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
            Quaternion rotation2 = Quaternion.Euler(new Vector3(0, 0, 0));
            StartCoroutine(rotateObject(gameObject, rotation2, rotationTime));
            isMoving = true;
            moveDir += Vector3.forward * (moveDistance+1);
        }

        if (inputHandler.BeachMoveLeft() && !isMoving)
        {
            if (moveDir.x - moveDistance < horizontalSpaces * moveDistance * -1) return;
            if (moveDir.x - moveDistance < -12.0f) return;
            
            Quaternion rotation2 = Quaternion.Euler(new Vector3(0, -90, 0));
            if(gameObject.transform.rotation.y > 0){
                StartCoroutine(rotateObject(gameObject, rotation2, 2 * rotationTime));
            }
            else if(gameObject.transform.rotation.y <= 0){
                StartCoroutine(rotateObject(gameObject, rotation2, rotationTime));
            }
            
            isMoving = true;
            moveDir += Vector3.left * moveDistance;
        }
        if (inputHandler.BeachMoveRight() && !isMoving)
        {
            if (moveDir.x + moveDistance > horizontalSpaces * moveDistance) return;
            if (moveDir.x - moveDistance > 12.0f) return;
            
            Quaternion rotation2 = Quaternion.Euler(new Vector3(0, 90, 0));
            if(gameObject.transform.rotation.y < 0){
                StartCoroutine(rotateObject(gameObject, rotation2, 2 * rotationTime));
            }
            else if(gameObject.transform.rotation.y >= 0){
                StartCoroutine(rotateObject(gameObject, rotation2, rotationTime));
            }
            isMoving = true;
            moveDir += Vector3.right * moveDistance;
        }
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
}
