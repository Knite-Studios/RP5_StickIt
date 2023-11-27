using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class StoryScreen : MonoBehaviour
{
    [SerializeField]
    GameObject storyScreen;

    private bool isTriggered = false;

    void Start()
    {
        if (storyScreen != null)
            storyScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            if (storyScreen != null)
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
                if (storyScreen != null)
                    storyScreen.SetActive(false);
                GetComponent<Collider>().enabled = false;
                isTriggered = false;
                GameManager.Instance.canPause = true;

            }
        }
    }
}
