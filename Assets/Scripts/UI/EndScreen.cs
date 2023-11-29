using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndScreen : MonoBehaviour
{
    [SerializeField]
    private string SceneName = "05_Level1";

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneName);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
    
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}