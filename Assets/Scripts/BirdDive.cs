using System.Collections;
using UnityEngine;

public class BirdDive : MonoBehaviour
{
    public Vector3 DivePosition; // Made public to set in Inspector
    public Vector3 startPosition;
    public Vector3 EndPosition;
    private bool movingToDivePosition = true;
    public float speed = 2f; // Movement speed

    void Start()
    {
        startPosition = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        DivePosition = new Vector3((3 * Random.Range(-4, 4)), 0.2f, gameObject.transform.position.z);
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        float journeyLength = Vector3.Distance(startPosition, DivePosition);
        float startTime = Time.time;
        EndPosition = new Vector3(startPosition.x * -1, startPosition.y, startPosition.z);
        while (true)
        {
            float distanceCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distanceCovered / journeyLength;

            if (movingToDivePosition)
            {
                transform.position = Vector3.Lerp(startPosition, DivePosition, fractionOfJourney);
                Vector3 targetDirection = DivePosition - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection,(2.0f*Time.deltaTime),0);
                transform.rotation = Quaternion.LookRotation(newDirection);
                if (fractionOfJourney >= 1.0f)
                {
                    movingToDivePosition = false;
                    startTime = Time.time; // Reset start time for returning journey
                    journeyLength = Vector3.Distance(DivePosition, startPosition);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(DivePosition, EndPosition, fractionOfJourney);
                Vector3 targetDirection = DivePosition - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection,(1.0f*Time.deltaTime),0);
                transform.rotation = Quaternion.LookRotation(newDirection);
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
            Gizmos.DrawLine(DivePosition, EndPosition);
        }
    }
}
