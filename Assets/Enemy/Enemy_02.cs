using UnityEngine;
using System.Collections;

public class Enemy_02 : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 0.5f;
    public int damage = 1;
    public int health = 3;

    private Transform player;
    private Animator animator;
    private bool isAttacking = false;
    private bool isInRange = false;
    public LayerMask detectionLayer;

    private EnemyManager enemyManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    void Update()
    {
        if (player == null) return;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, GetDirectionToPlayer(), attackRange, detectionLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            isInRange = true;
            if (!isAttacking)
            {
                StartAttack(); // Start attacking if not already attacking
            }
        }
        else
        {
            isInRange = false;
            if (!isAttacking)
            {
                MoveTowardsPlayer(); // Move towards player if not attacking
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        animator.SetBool("isRunning", true);
        animator.SetBool("Attack", false);

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("isRunning", false); // Stop running animation
        animator.SetBool("Attack", true); // Start attack animation
        StartCoroutine(AttackCooldown()); // Start the cooldown after the attack
    }

    private IEnumerator AttackCooldown()
    {
        // Wait for the specified cooldown time
        yield return new WaitForSeconds(attackCooldown);

        animator.SetBool("Attack", false); // Stop the attack animation after cooldown
        isAttacking = false; // Allow a new attack if the player is still in range

        // After cooldown, if the player is still in range, start attacking again
        if (isInRange)
        {
            StartAttack(); // Start attacking again
        }
    }

    public void DealDamage()
    {
        if (isInRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    private Vector2 GetDirectionToPlayer()
    {
        return (player.position - transform.position).normalized;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (enemyManager != null)
        {
            enemyManager.EnemyKilled();
        }
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)GetDirectionToPlayer() * attackRange);
        }
    }

    // Detects collision with sword and destroys the enemy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            Die(); // Destroy the enemy if it collides with the sword
        }
    }
}
