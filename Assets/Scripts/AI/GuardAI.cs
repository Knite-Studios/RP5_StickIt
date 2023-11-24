/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{

    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float hearingRange = 15f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float viewAngle = 110f;

    [SerializeField] private float restRecovery = 10f;
    [SerializeField] private float maxFatigue = 16f;
    [SerializeField] private float captureDistance = 2f;
    [SerializeField] private float detectionInterval = 0.2f;

    private State currentState;
    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private float fatigue = 0f;
    private bool isResting = false;
    private bool isChasing = false;
    private bool isInvestigating = false;
    private Vector3 lastHeardSoundPosition;
    private float lastDetectionTime = 0f;

    enum State
    {
        Patrol,
        Chase,
        Invertigate,
        Fatigue
    }

    private void SetState(State newState)
    {
        currentState = newState;
        StopAllCoroutines();

        switch (currentState)
        {
            case State.Patrol:
                StartCoroutine(OnPatrol());
                break;
            case State.Chase:
                StartCoroutine(OnChase());
                break;
            case State.Invertigate:
                StartCoroutine(OnInvestigate());
                break;
            case State.Fatigue:
                StartCoroutine(OnFatigue());
                break;


        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetState(State.Fatigue);

    }
    #region State Methods
    private IEnumerable OnFatigue()
    {
        while (currentState == State.Fatigue)
        {
            if (fatigue >= maxFatigue)
            {
                isResting = true;
                fatigue = 0;
                animator.SetBool("isResting", isResting);
                yield return new WaitForSeconds(restRecovery);
                isResting = false;
                animator.SetBool("isResting", isResting);
            }
            else
            {
                SetState(State.Patrol);
            }

            yield return null;
        }
    }
    private IEnumerable OnPatrol()
    {
        while (currentState == State.Patrol)
        {
            if (!IsPlayerInFOV())
            {
                if (patrolPoints.Length == 0)
                {
                    Debug.LogWarning("No patrol points assigned to " + gameObject.name);
                    yield return null;
                }


                agent.destination = patrolPoints[currentPatrolIndex].position;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
            else
            {
                SetState(State.Chase);
            }
            yield return null;
        }
    }
    private IEnumerable OnChase()
    {
        while (currentState == State.Chase)
        {
            yield return null;
        }
    }
    private IEnumerable OnInvestigate()
    {
        while (currentState == State.Invertigate)
        {

            yield return null;
        }
    }



    #endregion
    #region Helper Methods
    private bool IsPlayerInFOV()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Collider[] Colliders = Physics.OverlapSphere(this.transform.position, sightRange);

        if (player != null)
        {
            foreach (Collider collider in Colliders)
            {
                if (collider.gameObject == player)
                {
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                    float angleBetweenAIAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

                    if (angleBetweenAIAndPlayer < viewAngle / 2 && Vector3.Distance(transform.position, player.transform.position) < sightRange)// Check for line of sight
                    {
              
                                return true;
                       
                    }
                }
            }
        }
            return false; 
    }






    #endregion

    // Update is called once per frame
    void Update()
    {

    }
}
*/