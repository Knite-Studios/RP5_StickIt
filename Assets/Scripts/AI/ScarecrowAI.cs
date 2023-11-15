using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowAI : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float viewAngle = 110f; // FOV angle
    public LayerMask playerLayer;
    public Light detectionLight;

    void Start()
    {
        detectionLight.color = Color.red;
    }

    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer < viewAngle / 2)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius, playerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    HandlePlayerDetection(player);
                }
            }
        }
    }

    private void HandlePlayerDetection(GameObject player)
    {
        GameManager.Instance.GameOver();
        // Trigger player's give up animation
        player.GetComponent<Animator>().SetTrigger("GiveUp");

        // Start alarm lights across the farm
        StartAlarmLights();
    }

    private void StartAlarmLights()
    {
        foreach (var light in FindObjectsOfType<Light>())
        {
            if (light.CompareTag("AlarmLight"))
            {
                // Start alarm light effect, e.g., changing color or blinking
                light.color = Color.red; // Example: Change color to red
                // Add more effects as needed
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
