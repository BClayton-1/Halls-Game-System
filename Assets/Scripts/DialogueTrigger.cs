using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        /* Script Dependencies
		DialogueManager
		*/

        public int dialogueTxt = 0;
        public int dialogueNumber = 1;

        public void CallStartDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogueTxt, dialogueNumber);
            Destroy(gameObject);
        }

    }
}