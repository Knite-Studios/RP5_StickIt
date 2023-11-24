using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Health { get; private set; }

    [SerializeField]
    private string SceneName = "MM";
    [SerializeField]
    private GameObject rewardScreen;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
         //   DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        rewardScreen.SetActive(false);
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
        Health = 100;
        // Other initialization code
    }

    public void AddScore(int points)
    {

    }

    private void IncreaseHealth(int amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, 100);
        // Update health UI here
    }

    public void GameOver()
    {
        //togglePopUp
        
        
    }
    public void Continue()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneName);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WinGame()
    {
        rewardScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

}
