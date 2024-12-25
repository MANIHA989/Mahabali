using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{

    public static FadeIn Instance;
    public Image blackScreen;  // Reference to the black screen image
    public float fadeDuration = 1f;  // Duration for the fade-in effect


    private void Awake()
    {
        if (Instance == null)
            Instance = this;


    }

    void Start()
    {
        // Start the fade-in effect when the scene starts
        //StartCoroutine(FadeInEffect());
    }

    public IEnumerator FadeInEffect()
    {
        float timeElapsed = 0f;
        Color initialColor = blackScreen.color;
        initialColor.a = 1f;  // Start with fully opaque (black screen)

        // Fade from fully opaque to fully transparent
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - timeElapsed / fadeDuration);  // Decrease alpha over time
            blackScreen.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Ensure the screen is fully transparent after the fade
        blackScreen.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }

}
