using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNav : MonoBehaviour
{
    public enum ChaseState
    {
        Patrolling,
        Reach,
        Chase,
        Start
    }

    [Header("Variables")]
    public ChaseState myState;
    public Transform target;
    public Transform patrolLeft;
    public Transform patrolRight;
    public bool reachRightEnd;
    public float rotateSmooth = 3f;
    public float patrollingSpeed = 4f;
    public float chaseSpeed = 6f;
    public GameObject indicator;
    public Animator animator;

    private NavMeshAgent agent;
    private Vector3 newRotation;
    private PatrolChange patrolLeftScript;
    private PatrolChange patrolRightScript;
    private AudioSource chaseAudio;
    private Transform playerTransform;
    private bool audioPlayed = false;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myState = ChaseState.Start;

        // Cache player transform
        playerTransform = GameHandler.Instance.playerPos;

        // Cache PatrolChange components to avoid repeated GetComponent calls
        patrolLeftScript = patrolLeft.GetComponent<PatrolChange>();
        patrolRightScript = patrolRight.GetComponent<PatrolChange>();

        // Initialize audio
        chaseAudio = GameHandler.Instance.gameAudioData.AddNewAudioSourceFromGroup("Hazard", "Pig", gameObject, "Chase");
    }

    void Update()
    {
        if (!GameHandler.Instance.timerOn || !IsPlayerWithinRange()) return;
        if(myState == ChaseState.Start || myState == ChaseState.Reach)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            animator.SetBool("IsWalking", true);
        }
        switch (myState)
        {
            case ChaseState.Chase:
                if (!audioPlayed)
                {
                    chaseAudio.Play();
                    audioPlayed = true;
                }
                HandleChaseState();
                break;
            case ChaseState.Patrolling:
                HandlePatrollingState();
                break;
            case ChaseState.Reach:
                HandleReachState();
                break;
            case ChaseState.Start:
                HandleStartState();
                break;
        }
    }

    bool IsPlayerWithinRange()
    {
        Vector3 middle = transform.position;
        middle.x = 0f;
        float distanceToPlayer = Vector3.Distance(playerTransform.position, middle);

        return distanceToPlayer <= 28f;
    }

    private void HandleChaseState()
    {
        if (target == null) return;

        indicator.SetActive(true);
        agent.speed = chaseSpeed;

        // Avoid creating new Vector3 every frame
        Vector3 targetXZ = new Vector3(target.position.x, 0, target.position.z);
        agent.SetDestination(targetXZ);
    }

    private void HandlePatrollingState()
    {
        indicator.SetActive(false);
        agent.speed = patrollingSpeed;

        Transform currentPatrolTarget = reachRightEnd ? patrolLeft : patrolRight;
        if (agent.destination != currentPatrolTarget.position)
        {
            agent.SetDestination(currentPatrolTarget.position);
        }

        if (Vector3.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            newRotation = transform.eulerAngles + (reachRightEnd ? -180 : 180) * Vector3.up;
            myState = ChaseState.Reach;

            if (reachRightEnd)
            {
                StartCoroutine(InvokePatrolRoutine(1f));
                patrolLeftScript.RandomPatrol();
            }
            else
            {
                StartCoroutine(InvokePatrolRoutine(2f));
                patrolRightScript.RandomPatrol();
            }

            reachRightEnd = !reachRightEnd; // Switch ends
            agent.SetDestination(currentPatrolTarget.position);
        }
    }

    private void HandleReachState()
    {
        indicator.SetActive(false);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, newRotation, Time.deltaTime * rotateSmooth);
    }

    private void HandleStartState()
    {
        indicator.SetActive(false);
        StartCoroutine(InvokePatrolRoutine(Random.Range(0f, 4f)));
    }

    private IEnumerator InvokePatrolRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (target == null)
        {
            myState = ChaseState.Patrolling;
            audioPlayed = false; // Reset audio playback state
        }
    }
}
