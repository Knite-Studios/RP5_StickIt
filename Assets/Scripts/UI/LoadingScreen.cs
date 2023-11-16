using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public float fakeLoadingSpeed = 0.2f;
    private float progress = 0f;

    void Start()
    {
        if(loadingSlider == null)
        {
            Debug.LogWarning("No reference to loadingSlider. Please set in the Inspector.");
            return;
        }

        StartCoroutine(FakeLoadingCoroutine());
    }

    IEnumerator FakeLoadingCoroutine()
    {
        while(progress < 1f)
        {
            progress += fakeLoadingSpeed * Time.deltaTime;
            loadingSlider.value = progress;
            yield return null;
        }
        SceneManager.LoadScene(2);
    }
}
