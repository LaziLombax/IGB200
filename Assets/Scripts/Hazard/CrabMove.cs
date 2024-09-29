using System.Collections;
using UnityEngine;

public class CrabMove : MonoBehaviour
{
    public float moveSpeed = 2.0f; // Speed of the crab's movement
    public float movementRange = 5.0f; // The total range of movement
    public bool isUnderWater = false;

    private Vector3 startPosition;
    private float movementTimer;

    void Start()
    {
        startPosition = transform.position; // Store the initial position
        movementTimer = Random.Range(0f, 5f); // Direct assignment, no need for increment

        StartCoroutine(UpdateMovement());
    }

    private IEnumerator UpdateMovement()
    {
        while (true)
        {
            if (GameHandler.Instance.timerOn)
            {
                movementTimer += Time.deltaTime * moveSpeed; // Increment timer based on speed

                // Calculate the offset based on sine wave function
                float offset = Mathf.Sin(movementTimer) * movementRange;

                // Cache the current position to minimize repeated access
                Vector3 currentPosition = transform.position;

                if (isUnderWater)
                {
                    currentPosition.z = startPosition.z + offset; // Update z if under water
                }
                else
                {
                    currentPosition.x = startPosition.x + offset; // Update x if not under water
                }

                transform.position = currentPosition; // Set the new position
            }

            yield return new WaitForSeconds(0.1f); // Update every 0.1 seconds (10 times a second)
        }
    }
}