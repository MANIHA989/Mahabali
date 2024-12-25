using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    void Start()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        if (SceneManager.GetActiveScene().name == "Act2")
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }

        if (SceneManager.GetActiveScene().name == "Act3")
         {
          // Ensure Act3Manager instance is initialized and target is reached
          if (Act3Manager.Instance != null && Act3Manager.Instance.ReachedTarget)
          {
              Act3Manager.Instance.StartConversation(dialogue);
          }
         }
        
    }

    
}
