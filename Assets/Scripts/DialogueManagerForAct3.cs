using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ConversationManager : MonoBehaviour
{
    public static ConversationManager Instance;

    public Image speakerIcon;
    public TextMeshProUGUI speakerName;
    public TextMeshProUGUI conversationArea;

    private Queue<DialogueLine> dialogueQueue;

    public bool isConversationActive = false;
    public float textSpeed = 0.02f;

    public Animator ConversationAnimator;

    public Animator MainCharacterAnimator;
    public Animator SupportingCharacterAnimator;

    private bool isMainCharacterAnimating;

    public AudioSource TypingSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        dialogueQueue = new Queue<DialogueLine>();
    }

    void Start()
    {
        MainCharacterAnimator.enabled = false;
        SupportingCharacterAnimator.enabled = false;
    }

    public void BeginConversation(Dialogue dialogue)
    {
        isConversationActive = true;
        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            dialogueQueue.Enqueue(line);
        }

        ShowNextDialogueLine();
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

    private IEnumerator WaitBeforeSceneChange()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay duration as needed

        // Check if the current active scene is "Act2"
        if (SceneManager.GetActiveScene().name == "Act2")
        {
            SceneManager.LoadScene("Act2Scene2");
        }
    }

    IEnumerator ShowTextLine(string sentence)
    {
        ConversationAnimator.SetTrigger("ShowTrigger");
        yield return new WaitForSeconds(1f);

        if (TypingSound != null)
        {
            TypingSound.UnPause(); // Resume audio if paused
            TypingSound.Play();    // Play the typing sound
        }

        yield return new WaitForSeconds(1f);

        conversationArea.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            conversationArea.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(0.5f);

        if (TypingSound != null)
        {
            TypingSound.Pause(); // Pause the typing sound
        }

        ShowNextDialogueLine();
        ConversationAnimator.SetTrigger("HideTrigger");
    }

    void EndConversation()
    {
        isConversationActive = false;
    }
}
