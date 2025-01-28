using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MeatGame.Dialogue;

namespace MeatGame.ThreeD
{
    internal class ThreeDDoor : Interactable
    {
        /* Script Dependencies
		DialogueManager
		*/

        public string roomName;
        public string doorTransform;
        public bool enterRoomTwoD = true;

        protected override void TriggerInteractEffect()
        {
            if (enterRoomTwoD == true)
            {
                StartCoroutine(DialogueManager.Instance.EnterRoom(roomName));
            }
            else
            {
                StartCoroutine(DialogueManager.Instance.Enter3DRoom(roomName, doorTransform));
            }
        }
    }
}