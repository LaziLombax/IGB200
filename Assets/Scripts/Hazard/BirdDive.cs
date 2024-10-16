using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdDive : MonoBehaviour
{
    public Vector3 DivePosition; // Made public to set in Inspector
    public Vector3 startPosition;
    public Vector3 EndPosition;
    private bool movingToDivePosition = true;
    public float speed = 2f; // Movement speed
    public GameObject myIndicator;
    public GameObject savedTarget;

    void Start()
    {
        startPosition = transform.position;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        CheckDive();
        savedTarget = Instantiate(myIndicator, DivePosition, Quaternion.identity);
        StartCoroutine(MoveRoutine());
    }

    public void CheckDive(){
        Vector3[] AvailablePositions = new Vector3[7]; 
        List<Vector3> SetPositions = new List<Vector3>();
        SetPositions.Clear();
        for (int i = 0; i < 7; i++){
            AvailablePositions[i] = new Vector3(-9.0f+(i*3), 0.3f, gameObject.transform.position.z);
            if(isObjectHere(AvailablePositions[i]) == false){
                SetPositions.Add(AvailablePositions[i]);
            }
        }
        Debug.Log(SetPositions[0]);
        DivePosition = SetPositions[Random.Range(0,SetPositions.Count)];
    }

    bool isObjectHere(Vector3 position){
        Collider[] intersecting = Physics.OverlapSphere(position, 0.01f);
        foreach (var hitcolliders in intersecting){
            if (hitcolliders.gameObject.GetComponent<Barrier>() != null){
                Debug.Log(hitcolliders.gameObject.name);
                return true;
            }
        }
        return false;
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

            if (movingToDivePosition && GameHandler.Instance.timerOn)
            {
                transform.position = Vector3.Lerp(startPosition, DivePosition, fractionOfJourney);
                Vector3 targetDirection = DivePosition - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, (2.0f * Time.deltaTime), 0);
                transform.rotation = Quaternion.LookRotation(newDirection);
                if (fractionOfJourney >= 1.0f)
                {
                    movingToDivePosition = false;
                    Destroy(savedTarget);
                    startTime = Time.time; // Reset start time for returning journey
                    journeyLength = Vector3.Distance(DivePosition, startPosition);
                }
            }
            else if (GameHandler.Instance.timerOn)
            {
                transform.position = Vector3.Lerp(DivePosition, EndPosition, fractionOfJourney);
                Vector3 targetDirection = DivePosition - transform.position;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, (-4.0f * Time.deltaTime), 0);
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
