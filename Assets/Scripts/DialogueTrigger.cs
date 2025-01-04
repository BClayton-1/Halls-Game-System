using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager Dialogue_Manager;
    public int dialogueTxt = 0;
    public int dialogueNumber = 1;

    public void CallStartDialogue()
    {
        Dialogue_Manager.StartDialogue(dialogueTxt,dialogueNumber);
        Destroy(gameObject);
    }

}
