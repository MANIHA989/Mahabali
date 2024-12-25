using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Animator animator;  // Reference to the animator
    public float attackCooldown = 0.5f;
    private float attackTimer = 0f;

    void Update()
    {
        attackTimer -= Time.deltaTime;

        // If space is pressed and attack cooldown is over
        if (Input.GetKeyDown(KeyCode.Space) && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void Attack()
    {
        // Set isAttacking to true and trigger the attack
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("Attack");
    }

    // This function can be called at the end of the attack animation (using an animation event)
    void OnAttackAnimationEnd()
    {
        // Reset isAttacking to false
        animator.SetBool("isAttacking", false);
    }
}
