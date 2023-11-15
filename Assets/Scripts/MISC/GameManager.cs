using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public int Health { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();
    }

    void Update()
    {
        // Update game state if needed
    }

    private void InitializeGame()
    {
        // Initialize game variables
        Score = 0;
        Health = 100; // Example starting health
        // Other initialization code
    }

    public void AddScore(int points)
    {
        Score += points;
        // Update score UI here
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            GameOver();
        }
        // Update health UI here
    }

    public void CollectPowerUp(PowerUpType powerUpType)
    {
        // Handle different power-up types
        switch (powerUpType)
        {
            case PowerUpType.HealthBoost:
                IncreaseHealth(20); // Example value
                break;
            // Add other power-up types here
        }
    }

    private void IncreaseHealth(int amount)
    {
        Health += amount;
        // Ensure health does not exceed max value
        Health = Mathf.Min(Health, 100);
        // Update health UI here
    }

    public void GameOver()
    {
        // Handle game over logic
        // Show game over screen, etc.
    }
    
    public void WinGame()
    {
        // Handle game over logic
        // Show game over screen, etc.
    }
}

public enum PowerUpType
{
    HealthBoost,
    // Define other power-up types here
}
