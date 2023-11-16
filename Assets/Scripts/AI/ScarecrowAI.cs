using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowAI : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float viewAngle = 110f; // FOV angle
    public LayerMask playerLayer;
    public AudioClip alertSFX;
    public float rotationSpeed = 30f; // Degrees per second

    private Light detectionLight;
    private AudioSource audioSource;
    private Animator animator;
    private bool isAlerted = false;

    void Start()
    {
        detectionLight = GetComponentInChildren<Light>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        detectionLight.color = Color.red;
    }

    void Update()
    {
        if (!isAlerted)
        {
            RotateScarecrow();
            DetectPlayer();
        }
    }

    private void RotateScarecrow()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void DetectPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < viewAngle / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    HandlePlayerDetection();
                }
            }
        }
    }

    private void HandlePlayerDetection()
    {
        isAlerted = true;
        animator.SetTrigger("Alert");
        PlayAlertSFX();
        StartAlarmLights();
    }

    private void PlayAlertSFX()
    {
        if (alertSFX != null)
        {
            audioSource.PlayOneShot(alertSFX);
        }
    }

    private void StartAlarmLights()
    {
        foreach (var light in FindObjectsOfType<Light>())
        {
            if (light.CompareTag("AlarmLight"))
            {
                light.color = Color.red; // Example: Change color to red
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
