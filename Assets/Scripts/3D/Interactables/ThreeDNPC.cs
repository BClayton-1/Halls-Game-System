using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDNPC : Interactable
{
    public DialogueManager Dialogue_Manager;

    public string NPCName = "Default";

    public int dialogueTxt = 0;
    public int dialogueNumber = 1;

    private bool interacted = false; // Gonna try and get rid of this entirely


    void Start()
    {
        Dialogue_Manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }

    public override void TriggerInteract()
    {
        if (interacted == false)
        {
            interacted = true;
            Dialogue_Manager.StartDialogue(dialogueTxt, dialogueNumber);
        }
        else
        {
            Dialogue_Manager.StartDialogue(0, 0);
        }
    }
}
