using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveToPositionWithDialog : MonoBehaviour
{
    public Transform player;
    public Transform oldMan;
    public Transform playerTargetPosition;
    public Transform oldManTargetPosition;
    public GameObject dialogBox1;
    public GameObject dialogBox2;
    public Text dialogBox1Text; // Text for Dialog Box 1
    public Text dialogBox2Text; // Text for Dialog Box 2
    public RectTransform dialogBox1TextRect; // RectTransform for scrolling in Dialog Box 1
    public RectTransform dialogBox2TextRect; // RectTransform for scrolling in Dialog Box 2
    public float typewriterSpeed = 0.05f; // Speed of typewriter effect
    public float scrollSpeed = 30f; // Speed of scrolling effect
    public float moveSpeed = 2f;
    public float scaleSpeed = 1f;
    public Vector3 playerTargetScale = new Vector3(2f, 2f, 2f);
    public Vector3 oldManTargetScale = new Vector3(1.5f, 1.5f, 1.5f);
    public AudioSource dialogSound; // Sound for dialog box
    public AudioSource typewritingSound; // Sound for typewriting effect

    private Animator playerAnimator;
    private Animator oldManAnimator;

    public GameObject Training;

    public FadeIn obj;
    private void Start()
    {

        obj.FadeInEffect();
        playerAnimator = player.GetComponent<Animator>();
        oldManAnimator = oldMan.GetComponent<Animator>();

        playerAnimator.SetBool("isWalking", true);
        oldManAnimator.enabled = false;

        StartCoroutine(MovePlayerThenOldMan());
        typewritingSound.enabled = false;
    }

    private IEnumerator MovePlayerThenOldMan()
    {
        // Move Player to Target Position
        while (Vector3.Distance(player.position, playerTargetPosition.position) > 0.1f ||
               player.localScale != playerTargetScale)
        {
            player.position = Vector3.MoveTowards(player.position, playerTargetPosition.position, moveSpeed * Time.deltaTime);
            player.localScale = Vector3.MoveTowards(player.localScale, playerTargetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        player.localScale = playerTargetScale;
        playerAnimator.SetBool("isWalking", false);

        yield return new WaitForSeconds(1f);

        // Show first dialog box with sound
        dialogBox1.SetActive(true);
        PlayDialogSound();

        // Typewriter effect for the first dialog
        yield return StartCoroutine(ScrollingTypewriterEffect(
            dialogBox1Text,
            dialogBox1TextRect,
            "Hello My dear Villagers."
        ));
        typewritingSound.enabled = false;

        // Wait for 2 seconds after dialogue completes
        yield return new WaitForSeconds(2f);

        // Hide first dialog box
        dialogBox1.SetActive(false);
        playerAnimator.enabled=false;

        // Activate Old Man's walk animation and move him to target
        oldManAnimator.enabled = true;
        oldManAnimator.SetBool("isWalking", true);

        while (Vector3.Distance(oldMan.position, oldManTargetPosition.position) > 0.1f ||
               oldMan.localScale != oldManTargetScale)
        {
            oldMan.position = Vector3.MoveTowards(oldMan.position, oldManTargetPosition.position, moveSpeed * Time.deltaTime);
            oldMan.localScale = Vector3.MoveTowards(oldMan.localScale, oldManTargetScale, scaleSpeed * Time.deltaTime);
            yield return null;
        }

        oldMan.localScale = oldManTargetScale;
        oldManAnimator.SetBool("isWalking", false);

        yield return new WaitForSeconds(2f);

        // Show second dialog box with sound
        dialogBox2.SetActive(true);
        PlayDialogSound();

 
        // Typewriter effect for the second dialog
        yield return StartCoroutine(ScrollingTypewriterEffect(
            dialogBox2Text,
            dialogBox2TextRect,
            "Mahabali, your grandfather Prahlada’s devotion to Vishnu and his unwavering justice inspire us all. You must carry that legacy."
        ));
        typewritingSound.enabled = false;

        yield return new WaitForSeconds(2f);

       
        // Show second dialog box with sound
        dialogBox2.SetActive(false);
        oldManAnimator.enabled = false;

        playerAnimator.enabled = true;
        dialogBox1.SetActive(true);

        yield return StartCoroutine(ScrollingTypewriterEffect(
           dialogBox1Text,
           dialogBox1TextRect,
           "I will, with all my heart."
       ));
        typewritingSound.enabled = false;

        yield return new WaitForSeconds(1f);

        playerAnimator.enabled = false;
        dialogBox1.SetActive(false);
        yield return new WaitForSeconds(1f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Training.SetActive(true);
        yield return new WaitForSeconds(2f);
        obj.FadeInEffect();
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(currentSceneIndex + 1);

    }

    private IEnumerator ScrollingTypewriterEffect(Text targetText, RectTransform textRect, string message)
    {
        typewritingSound.enabled = true;
        targetText.text = ""; // Clear previous text

        foreach (char letter in message.ToCharArray())
        {
            targetText.text += letter; // Add one character at a time
           
            // Play typewriting sound for each letter
            if (typewritingSound != null && !typewritingSound.isPlaying)
            {
                typewritingSound.Play();
            }

            // Check if the content exceeds the container's height
            float contentHeight = targetText.preferredHeight;
            float containerHeight = textRect.rect.height;

            if (contentHeight > containerHeight)
            {
                // Move the text up by modifying the anchored position of the text.
                Vector2 currentPos = textRect.anchoredPosition;
                textRect.anchoredPosition = new Vector2(currentPos.x, currentPos.y + scrollSpeed * Time.deltaTime);
            }

            yield return new WaitForSeconds(typewriterSpeed); // Delay between each character
        }
    }

    private void PlayDialogSound()
    {
        if (dialogSound != null)
        {
            dialogSound.Play();
        }
    }
}