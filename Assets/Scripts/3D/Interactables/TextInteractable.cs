using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInteractable : Interactable
{
    public EventManager Event_Manager;

    public string objectName = "Default";


    void Start()
    {
        Event_Manager = GameObject.Find("GameManager").GetComponent<EventManager>();
    }

    public override void TriggerInteract()
    {
        Event_Manager.StartInteractDialogue(objectName);
    }
}