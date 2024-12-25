using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VamanaSteps : MonoBehaviour
{
    public Animator VamanaAnimator;
    public GameObject Vamana;
    public Transform Target;
    public float movementSpeed = 2f; // Speed at which Vamana moves

    // Start is called before the first frame update
    void Start()
    {
        // Start Vamana's walking animation
        if (VamanaAnimator != null)
        {
            VamanaAnimator.Play("vamana walk");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveVamanaToTarget();
    }

    private void MoveVamanaToTarget()
    {
        if (Vamana != null && Target != null)
        {
            // Calculate the distance to the target
            float distanceToTarget = Vector3.Distance(Vamana.transform.position, Target.position);

            if (distanceToTarget > 0.1f) // Stop when close to the target
            {
                // Move Vamana towards the target
                Vamana.transform.position = Vector3.MoveTowards(
                    Vamana.transform.position,
                    Target.position,
                    movementSpeed * Time.deltaTime
                );

                // Ensure the walking animation is playing
                if (!VamanaAnimator.GetCurrentAnimatorStateInfo(0).IsName("vamana walk"))
                {
                    VamanaAnimator.Play("vamana walk");
                }
            }
            else
            {
                // Vamana has reached the target
                if (VamanaAnimator != null)
                {
                    VamanaAnimator.Play("VamanaIdle"); // Replace with the name of your idle animation
                }
            }
        }
    }
}
