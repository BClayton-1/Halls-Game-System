using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.Dialogue;

namespace MeatGame.ThreeD
{
    internal class ThreeDNPC : Interactable
    {
        /* Script Dependencies
		DialogueManager
		*/

        public string NPCName = "Default";

        public int dialogueTxt = 0;
        public int dialogueNumber = 1;

        private bool interacted = false; // Gonna try and get rid of this entirely

        protected override void TriggerInteractEffect()
        {
            if (interacted == false)
            {
                interacted = true;
                DialogueManager.Instance.StartDialogue(dialogueTxt, dialogueNumber);
            }
            else
            {
                DialogueManager.Instance.StartDialogue(0, 0);
            }
        }
    }
}