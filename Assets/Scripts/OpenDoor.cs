using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField]
    private float doorSpeed = 1f;

    private float openAngle = -90f; // Change this to the desired open angle

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
        targetRotation = Quaternion.Euler(initialRotation.eulerAngles + new Vector3(0f, openAngle, 0f));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(OpenDoorCoroutine());
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.enabled = false;
            }
        }
    }

    private IEnumerator OpenDoorCoroutine()
    {
        float duration = 1f / doorSpeed; // Adjust duration for rotation

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, normalizedTime);

            yield return null;
        }
    }
}
