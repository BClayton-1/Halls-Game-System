using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFreeze : MonoBehaviour
{
    [SerializeField] private MouseLook Mouse_Look;
    [SerializeField] private PlayerMovement Player_Movement;

    private GameObject DialogueManager;
    private GameObject PlayerMenu;

    private GameObject playerMenu_Main;
    private GameObject playerMenu_Inventory;
    private GameObject dialogueMenu;

    void Start()
    {
        DialogueManager = GameObject.Find("DialogueManager");
        PlayerMenu = GameObject.Find("PlayerMenu");

        playerMenu_Main = PlayerMenu.transform.GetChild(0).gameObject;
        playerMenu_Inventory = PlayerMenu.transform.GetChild(1).gameObject;
        dialogueMenu = DialogueManager.transform.GetChild(0).gameObject;

        CheckFreezeMenu();
    }

    public void CheckFreezeMenu()
    {
        if (playerMenu_Main.activeSelf == true || playerMenu_Inventory.activeSelf == true || dialogueMenu.activeSelf == true)
        {
            Mouse_Look.enabled = false;
            Player_Movement.enabled = false;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Mouse_Look.enabled = true;
            Player_Movement.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
