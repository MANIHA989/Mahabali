using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    public Animator animator; // Reference to the animator
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

    public float attackCooldown = 0.5f; // Time between attacks
    private bool canAttack = true; // To manage attack cooldown

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Automatically find and assign the PlayerHealth component if not assigned
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        // Handle walking based on arrow keys
        HandleWalking();

        // Set "isRunning" to true when moving, false when idle
        animator.SetBool("isRunning", movement.x != 0);

        // Handle attacking
        HandleAttacking();
    }

    // Function to handle player walking
    void HandleWalking()
    {
        movement.x = 0f;

        // Check arrow keys for movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.x = 1f;
        }

        // Flip the sprite when moving left or right
        if (movement.x < 0)
            spriteRenderer.flipX = true;
        else if (movement.x > 0)
            spriteRenderer.flipX = false;
    }

    // Function to handle attacking
    void HandleAttacking()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            // Trigger attack animation
            animator.SetTrigger("Attack");

            // Start cooldown
            canAttack = false;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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
