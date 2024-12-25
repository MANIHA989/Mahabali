using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private Animator animator;

    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Automatically find and assign the PlayerHealth component if not assigned
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Update animator parameter for running
        animator.SetBool("isRunning", moveInput != 0);

        // Flip player sprite based on movement direction
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Handle attack
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Attack();
        }

        // Handle defend
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            Defend();
        }
    }

    void Attack()
    {
        animator.SetTrigger("isAttacking");
        // Add additional logic for attack (e.g., damage detection) here
    }

    void Defend()
    {
        animator.SetTrigger("isDefending");
        // Add additional logic for defend (e.g., block mechanics) here
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if colliding with an object tagged as "weapon"
        if (other.gameObject.CompareTag("weapon"))
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Deal 1 damage to the player
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not assigned!");
            }
        }
    }
}
