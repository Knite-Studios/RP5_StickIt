using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StoryScreen : MonoBehaviour
{
    [SerializeField]
    GameObject[] storyScreens;
    int currentScreen = 0;
    private bool isShowingStory = false;

    void Start()
    {
        currentScreen = 0;
        if (storyScreens != null && storyScreens.Length > 0)
        {
            foreach (GameObject screen in storyScreens)
            {
                screen.SetActive(false);
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isShowingStory)
        {
            storyScreens[currentScreen].SetActive(true);
            isShowingStory = true;
            Time.timeScale = 0f;
            GameManager.Instance.canPause = false;
        }
    }

    void Update()
    {
        if (isShowingStory)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextScreen();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                SkipStory();
            }
        }
    }

    void NextScreen()
    {
        if (currentScreen < storyScreens.Length - 1)
        {
            storyScreens[currentScreen].SetActive(false);
            currentScreen++;
            storyScreens[currentScreen].SetActive(true);
        }
        else
        {
            FinishStory();
        }
    }

    void SkipStory()
    {
        Time.timeScale = 1f;
        foreach (GameObject screen in storyScreens)
        {
            screen.SetActive(false);
        }
        GetComponent<Collider>().enabled = false;
        isShowingStory = false;
        GameManager.Instance.canPause = true;
    }

    void FinishStory()
    {
        Time.timeScale = 1f;
        foreach (GameObject screen in storyScreens)
        {
            screen.SetActive(false);
        }
        isShowingStory = false;
        GetComponent<Collider>().enabled = false;
        GameManager.Instance.canPause = true;
    }
}
