using System.Collections;
using UnityEngine;

public class PlayerUnderWorld : MonoBehaviour
{
    public Transform TargetPosition; // Target position to move toward
    public Animator PlayerAnimator; // Reference to the Animator
    public float moveSpeed = 2f; // Speed at which the player moves
    public Transform player; // Reference to the player's transform
    public Vector3 TargetScale = new Vector3(1f, 1f, 1f); // Target scale for the player
    public float scaleSpeed = 1f; // Speed for scaling the player
    public float fadeDuration = 1f; // Duration of the fade-out effect

    private SpriteRenderer playerSpriteRenderer; // SpriteRenderer of the player

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator.SetBool("isWalking", true); // Start walking animation
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>(); // Get the player's SpriteRenderer
        StartCoroutine(MoveToTargetCoroutine());
    }

    IEnumerator MoveToTargetCoroutine()
    {
        // While the player has not reached the target position or the target scale
        while (Vector3.Distance(player.position, TargetPosition.position) > 0.1f ||
               player.localScale != TargetScale)
        {
            // Move the player toward the target position
            player.position = Vector3.MoveTowards(player.position, TargetPosition.position, moveSpeed * Time.deltaTime);

            // Scale the player toward the target scale
            player.localScale = Vector3.MoveTowards(player.localScale, TargetScale, scaleSpeed * Time.deltaTime);

            // Wait for the next frame
            yield return null;
        }

        // Stop walking animation once the target is reached
        PlayerAnimator.SetBool("isWalking", false);
        yield return new WaitForSeconds(1f);

        // Start fading effect
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color initialColor = playerSpriteRenderer.color;

        // Fade out by decreasing alpha
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            playerSpriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Ensure alpha is set to 0 after fading
        playerSpriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }
}