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
         //   DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        UpdateHighscoreText();
    }

    void Start()
    {
        InitializeGame();
        UpdateHighscoreText();
    }

    void Update()
    {
        // Update game state if needed
    }

    private void InitializeGame()
    {
        Score = 0;
        Health = 100;
        // Other initialization code
    }

    public void AddScore(int points)
    {
        Score += points;
        OnScoreChanged?.Invoke();
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
        switch (powerUpType)
        {
            case PowerUpType.HealthBoost:
                IncreaseHealth(20);
                break;
                // Add other power-up types here
        }
    }

    private void IncreaseHealth(int amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, 100);
        // Update health UI here
    }

    public void GameOver()
    {
        if (highscore < Score)
        {
            PlayerPrefs.SetInt("highscore", Score);
        }
        GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene(5);
        UpdateHighscoreText();
    }

    public void Retry()
    {
        Score = 0;
    }

    public void WinGame()
    {
        SceneManager.LoadScene(3);
        UpdateHighscoreText();
    }

    private void UpdateHighscoreText()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = "High Score: " + highscore.ToString();
    }

}

public enum PowerUpType
{
    HealthBoost,
}
