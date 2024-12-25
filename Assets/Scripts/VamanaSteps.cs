using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VamanaSteps : MonoBehaviour
{
    public Animator VamanaAnimator;
    public GameObject Vamana;
    public Transform Target;
    public float movementSpeed = 2f; // Speed at which Vamana moves
    public float delayBeforeWalking = 2f; // Delay before Vamana starts walking

    // Start is called before the first frame update
    void Start()
    {
        // Start the delay before Vamana starts walking
        StartCoroutine(DelayBeforeWalking());
    }

    private IEnumerator DelayBeforeWalking()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayBeforeWalking);

        // Start Vamana's walking animation and begin moving towards the target
        if (VamanaAnimator != null)
        {
            VamanaAnimator.Play("vamana walk");
        }

        // Continue moving Vamana towards the target
        while (Vector3.Distance(Vamana.transform.position, Target.position) > 0.1f)
        {
            MoveVamanaToTarget();
            yield return null;
        }

        // Once Vamana reaches the target, play idle animation
        if (VamanaAnimator != null)
        {
            VamanaAnimator.Play("VamanaIdle");
        }

        //SceneManager.LoadScene("Act3Scene3");
    }

    // Move Vamana towards the target
    private void MoveVamanaToTarget()
    {
        if (Vamana != null && Target != null)
        {
            // Move Vamana towards the target
            Vamana.transform.position = Vector3.MoveTowards(
                Vamana.transform.position,
                Target.position,
                movementSpeed * Time.deltaTime
            );
        }

    }
}
