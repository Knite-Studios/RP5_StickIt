using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StaffAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float sightRange = 10f;
    public LayerMask playerLayer;
    public float viewAngle = 110f;
    public float chaseFatigueRate = 5f;
    public float restRecoveryRate = 10f;
    public float maxFatigue = 100f;
    public float captureDistance = 2f;
    public float detectionInterval = 0.2f;

    private NavMeshAgent agent;
    private Animator animator;
    private int currentPatrolIndex;
    private float fatigue = 0f;
    private bool isResting = false;
    private bool isChasing = false;
    private bool isInvestigating = false;
    private Vector3 lastHeardSoundPosition;
    private float lastDetectionTime = 0f;
    public float hearingRange = 15f;
    public float scarecrowViewRange = 10f;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (Time.time - lastDetectionTime > detectionInterval)
        {
            DetectThreat();
            lastDetectionTime = Time.time;
        }

        if (isChasing)
        {
            ChasePlayer();
            animator.SetBool("Move", false);

        }

        if (isInvestigating)
        {
            animator.SetBool("Move", false);

            InvestigateSound();
        }

        UpdateFatigue();

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !isChasing && !isInvestigating)
        {
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;
        animator.SetBool("Move", true);
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void DetectThreat()
    {
        if (isResting) return;

        bool playerInSight = IsPlayerInFieldOfView();
        bool heardPlayer = HeardPlayerSound();
        bool scarecrowInView = IsScarecrowInView();

        if (playerInSight || heardPlayer)
        {
            if (scarecrowInView)
            {
                // If the scarecrow is in view, the AI should ignore the player
                StopChase();
            }
            else
            {
                StartChase();
            }
        }
        else if (!playerInSight && !heardPlayer && isChasing)
        {
            StopChase();
        }
    }
    private bool IsPlayerInFieldOfView()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleBetweenAIAndPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleBetweenAIAndPlayer < viewAngle / 2 && Vector3.Distance(transform.position, player.transform.position) < sightRange)
        {
            // Check for line of sight
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool HeardPlayerSound()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AudioSource playerAudio = player.GetComponent<AudioSource>();

        if (playerAudio != null && playerAudio.isPlaying)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= hearingRange)
            {
                // Optional: Check for line of sight
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, hearingRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private bool IsScarecrowInView()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, scarecrowViewRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Scarecrow"))
            {
                Vector3 directionToScarecrow = (hitCollider.transform.position - transform.position).normalized;
                float angleToScarecrow = Vector3.Angle(transform.forward, directionToScarecrow);

                if (angleToScarecrow < viewAngle / 2)
                {
                    // Check for line of sight
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToScarecrow, out hit, scarecrowViewRange))
                    {
                        if (hit.collider.CompareTag("Scarecrow"))
                        {
                            return true;
                        }
                    }
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
        // Play chase animation and sound
        animator.SetTrigger("Chase");
    }

    private void ChasePlayer()
    {
        agent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < captureDistance)
        {
            CapturePlayer();
        }
    }
    private void StopChase()
    {
        isChasing = false;
        GoToNextPatrolPoint();
        animator.ResetTrigger("Chase");
    }

    private void CapturePlayer()
    {
        
     // Implement capture logic
        GameManager.Instance.GameOver();
        animator.SetTrigger("Dance");

        // Trigger player's sobbing animation
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Sob");
    }

    private void InvestigateSound()
    {
        if (HeardPlayerSound())
        {
            lastHeardSoundPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
            isInvestigating = true;
            agent.SetDestination(lastHeardSoundPosition);
            // Play investigation animation or sound
            animator.SetTrigger("Investigate");
        }

        if (isInvestigating && Vector3.Distance(transform.position, lastHeardSoundPosition) < 1f)
        {
            isInvestigating = false;
            // Resume normal behavior
            animator.ResetTrigger("Investigate");
        }
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
        // Play rest animation
        animator.SetTrigger("Rest");
    }

    private void StopResting()
    {
        isResting = false;
        fatigue = 0;
        // Resume normal behavior
        animator.ResetTrigger("Rest");
    }

    void OnDrawGizmosSelected()
    {
        // Draw sight range in the editor for visualization
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
