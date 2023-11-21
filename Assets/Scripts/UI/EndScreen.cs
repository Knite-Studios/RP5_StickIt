using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndScreen : MonoBehaviour
{
    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    public void RestartGame()
    {
       SceneManager.LoadScene(1);
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}