using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 2; // Maximum health of the player
    private int currentHealth; // Current health of the player

    public SpriteRenderer spriteRenderer; // Reference to the sprite renderer for color change
    public Color damageColor = Color.red; // Color to change to when damaged
    private Color originalColor; // Original color of the player sprite
    public float damageColorDuration = 0.2f; // Duration to keep the red color

    private void Start()
    {
        // Initialize health and store the original color
        currentHealth = maxHealth;
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Change color to indicate damage
        if (spriteRenderer != null)
        {
            StartCoroutine(ChangeColorOnDamage());
        }

        // Check if the player is out of health
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator ChangeColorOnDamage()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageColorDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        // Destroy the player object
        Destroy(gameObject);
    }
}
