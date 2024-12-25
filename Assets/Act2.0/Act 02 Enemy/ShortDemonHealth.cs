using System.Collections;
using UnityEngine;

public class ShortDemonHealth : MonoBehaviour
{
    public int maxHealth = 1;  // Maximum health of the demon (1 hit before death)
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Call this method when the demon is hit
    public void TakeDamage(int damage)
    {
        Debug.Log("Took Damage ");
        currentHealth -= damage;

        // If health is less than or equal to 0, trigger death
        if (currentHealth <= 0)
        {
            ShortDemon.IsDead = true;
            Die();
            DemonCombat.counter++;
            Debug.Log(DemonCombat.counter);
        }
    }

    // Trigger the death animation and destroy the demon object
    private void Die()
    {
        animator.SetTrigger("Death");  // Play the death animation (you need to set up the "Death" trigger in Animator)

        // Get the length of the death animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length + 1.5f;

        // Destroy the game object after the animation has finished
        StartCoroutine(DestroyAfterAnimation(animationLength));
    }

    // Coroutine to destroy the object after the death animation
    private IEnumerator DestroyAfterAnimation(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);  // Wait for the animation to finish
        Destroy(gameObject);  // Destroy the demon object
    }
}
