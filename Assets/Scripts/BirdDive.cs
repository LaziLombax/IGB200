using System.Collections;
using UnityEngine;

public class BirdDive : MonoBehaviour
{
    public Vector3 DivePosition; // Made public to set in Inspector
    private Vector3 startPosition;
    private bool movingToDivePosition = true;
    public float speed = 2f; // Movement speed

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        DivePosition = new Vector3((3 * Random.Range(-4, 4)), 0.5f, (player.transform.position.z + (4 * Random.Range(0, 8))));
    }

    void Start()
    {
        startPosition = transform.position; // Set initial position
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float journeyLength = Vector3.Distance(startPosition, DivePosition);
        float startTime = Time.time;

        while (true)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;

            if (movingToDivePosition)
            {
                transform.position = Vector3.Lerp(startPosition, DivePosition, fractionOfJourney);
                if (fractionOfJourney >= 1.0f)
                {
                    movingToDivePosition = false;
                    startTime = Time.time; // Reset start time for returning journey
                    journeyLength = Vector3.Distance(DivePosition, startPosition);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(DivePosition, startPosition, fractionOfJourney);
                if (fractionOfJourney >= 1.0f)
                {
                    // The object has returned to its original position
                    Destroy(gameObject);
                    yield break; // Exit the coroutine
                }
            }
            yield return null; // Wait for the next frame
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Draw the path
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPosition, DivePosition);
            Gizmos.DrawLine(DivePosition, startPosition);
        }
    }
}
