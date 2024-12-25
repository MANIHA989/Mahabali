using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class Enemy : MonoBehaviour
{
    public int health = 1; // Health of the enemy
    private EnemyManager enemyManager; // Reference to the manager
    private Animator animator; // Reference to the Animator
    private bool isDying = false; // Prevent multiple triggers of death logic
    private static int enemy_counter = 0; // Shared across enemies if needed

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return; // Prevent logic if enemy is already dying

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDying) return; // Ensure death logic only executes once
        isDying = true;

        animator.SetTrigger("isDead");

        if (enemyManager != null)
        {
            enemyManager.EnemyKilled();
        }

        enemy_counter++; // Increment the counter when this enemy dies
        CheckRoundTransition(); // Check if we need to change rounds

        Destroy(gameObject, GetAnimationClipLength("Death")); // Destroy this enemy after death animation
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the enemy is hit by the player's sword
        if (collision.CompareTag("Sword"))
        {
            TakeDamage(1); // Reduce health by 1
        }
    }

    private void CheckRoundTransition()
    {
        Debug.Log("Enemy counter: " + enemy_counter); // Check the counter value
        if (enemy_counter >= 2) // When counter reaches 2, load Scene_2
        {
            LoadNextScene(); // Load the next scene after killing 2 enemies
        }
    }

    // Function to load the next scene
    private void LoadNextScene()
    {
        SceneManager.LoadScene("Scene_2"); // Load the scene named "Scene_2"
    }

    // Helper function to get the length of the animation
    private float GetAnimationClipLength(string animationName)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        Debug.LogError("Animation clip not found: " + animationName);
        return 0f; // Default to immediate destroy if clip not found
    }
}
