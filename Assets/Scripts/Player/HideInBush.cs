using Percy.EnemyVision;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInBush : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           other.GetComponent<PlayerMovement>().ToggleCrouch();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ToggleCrouch();
        }
    }
    
}
