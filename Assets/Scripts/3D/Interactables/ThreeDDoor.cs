using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThreeDDoor : Interactable
{
    public string roomName;
    public string doorTransform;
    public bool enterRoomTwoD = true;

    private DialogueManager Dialogue_Manager;

    // Start is called before the first frame update
    void Start()
    {
        Dialogue_Manager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
    }


    public override void TriggerInteract()
    {
        if (enterRoomTwoD == true)
        {
            StartCoroutine(Dialogue_Manager.EnterRoom(roomName));
        }
        else
        {
            StartCoroutine(Dialogue_Manager.Enter3DRoom(roomName, doorTransform));
        }
    }


}
