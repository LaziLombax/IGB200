using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseNav : MonoBehaviour
{
    public enum ChaseState
    {
        patrolling,
        reach,
        chase,
        start
    }
    [Space(10)]
    [Header("Variables")]
    public ChaseState myState;
    public Transform target;
    public Transform patrolLeft;
    public Transform patrolRight;
    public bool reachRightEnd;
    public float rotateSmooth = 3f;
    private Vector3 newRotation;
    public float patrollingSpeed = 4f;
    public float chaseSpeed = 6f;
    public GameObject indicator;


    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myState = ChaseState.start;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameHandler.Instance.timerOn) return;
        if (myState == ChaseState.chase && target != null)
        {
            indicator.SetActive(true);
            agent.speed = chaseSpeed;
            Vector3 targetXZ = target.position;
            targetXZ.y = 0;
            agent.SetDestination(targetXZ);
        }
        else if (myState == ChaseState.patrolling)
        {
            indicator.SetActive(false);
            agent.speed = patrollingSpeed;
            if (reachRightEnd)
            {
                agent.SetDestination(patrolLeft.position);
                if (Vector3.Distance(transform.position, patrolLeft.position) < 0.1f)
                {
                    newRotation = transform.eulerAngles - 180 * Vector3.up;
                    myState = ChaseState.reach;
                    Invoke(nameof(StartPatrolling), 1f);
                    patrolLeft.GetComponent<PatrolChange>().RandomPatrol();
                }
            }
            else
            {
                agent.SetDestination(patrolRight.position);
                if (Vector3.Distance(transform.position, patrolRight.position) < 0.1f)
                {
                    newRotation = transform.eulerAngles + 180 * Vector3.up;
                    myState = ChaseState.reach;
                    Invoke(nameof(StartPatrolling), 2f);
                    patrolRight.GetComponent<PatrolChange>().RandomPatrol();
                }
            }
        }
        else if (myState == ChaseState.reach)
        {
            indicator.SetActive(false);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, newRotation, Time.deltaTime * rotateSmooth);
        }
        else if (myState == ChaseState.start)
        {
            indicator.SetActive(false);
            Invoke(nameof(StartPatrolling), Random.Range(0f,4f));
        }
    }

    public void StartPatrolling()
    {
        if (target != null) return;
        reachRightEnd = !reachRightEnd;
        myState = ChaseState.patrolling;
    }
}
