using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSeclector : MonoBehaviour
{   [Header("Level names In order"),SerializeField] 
    private string[] levelName;
    public void LoadLevel_0()
    {
        SceneManager.LoadScene(levelName[0]);
    }
    public void LoadLevel_1()
    {
        SceneManager.LoadScene(levelName[1]);
    }
    public void LoadLevel_2()
    {
        SceneManager.LoadScene(levelName[2]);
    }
    public void LoadLevel_3()
    {
        SceneManager.LoadScene(levelName[3]);
    }
}
