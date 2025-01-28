using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MeatGame.ThreeD
{
    internal abstract class Interactable : MonoBehaviour
    {
        /* Script Dependencies
        PlayerMenu
        */

        public string interactText = "Interact";

        public void TriggerInteract()
        {
            if (PlayerMenu.Instance.playerMenuObject.activeSelf)
            {
                return;
            }
            TriggerInteractEffect();
        }

        protected abstract void TriggerInteractEffect();


    }
}