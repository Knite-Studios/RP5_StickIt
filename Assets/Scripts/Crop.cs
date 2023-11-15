using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Crop : MonoBehaviour
{
    public CropType cropType;
    public TextMeshProUGUI healthText;

    private int currentHealth;
    private AudioSource audioSource;
    public HealthBar healthBar; // Reference to the HealthBar script

    void Start()
    {
        currentHealth = cropType.healthPoints;
        audioSource = GetComponent<AudioSource>();
        UpdateHealthUI();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        DamagePopup.Create(transform.position, damage); // For floating damage numbers
        UpdateHealthUI(); // For updating the health bar
        PlayDamageSFX(); // For playing the damage sound effect

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

    private void UpdateHealthUI()
    {
        healthBar.SetHealth(currentHealth); // Update the health bar
    }

    private void DestroyCrop()
    {
        // Add score to the player
        GameManager.Instance.AddScore(cropType.scoreValue);
        Destroy(gameObject);
    }
}