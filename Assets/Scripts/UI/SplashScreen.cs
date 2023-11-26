using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField]
    private string sceneName = "03_Loading";

    public void LoadScene()
    {
      SceneManager.LoadScene(sceneName);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadScene(); 
        }

    }
}
