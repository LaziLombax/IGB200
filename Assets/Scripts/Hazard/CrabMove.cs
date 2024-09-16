using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMove : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of the crab's movement
    public float movementRange = 5.0f; // The total range of movement

    private Vector3 startPosition;
    private float movementTimer;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
        movementTimer += Random.Range(0f,5f);
    }

    void Update()
    {
        movementTimer += Time.deltaTime * moveSpeed; // Increment timer based on speed

        // Calculate the new position using a sine wave function
        float offset = Mathf.Sin(movementTimer) * movementRange;

        // Set the new position while keeping the original y and z coordinates
        transform.position = new Vector3(startPosition.x + offset, transform.position.y, transform.position.z);
    }
}
