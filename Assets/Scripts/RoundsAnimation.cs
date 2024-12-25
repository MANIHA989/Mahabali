using UnityEngine;

public class ZoomOutAnimation : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(0.5f, 0.5f, 0.5f); // Desired scale after zooming out
    public float duration = 2f; // Time in seconds for the zoom-out animation

    private Vector3 initialScale; // Original scale of the GameObject
    private float elapsedTime = 0f; // Tracks elapsed time for the animation

    private void Start()
    {
        initialScale = transform.localScale; // Save the initial scale of the GameObject
    }

    private void Update()
    {
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
        }
    }
}
