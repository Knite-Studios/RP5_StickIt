using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Crop : MonoBehaviour
{
    public CropType cropType;
    private int currentHealth;
    private AudioSource audioSource;
    private HealthBar healthBar;
    private bool tookDamage = false;

 //   public static event Action<CropType> OnCropDestroyed; // Event for notifying destruction of a crop

    void Start()
    {
        currentHealth = cropType.healthPoints;
        audioSource = GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<HealthBar>();
    }
    private void Update()
    {
        if(healthBar != null)
        {
            if (tookDamage)
            {
                healthBar.SetHealth(currentHealth);
            }
            else
            {
                healthBar.SetHealth(0);
            }
        }
        
       
    }
    public void TakeDamage(int damage)
    {
        tookDamage = true;
        //turn on canvas based on tookDamage bool
        currentHealth -= damage;
        PlayDamageSFX();

        if (currentHealth == 0)
        {
            DestroyCrop();
        }
    }

    private void PlayDamageSFX()
    {
        if (cropType.damageSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(cropType.damageSFX);
        }
    }

    public void DestroyCrop()
    {
        Debug.Log("Crop Destroyed");
        ObjectiveManager.Instance.HandleCropDestroyed(cropType);
        //GameManager.Instance.AddScore(cropType.scoreValue);
        if (cropType.destroyedPrefab != null)
            Instantiate(cropType.destroyedPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject,0.1f);
    }
}
