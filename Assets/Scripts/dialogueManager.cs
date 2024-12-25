using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public float typingSpeed = 0.02f;

    public Animator DialogAnimator;

    public Animator MahabaliAnimator;
    public Animator MinisterAnimator;

    private bool isMahabaliAnimating;

    public AudioSource Typing;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    void Start()
    {
        MahabaliAnimator.enabled = false;
        MinisterAnimator.enabled = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void ToggleAnimations()
    {
        if (isMahabaliAnimating)
        {
            MahabaliAnimator.enabled = false;
            MinisterAnimator.enabled = true;
            isMahabaliAnimating = false;
        }
        else
        {
            MahabaliAnimator.enabled = true;
            MinisterAnimator.enabled = false;
            isMahabaliAnimating = true;
        }
    }

    public void DisplayNextDialogueLine()
    {
        dialogueArea.text = ""; // Clear the dialogue area

        ToggleAnimations();

        if (lines.Count == 0)
        {
            MahabaliAnimator.enabled = false;
            MinisterAnimator.enabled = false;

            StartCoroutine(DelayBeforeSceneLoad());
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        if (currentLine.character != null && currentLine.character.icon != null)
        {
            characterIcon.sprite = currentLine.character.icon;
            characterIcon.SetNativeSize();
        }

        characterName.text = currentLine.character.name;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.line));
    }

    private IEnumerator DelayBeforeSceneLoad()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay duration as needed

        // Check if the current active scene is "Act2"
        if (SceneManager.GetActiveScene().name == "Act2")
        {
            SceneManager.LoadScene("Act2Scene2");
        }
    }


    IEnumerator TypeSentence(string sentence)
    {
        DialogAnimator.SetTrigger("ShowTrigger");
        yield return new WaitForSeconds(1f);

        if (Typing != null)
        {
            Typing.UnPause(); // Resume audio if paused
            Typing.Play();    // Play the typing sound
        }

        yield return new WaitForSeconds(1f);

        dialogueArea.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(0.5f);

        if (Typing != null)
        {
            Typing.Pause(); // Pause the typing sound
        }

        DisplayNextDialogueLine();
        DialogAnimator.SetTrigger("HideTrigger");
    }

    void EndDialogue()
    {
        isDialogueActive = false;
    }
}
