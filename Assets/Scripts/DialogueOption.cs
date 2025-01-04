using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueOption : MonoBehaviour
{
    public DialogueManager Dialogue_Manager;
    public TextMeshProUGUI buttonTextUI;
    public string branchID;

    void Start()
    {
        Dialogue_Manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    public void ChooseOption()
    {
        Dialogue_Manager.Jump(branchID);
        GameObject.Find("DialogueBranchPanel").SetActive(false);
        Dialogue_Manager.advanceDialogueButton.gameObject.SetActive(true);
        Debug.Log("Option " + branchID + " chosen.");
        Dialogue_Manager.AdvanceDialogue();
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }
    
}
