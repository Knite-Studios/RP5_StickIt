using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class TutorialScreen : MonoBehaviour
{

    [SerializeField]
    GameObject tutorialText;


    void Start()
    {
        if (tutorialText != null)
            tutorialText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialText != null)
                tutorialText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (tutorialText != null)
                tutorialText.SetActive(false);
        }
    }
           
   
}
