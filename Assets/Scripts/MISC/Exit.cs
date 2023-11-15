using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player has reached the exit
            GameManager.Instance.WinGame();
            // Trigger win game canvas here
        }
    }
}
