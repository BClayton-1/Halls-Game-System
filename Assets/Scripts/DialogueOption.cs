using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MeatGame.Dialogue
{
    public class DialogueOption : MonoBehaviour
    {
        /* Script Dependencies
		DialogueManager
		*/

        public TextMeshProUGUI buttonTextUI;
        public string branchID;

        public void ChooseOption()
        {
            DialogueManager.Instance.Jump(branchID);
            GameObject.Find("DialogueBranchPanel").SetActive(false);
            DialogueManager.Instance.advanceDialogueButton.gameObject.SetActive(true);
            Debug.Log("Option " + branchID + " chosen.");
            DialogueManager.Instance.AdvanceDialogue();
        }

        void OnDisable()
        {
            Destroy(gameObject);
        }
    }
}