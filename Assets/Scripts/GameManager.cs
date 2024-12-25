using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioClip backgroundMusic; // Assign the music clip in the Inspector
    private AudioSource audioSource;

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

    // Method to load the next scene
    public void LoadNextScene()
    {
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
