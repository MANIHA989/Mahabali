using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public AudioClip backgroundMusic; // Assign the music clip in the Inspector
    private AudioSource audioSource;

    public FadeIn obj;  // Reference to the FadeIn script

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; // Make the music loop
            audioSource.playOnAwake = false; // Prevent it from playing automatically
            audioSource.Play(); // Start playing the background music
        }
        else
        {
            Debug.LogWarning("AudioSource or BackgroundMusic is missing!");
        }
    }

    private void Update()
    {
        // Optional: Add game-wide input or behavior checks here
    }

    // Method to load the next scene with a delay after the fade-in effect
    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneWithDelay());
    }

    private IEnumerator LoadSceneWithDelay()
    {
        // Start the fade-in effect
        StartCoroutine(obj.FadeInEffect());

        // Wait for the duration of the fade-in (adjust the time as needed)
        yield return new WaitForSeconds(1f);  // Adjust delay time here if needed

        // Load the next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1); // Load the next scene by index
    }

    // Method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");

        // If the game is running in the Unity editor, stop playing the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If the game is built, close the application
        Application.Quit();
#endif
    }
}
