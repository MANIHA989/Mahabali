using UnityEngine;
using System.Collections;

public class ShortDemon : MonoBehaviour
{
    public float moveSpeed = 2f;  // Speed at which the demon moves towards the player
    public float raycastLength = 5f;  // Length of the raycast for laser detection
    public LayerMask playerLayer;  // Layer to detect the player
    public Transform raycastOrigin;  // Origin point for raycast (assign this in the Inspector)

    public float knockbackForce = 10f;  // The force applied to the demon when it gets hit by the sword
    public Color hurtColor = Color.red;  // Color to apply when the demon gets hurt
    private Color originalColor;  // Store the original color of the demon's sprite
    private SpriteRenderer spriteRenderer;  // Reference to the demon's SpriteRenderer (child object)

    private Transform player;  // Reference to the player's transform
    private Rigidbody2D rb;  // Reference to the Rigidbody2D component for movement
    private Animator animator;  // Reference to the Animator component

    private bool isAttacking = false;  // Tracks if the demon is currently attacking
    private bool attackOnCooldown = false;  // Tracks if the demon is on attack cooldown
    public float attackCooldown = 2f;  // Delay between attacks in seconds
    public ShortDemonHealth demonHealth;  // Reference to the DemonHealth script

    public int maxAttacks = 3; // Maximum number of attacks before retreating
    private int attackCount = 0; // Tracks the current number of attacks

    static public bool IsDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Get the player transform by tag
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody2D component
        animator = GetComponent<Animator>();  // Get the Animator component
        demonHealth = GetComponent<ShortDemonHealth>();

        // Get the SpriteRenderer from the first child of the demon
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;  // Store the original color of the sprite

        if (raycastOrigin == null)
        {
            Debug.LogError("Raycast origin is not assigned! Please assign it in the Inspector.");
        }
        IsDead = false;
    }

    void Update()
    {
        if (!isAttacking && !attackOnCooldown)
        {
            UpdateRaycastOrigin();
            RaycastToPlayer();  // Check for player detection if not attacking
        }
    }

    void RaycastToPlayer()
    {
        // Horizontal direction towards player
        Vector2 direction = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        // Perform a raycast
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, direction, raycastLength, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            // Start attack sequence
            if (!isAttacking && !attackOnCooldown)
            {
                StartCoroutine(AttackSequence());
            }
        }
        else
        {
            // Continue moving if player not detected
            if (!IsDead)
                MoveTowardsPlayer();
        }

        // Debug the raycast
        Debug.DrawRay(raycastOrigin.position, direction * raycastLength, Color.red);
    }


    void UpdateRaycastOrigin()
    {
        // Align the raycastOrigin with the demon's current position
        raycastOrigin.position = new Vector2(transform.position.x, transform.position.y);
    }


    void MoveTowardsPlayer()
    {
        // Calculate the direction to move towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);  // Move the demon towards the player

        // Set the IsWalking parameter in the Animator based on movement
        animator.SetBool("IsWalking", true);

        // Flip the sprite based on the direction of movement
        if (transform.position.x < player.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x > player.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator AttackSequence()
    {
        isAttacking = true; // Mark as attacking

        // Stop movement and play attack animation
        rb.velocity = Vector2.zero; // Stop movement
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAttacking", true);
        animator.SetTrigger("Attack");

        // Wait for the attack animation to complete dynamically
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // Immediately stop the attack animation
        animator.SetBool("IsAttacking", false);

        attackCount++; // Increment the attack count

        // Check if the maximum number of attacks has been reached
        if (attackCount >= maxAttacks)
        {
            attackCount = 0; // Reset attack count
            StartCoroutine(RetreatToPosition(new Vector2(4.9f, transform.position.y))); // Retreat to x=4.9
            yield break; // Exit the coroutine
        }

        isAttacking = false; // Reset attack state
        StartCoroutine(AttackCooldown()); // Start cooldown
    }


    IEnumerator RetreatToPosition(Vector2 retreatPosition)
    {
        isAttacking = true; // Prevent other actions during retreat
        attackOnCooldown = true; // Prevent further attacks during retreat

        // Start walking animation
        animator.SetBool("IsWalking", true);

        // Move towards the retreat position
        while (Vector2.Distance(transform.position, retreatPosition) > 0.1f)
        {
            Vector2 direction = (retreatPosition - (Vector2)transform.position).normalized;

            // Flip sprite based on movement direction (reversed logic)
            if (direction.x < 0 && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (direction.x > 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y); // Move with velocity
            yield return null;
        }

        // Stop movement once the retreat position is reached
        rb.velocity = Vector2.zero;
        transform.position = retreatPosition;

        // Stop walking animation and start idle animation
        animator.SetBool("IsWalking", false);


        // Reverse flip logic to face the player
        if (player.position.x > transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        animator.SetBool("IsIdle", true);

        yield return new WaitForSeconds(1f); // Idle state duration

        // Stop idle animation before resuming movement
        animator.SetBool("IsIdle", false);
        yield return new WaitForSeconds(0.5f); // Optional delay before following

        // Resume following the player
        isAttacking = false;
        attackOnCooldown = false;
    }

    IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;  // Start cooldown
        yield return new WaitForSeconds(attackCooldown);  // Wait for cooldown duration
        attackOnCooldown = false;  // Cooldown over
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            // Trigger hurt animation
            animator.SetTrigger("Hurt");

            // Apply knockback force
            Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            spriteRenderer.color = hurtColor;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            // Call TakeDamage from DemonHealth
            if (demonHealth != null)
            {
                demonHealth.TakeDamage(1);  // Assume 1 is the damage value
            }

            // Optionally, reset color after a short delay
            StartCoroutine(ResetColor());
        }

        if (other.CompareTag("Player"))
        {
            // Example of camera shake when enemy hits the player
            CameraShake.Instance.TriggerShake(0.3f, 0.2f); // Duration: 0.3 seconds, Magnitude: 0.2
        }
        else if (other.CompareTag("Ground"))
        {
            // Example of camera shake when weapon touches the ground
            CameraShake.Instance.TriggerShake(0.4f, 0.4f); // Stronger shake for ground impact
        }
    }

    private IEnumerator ResetColor()
    {
        // Wait for a short time (e.g., 0.5 seconds)
        yield return new WaitForSeconds(0.5f);

        // Reset the sprite color back to the original color
        spriteRenderer.color = originalColor;
    }


}
