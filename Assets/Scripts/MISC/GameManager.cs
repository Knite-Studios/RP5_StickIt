using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int Health { get; private set; }

    [SerializeField]
    private string SceneName = "MM";
    [SerializeField]
    private GameObject rewardScreen;
    [SerializeField]
    private GameObject[] stars;
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
    private void IncreaseHealth(int amount)
    {
        Health += amount;
        Health = Mathf.Min(Health, 100);
        // Update health UI here
    }

    public void GameOver()
    {
        SceneManager.LoadScene("08_LoadingLose");
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
        ToggleStars();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }
    private void ToggleStars()
    {
      int numOfStars = ObjectiveManager.Instance.StarCount();
        numOfStars = Mathf.Clamp(numOfStars, 0, 3);

        for (int i = 0; i < stars.Length; i++)
        {
            // Enable stars up to the given number and disable the rest
            stars[i].SetActive(i < numOfStars);
        }
    }
}
