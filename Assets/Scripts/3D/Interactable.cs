using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.Dialogue;

namespace MeatGame.ThreeD
{
    internal abstract class Interactable : MonoBehaviour
    {
        /* Script Dependencies
        PlayerMenu
        DialogueManager
        */

        public string interactText = "Interact";

        public void TriggerInteract()
        {
            if (PlayerMenu.Instance.playerMenuObject.activeSelf || DialogueManager.Instance.DialogueUI.activeSelf)
            {
                return;
            }
            TriggerInteractEffect();
        }

        protected abstract void TriggerInteractEffect();


    }
}