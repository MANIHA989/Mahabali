using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Act3Manager : MonoBehaviour
{
    public static Act3Manager Instance;

    public GameObject Vamana;
    public Transform TargetPosition;
    public float movementSpeed = 2f; // Speed of Vamana's movement

    public Image speakerIcon;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI conversationArea;

    public Animator ConversationAnimator;
    public Animator MainCharacterAnimator;
    public Animator SupportingCharacterAnimator;

    public AudioSource TypingSound;

    private bool isMoving = true;
    private bool isMainCharacterAnimating;
    private Queue<DialogueLine> dialogueQueue;
    public float textSpeed = 0.02f;
    public bool isConversationActive = false;

    public bool ReachedTarget = false;

    public DialogueTrigger trigger;

    public float scaleIncreaseSpeed = 0.5f; // Speed of scaling
    public Vector3 maxScale = new Vector3(2f, 2f, 2f); // Maximum size of Vamana

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        dialogueQueue = new Queue<DialogueLine>();
    }

    void Start()
    {

        if (SceneManager.GetActiveScene().name == "Act3")
        {
           isMainCharacterAnimating = true;
}
        TypingSound.enabled=false;
        MainCharacterAnimator.enabled = false;

        Animator vamanaAnimator = Vamana.GetComponent<Animator>();
        
    }

    void Update()
    {
        if (isMoving)
        {
            MoveVamanaToTarget();
        }
    }

    private bool isWalkAnimationPaused = false; // New variable to track walk animation state

    private void MoveVamanaToTarget()
    {
        if (Vamana != null && TargetPosition != null)
        {
            // Check if Vamana is already at the target
            float distanceToTarget = Vector3.Distance(Vamana.transform.position, TargetPosition.position);

            if (distanceToTarget > 0.1f)
            {
                // Vamana is moving towards the target
                Vamana.transform.position = Vector3.MoveTowards(
                    Vamana.transform.position,
                    TargetPosition.position,
                    movementSpeed * Time.deltaTime
                );

                // Play walk animation if not already playing
                Animator vamanaAnimator = Vamana.GetComponent<Animator>();
                if (vamanaAnimator != null && !vamanaAnimator.GetCurrentAnimatorStateInfo(0).IsName("vamana walk"))
                {
                    vamanaAnimator.Play("vamana walk");
                }
            }
            else
            {
                // Vamana has reached the target
                isMoving = false;

                Animator vamanaAnimator = Vamana.GetComponent<Animator>();
                if (vamanaAnimator != null)
                {
                    vamanaAnimator.Play("VamanaIdle"); // Replace with your idle animation state name
                }

                // Stop supporting character's animation
                if (SupportingCharacterAnimator != null)
                {
                    SupportingCharacterAnimator.SetBool("isIdle", true); // Ensure idle animation
                    SupportingCharacterAnimator.SetBool("isWalking", false); // Stop walking animation
                }

                ReachedTarget = true;
                SupportingCharacterAnimator.enabled = false;
                // Add a delay before starting the dialogue
                StartCoroutine(DelayBeforeDialogue());
            }
        }
    }

    private IEnumerator DelayBeforeDialogue()
    {
        yield return new WaitForSeconds(2f); // Adjust delay duration as needed
        if (trigger != null)
        {
            trigger.TriggerDialogue(); // Start the dialogue
        }
    }




    public void StartConversation(Dialogue dialogue)
    {
        if (SupportingCharacterAnimator != null)
        {
            SupportingCharacterAnimator.SetBool("isIdle",true); // Replace with your idle animation state name
            SupportingCharacterAnimator.SetBool("isWalking",false); // Replace with your idle animation state name
        }
        isConversationActive = true;

        dialogueQueue.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            dialogueQueue.Enqueue(dialogueLine);
        }

        ShowNextDialogueLine();
    }

    public void ShowNextDialogueLine()
    {
        conversationArea.text = ""; // Clear the conversation area
        ToggleCharacterAnimations();

        if (dialogueQueue.Count == 0)
        {
            MainCharacterAnimator.enabled = false;
            SupportingCharacterAnimator.enabled = false;
            StartCoroutine(WaitBeforeSceneChange());
            EndConversation();
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();

        if (currentLine.character != null && currentLine.character.icon != null)
        {
            speakerIcon.sprite = currentLine.character.icon;
            speakerIcon.SetNativeSize();
        }

        speakerName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(ShowTextLine(currentLine.line));
    }

    private IEnumerator ShowTextLine(string sentence)
    {
      
        ConversationAnimator.SetTrigger("ShowTrigger");
        yield return new WaitForSeconds(1f);

        if (TypingSound != null)
        {
            TypingSound.enabled = true;
            TypingSound.UnPause();
            TypingSound.Play();
        }

        conversationArea.text = ""; // Clear the conversation area

        foreach (char letter in sentence.ToCharArray())
        {
            conversationArea.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        if (TypingSound != null)
        {
            TypingSound.Pause();
        }

        yield return new WaitForSeconds(0.5f);

        ShowNextDialogueLine();
        ConversationAnimator.SetTrigger("HideTrigger");

        yield return new WaitForSeconds(1.5f);
    }

    private IEnumerator WaitBeforeSceneChange()
    {
        yield return new WaitForSeconds(2f); // Adjust delay duration as needed

        // SceneManager.LoadScene("NextScene"); // Replace with your scene logic
    }

    public void ToggleCharacterAnimations()
    {
        if (isMainCharacterAnimating)
        {
            MainCharacterAnimator.enabled = false;
            SupportingCharacterAnimator.enabled = true;
            isMainCharacterAnimating = false;
        }
        else
        {
            MainCharacterAnimator.enabled = true;
            SupportingCharacterAnimator.enabled = false;
            isMainCharacterAnimating = true;
        }
    }

    void EndConversation()
    {
        ConversationAnimator.SetTrigger("HideTrigger");
        isConversationActive = false;

        if (SceneManager.GetActiveScene().name == "Act3")
        {
            StartCoroutine(ScaleVamanaGradually());
        }

    }

    private IEnumerator ScaleVamanaGradually()
    {
        Vector3 targetScale = maxScale;
        float targetYPosition = Vamana.transform.position.y + (maxScale.y - Vamana.transform.localScale.y); // Adjust Y position based on scale
        Vector3 startPosition = Vamana.transform.position;

        while (Vamana.transform.localScale.x < targetScale.x)
        {
            // Increase scale
            Vamana.transform.localScale += Vector3.one * scaleIncreaseSpeed * Time.deltaTime;

            // Interpolate Y-axis position based on the scaling progress
            float progress = (Vamana.transform.localScale.x - 1) / (targetScale.x - 1);
            Vamana.transform.position = new Vector3(
                Vamana.transform.position.x,
                Mathf.Lerp(startPosition.y, targetYPosition, progress),
                Vamana.transform.position.z
            );

            yield return null; // Wait for the next frame
        }

        // Ensure exact size and final position
        Vamana.transform.localScale = targetScale;
        Vamana.transform.position = new Vector3(
            Vamana.transform.position.x,
            targetYPosition,
            Vamana.transform.position.z
        );

        SceneManager.LoadScene("Act3Scene2");
    }

}
