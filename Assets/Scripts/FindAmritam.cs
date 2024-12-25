using System.Collections;
using System.Collections.Generic;
using TMPro; // Import TextMesh Pro namespace
using UnityEngine;

public class FindAmritam : MonoBehaviour
{
    public GameObject Player;
    public GameObject Diamond;
    public Animator PlayerAnimator;
    public Transform TargetPosition;
    public GameObject PlayerDialogue;
    public TextMeshProUGUI DialogueText; // Reference to TextMesh Pro component
    public AudioSource TypingAudioSource; // Reference to an AudioSource component

    public Vector3 TargetScale = new Vector3(2.0f, 2.0f, 2.0f); // Final size of the player
    public float MoveDuration = 3.0f; // Duration of the movement and scaling
    public float DelayDuration = 1.5f; // Delay duration before reactivating the player
    public float TypewriterSpeed = 0.05f; // Speed of the typewriter effect

    private bool isDelayActive = false; // To track if the delay is active
    private float delayTimer = 0f; // Timer to track the delay

    void Start()
    {
    }

    void Update()
    {
        if (isDelayActive)
        {
            delayTimer += Time.deltaTime;

            if (delayTimer >= DelayDuration)
            {
                // Delay is complete, proceed to reactivate the player
                isDelayActive = false;
                PerformReactivatePlayer();
            }
        }
    }

    public void ReactivatePlayer()
    {
        // Start the delay
        isDelayActive = true;
        delayTimer = 0f; // Reset the timer
    }

    private void PerformReactivatePlayer()
    {
        // Reactivate the player
        Player.SetActive(true);
        Diamond.SetActive(true);

        // Trigger the player's animation
        if (PlayerAnimator != null)
        {
            PlayerAnimator.SetBool("isWalking", true); // Set isWalking to true when the movement starts
        }

        // Move and scale the player
        StartCoroutine(MovePlayerToTarget());
    }

    private IEnumerator MovePlayerToTarget()
    {
        float elapsedTime = 0f;

        Vector3 startingPosition = Player.transform.position;
        Vector3 startingScale = Player.transform.localScale; // Initial size of the player
        Vector3 targetPosition = TargetPosition.position;

        while (elapsedTime < MoveDuration)
        {
            // Interpolate position
            Player.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / MoveDuration);

            // Interpolate scale
            Player.transform.localScale = Vector3.Lerp(startingScale, TargetScale, elapsedTime / MoveDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final position and scale are reached
        Player.transform.position = targetPosition;
        Player.transform.localScale = TargetScale;

        // Set isWalking to false after reaching the target
        if (PlayerAnimator != null)
        {
            PlayerAnimator.SetBool("isWalking", false);
        }

        // Activate player dialogue
        PlayerDialogue.SetActive(true);

        // Start typewriter effect
        if (DialogueText != null)
        {
            string fullText = "And with this heaven shall bow to us";
            StartCoroutine(TypewriterEffect(fullText));
        }
    }

    private IEnumerator TypewriterEffect(string text)
    {
        DialogueText.text = ""; // Clear existing text
        foreach (char letter in text.ToCharArray())
        {
            DialogueText.text += letter; // Append one character at a time

            // Play the typing sound if audio source is set
            if (TypingAudioSource != null && !TypingAudioSource.isPlaying)
            {
                TypingAudioSource.Play();
            }

            yield return new WaitForSeconds(TypewriterSpeed);
        }
    }
}
