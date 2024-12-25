using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMesh Pro

public class ItemClickHandler : MonoBehaviour
{
    public GameObject Background;
    public GameObject obj;
    private Vector3 originalScale;
    public TMP_Text statusText; // Reference to a TextMesh Pro UI element
    public AudioSource typingAudioSource; // Reference to the AudioSource for typing sound
    public AudioSource clickAudioSource; // Reference to the AudioSource for click sound

    public string firstText = "You have 6 items to find, find them to unlock Amritam";
    public float displayTime = 3.0f; // Time to show each text
    public float typingSpeed = 0.05f; // Time between each character
    public float moveUpDistance = 1.0f; // Distance to move up along the Y-axis
    public float moveSpeed = 1.0f; // Speed of the movement

    private bool itemDeactivated = false; // Flag to check if the current item is deactivated

    private static List<ItemClickHandler> allItems = new List<ItemClickHandler>(); // Track all instances of the items
    private static int totalItems = 6; // Total number of items (adjust this number as necessary)

    public FindAmritam findAmritam;
    void Start()
    {
        originalScale = transform.localScale; // Save the initial scale
        allItems.Add(this); // Add this instance to the list of all items

        // Ensure the click audio source is not playing at the start
        if (clickAudioSource != null && clickAudioSource.isPlaying)
        {
            clickAudioSource.Stop();
        }

        StartCoroutine(MoveAndDisplayText());
    }


    void Update()
    {
        // Check if all items are deactivated, only then reactivate the player
        if (AreAllItemsDeactivated())
        {
            findAmritam.ReactivatePlayer();
        }
    }

    private IEnumerator MoveAndDisplayText()
    {
        // Move the object upwards along the Y-axis before displaying text
        Vector3 targetPosition = obj.transform.position + new Vector3(0, moveUpDistance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveSpeed)
        {
            obj.transform.position = Vector3.Lerp(obj.transform.position, targetPosition, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        obj.transform.position = targetPosition; // Ensure the final position is reached

        // Now start the text typing effect after the movement
        yield return StartCoroutine(DisplayTexts());
    }

    private IEnumerator DisplayTexts()
    {
        if (statusText != null)
        {
            // Display the first text with typewriter effect
            yield return StartCoroutine(TypeWriterEffect(firstText));
            yield return new WaitForSeconds(displayTime); // Wait for the set duration
        }
    }

    private IEnumerator TypeWriterEffect(string textToType)
    {
        string currentText = ""; // Temporary string to accumulate the text
        for (int i = 0; i < textToType.Length; i++)
        {
            currentText += textToType[i]; // Add the next character to currentText
            statusText.text = currentText; // Update the status text once for each iteration

            // Play typing sound for each character
            if (typingAudioSource != null && !typingAudioSource.isPlaying) // Play sound only if no sound is currently playing
            {
                typingAudioSource.Play(); // Play the sound attached to the typing AudioSource
            }

            yield return new WaitForSeconds(typingSpeed); // Wait for the defined typing speed
        }
        typingAudioSource.Pause();
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
        Background.SetActive(false);

        // After deactivating, check if it's the last item and reactivate the player
        itemDeactivated = true;
    }

    void OnMouseDown()
    {
        // Play the click sound
        if (clickAudioSource != null)
        {
            clickAudioSource.Play(); // Play the sound for clicking
        }

        // Increase the size of the item
        transform.localScale = originalScale * 2.0f;

        // Update the TextMesh Pro element
        if (statusText != null)
        {
            statusText.text = $"{gameObject.name} clicked!";
        }

        // Deactivate the item after a short delay
        StartCoroutine(DeactivateItem());
    }

    private IEnumerator DeactivateItem()
    {
        yield return new WaitForSeconds(0.3f); // Wait for a short duration
        gameObject.SetActive(false); // Deactivate the game object

        // After deactivating, check if it's the last item and reactivate the player
        itemDeactivated = true;
        CheckIfLastItemDeactivated();
    }

    private void CheckIfLastItemDeactivated()
    {
        // Check if all items are deactivated
        if (AreAllItemsDeactivated())
        {
            findAmritam.ReactivatePlayer();
        }
    }

    private bool AreAllItemsDeactivated()
    {
        // Return true if all items in the list are deactivated
        foreach (ItemClickHandler item in allItems)
        {
            if (item.gameObject.activeSelf) // If any item is still active, return false
            {
                return false;
            }
        }
        return true; // All items are deactivated
    }
}
