using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropType cropType;
    private int currentHealth;
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = cropType.healthPoints;
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        PlayDamageSFX();

        if (currentHealth <= 0)
        {
            DestroyCrop();
        }
    }

    private void PlayDamageSFX()
    {
        if (cropType.damageSFX != null)
        {
            audioSource.PlayOneShot(cropType.damageSFX);
        }
    }

    private void DestroyCrop()
    {
        GameManager.Instance.AddScore(cropType.scoreValue);
        Destroy(gameObject);
    }
}
