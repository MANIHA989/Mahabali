using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance; // Singleton instance to easily access the shake effect

    private Vector3 originalPosition; // Stores the camera's initial position
    private Coroutine shakeCoroutine; // Reference to the currently running shake coroutine

    private void Awake()
    {
        // Initialize Singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        originalPosition = transform.localPosition; // Save the starting position
    }

    /// <summary>
    /// Triggers a camera shake effect.
    /// </summary>
    /// <param name="duration">How long the shake lasts.</param>
    /// <param name="magnitude">How strong the shake is.</param>
    public void TriggerShake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float xOffset = Random.Range(-3f, 3f) * magnitude;
            float yOffset = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition; // Reset the camera position
    }
}
