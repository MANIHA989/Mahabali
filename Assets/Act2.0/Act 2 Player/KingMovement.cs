using System.Collections;
using UnityEngine;

public class KingMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    private Rigidbody2D rb;
    public float knockbackForce = 7f;  // The force applied to the demon when it gets hit by the sword
    private Animator animator;
    private bool isHurt;  // New variable to track if the player is hurt

    private float attackCooldown = 1f;  // Time to wait before the player can attack again
    private float cooldownTimer = 0f;  // Timer to track cooldown state

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isHurt)  // Only allow movement and attack if not hurt
        {
            HandleMovement();
            HandleAttack();
        }

        // Handle cooldown timer counting down
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        Flip(moveInput);

        if (!animator.GetBool("IsAttacking"))
        {
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            if (moveInput != 0)
            {
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsIdle", true);
            }
        }
    }

    void HandleAttack()
    {
        // Check for Mouse Button Down or Spacebar press to trigger attack
        // Ensure the cooldown timer is finished (cooldownTimer <= 0)
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && cooldownTimer <= 0 && !animator.GetBool("IsAttacking"))
        {
            animator.SetTrigger("Attack");
            animator.SetBool("IsAttacking", true);

            // Start the cooldown timer after the attack animation starts
            cooldownTimer = attackCooldown;  // Reset the cooldown timer to the cooldown duration

            StartCoroutine(ResetAttackState());  // Reset the attack state after animation ends
        }
    }

    IEnumerator ResetAttackState()
    {
        // Wait for the attack animation to finish (adjust the time based on your animation length)
        yield return new WaitForSeconds(0.5f);  // Wait for animation to finish (change time to match animation duration)

        animator.SetBool("IsAttacking", false);  // Reset the attack state
    }

    void Flip(float moveInput)
    {
        Transform playerSpriteTransform = transform.GetChild(0);

        if (playerSpriteTransform != null)
        {
            if (moveInput < 0 && playerSpriteTransform.localScale.x > 0)
            {
                playerSpriteTransform.localScale = new Vector3(-playerSpriteTransform.localScale.x, playerSpriteTransform.localScale.y, playerSpriteTransform.localScale.z);
            }
            else if (moveInput > 0 && playerSpriteTransform.localScale.x < 0)
            {
                playerSpriteTransform.localScale = new Vector3(-playerSpriteTransform.localScale.x, playerSpriteTransform.localScale.y, playerSpriteTransform.localScale.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapon"))
        {
            animator.SetTrigger("Hurt");
             Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
