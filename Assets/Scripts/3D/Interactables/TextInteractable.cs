using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame.ThreeD
{
    internal class TextInteractable : Interactable
    {
        // Script Dependencies
        [SerializeField] private EventManager EventManager;

        public string objectName = "Default";

        protected override void TriggerInteractEffect()
        {
            EventManager.StartInteractDialogue(objectName);
        }
    }
}