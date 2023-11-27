using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScreen : MonoBehaviour
{
    [SerializeField]
    GameObject storyScreen;

    private bool isTriggered = false;

    void Start()
    {
        storyScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            storyScreen.SetActive(true);
            Time.timeScale = 0f;
            isTriggered = true;
            GameManager.Instance.canPause = false;
        }
    }

    void Update()
    {
        if (isTriggered)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Time.timeScale = 1f;
                storyScreen.SetActive(false);
                GetComponent<Collider>().enabled = false;
                isTriggered = false;
                GameManager.Instance.canPause = true;

            }
        }
    }
}
