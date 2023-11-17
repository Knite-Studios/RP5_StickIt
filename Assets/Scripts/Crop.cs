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

    void Start()
    {
        currentHealth = cropType.healthPoints;
        audioSource = GetComponent<AudioSource>();
        healthBar = GetComponentInChildren<HealthBar>();
    }
    private void Update()
    {
        if(tookDamage)
        {
            healthBar.SetHealth(currentHealth);
        }else
        {
            healthBar.SetHealth(0);
        }
       
    }
    public void TakeDamage(int damage)
    {
        tookDamage = true;
        //turn on canvas based on tookDamage bool
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

        Destroy(gameObject,0.7f);
    }
}
