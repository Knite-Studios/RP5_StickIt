using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Score { get; private set; }
    public int Health { get; private set; }

    // Declare the event for score changes
    public delegate void ScoreChanged();
    public event ScoreChanged OnScoreChanged;

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
        OnScoreChanged?.Invoke(); // Trigger the event
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
        SceneManager.LoadScene(0);
    }
    
    public void WinGame()
    {
        SceneManager.LoadScene(0);
    }
}

public enum PowerUpType
{
    HealthBoost,
}
