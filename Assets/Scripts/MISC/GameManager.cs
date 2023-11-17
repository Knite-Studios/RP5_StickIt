using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    private TMP_Text highscoreText;
    private int highscore;

    private void Awake()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
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
        if (highscore < Score)
        {
            highscore = Score;
            highscoreText.text = highscore.ToString();
        }
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
        highscore = PlayerPrefs.GetInt("highscore", 0);//
        if (highscore < Score)//if new score greater than current highscore, then highscore=new score.
        {
            PlayerPrefs.SetInt("highscore", Score);//save highscore
        }
        SceneManager.LoadScene(5);
    }
    public void Retry()
    {
        Score = 0;
        //scoreText.text = Score.ToString();
        highscoreText.text = highscore.ToString();
    }

    public void WinGame()
    {
        SceneManager.LoadScene(3);
    }
}

public enum PowerUpType
{
    HealthBoost,
}