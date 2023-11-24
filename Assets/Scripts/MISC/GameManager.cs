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

    [SerializeField]
    private string LoadToAfterWin = "09_LoadingWin";
    [SerializeField]
    private GameObject rewardScreen;
    [Header("Star System"),SerializeField]
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

    public void GameOver()
    {
        SceneManager.LoadScene("08_LoadingLose");
    }
    public void Continue()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(LoadToAfterWin);
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
