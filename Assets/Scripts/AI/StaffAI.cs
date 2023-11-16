using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class StaffAI : MonoBehaviour
{
    [Header("AI Settings")]
    public GameObject player; // Direct reference to the player
    public float patrolRadius = 10f;
    public float sightRange = 10f;
    public float viewAngle = 110f;
    public float chaseFatigueRate = 5f;
    public float restRecoveryRate = 10f;
    public float maxFatigue = 100f;
    public float captureDistance = 2f;
    public LayerMask propLayer;
    public float turnSpeed = 5f; // Speed at which the AI turns
    public float investigationDuration = 3f; // Duration of investigation

    [Header("References")]
    private NavMeshAgent agent;
    private Animator animator;
    private float fatigue = 0f;
    private bool isResting = false;
    private bool isChasing = false;
    private Vector3 randomPatrolPoint;
    private Vector3 lastKnownPlayerPosition;
    private bool isInvestigating = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetRandomPatrolPoint();
        StartCoroutine(DetectionCoroutine());
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isInvestigating)
        {
            Investigate();
        }
        else
        {
            Patrol();
        }

        UpdateFatigue();
    }

    IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            DetectThreat();
            yield return new WaitForSeconds(0.2f); // Detection interval
        }
    }

    private void SetRandomPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
        randomPatrolPoint = hit.position;
        agent.SetDestination(randomPatrolPoint);
        animator.SetBool("Move", true);
    }

    private void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isChasing && !isInvestigating)
        {
            SetRandomPatrolPoint();
        }
    }

    private void DetectThreat()
    {
        if (isResting) return;

        bool playerInSight = IsPlayerInFieldOfView();

        if (playerInSight)
        {
            StartChase();
        }
        else if (!playerInSight && isChasing)
        {
            StopChase();
            StartInvestigation();
        }
    }

    private bool IsPlayerInFieldOfView()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleBetweenAIAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleBetweenAIAndPlayer < viewAngle / 2 && Vector3.Distance(transform.position, player.transform.position) < sightRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, propLayer))
            {
                if (hit.collider.gameObject == player)
                {
                    lastKnownPlayerPosition = player.transform.position;
                    return true;
                }
            }
        }
        return false;
    }

    private void StartChase()
    {
        isChasing = true;
        isResting = false;
        isInvestigating = false;
        animator.SetTrigger("Chase");
    }

    private void ChasePlayer()
    {
        if (player)
        {
            agent.SetDestination(player.transform.position);
            if (Vector3.Distance(transform.position, player.transform.position) < captureDistance)
            {
                CapturePlayer();
            }
        }
    }

    private void StopChase()
    {
        isChasing = false;
        animator.ResetTrigger("Chase");
    }

    private void StartInvestigation()
    {
        isInvestigating = true;
        agent.SetDestination(lastKnownPlayerPosition);
        animator.SetTrigger("Investigate");
    }

    private void Investigate()
    {
        if (Vector3.Distance(transform.position, lastKnownPlayerPosition) < 1f)
        {
            if (!isChasing)
            {
                StartCoroutine(PerformInvestigation());
            }
        }
    }

    IEnumerator PerformInvestigation()
    {
        isInvestigating = false;
        animator.SetTrigger("Investigate");
        yield return new WaitForSeconds(investigationDuration);
        animator.ResetTrigger("Investigate");
        SetRandomPatrolPoint();
    }

    private void CapturePlayer()
    {
        GameManager.Instance.GameOver();
        animator.SetTrigger("Dance");
        player.GetComponent<Animator>().SetTrigger("Sob");
    }

    private void UpdateFatigue()
    {
        if (isChasing)
        {
            fatigue += Time.deltaTime * chaseFatigueRate;
            if (fatigue >= maxFatigue)
            {
                StartResting();
            }
        }
        else if (isResting)
        {
            fatigue -= Time.deltaTime * restRecoveryRate;
            if (fatigue <= 0)
            {
                StopResting();
            }
        }
    }

    private void StartResting()
    {
        isResting = true;
        isChasing = false;
        isInvestigating = false;
        animator.SetTrigger("Rest");
    }

    private void StopResting()
    {
        isResting = false;
        fatigue = 0;
        animator.ResetTrigger("Rest");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
