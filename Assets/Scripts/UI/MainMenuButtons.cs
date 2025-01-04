using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public DialogueManager Dialogue_Manager;
    public LevelLoader Level_Loader;
    public TimeManager Time_Manager;


    public int dialogueTxt = 0;
    public int dialogueNumber = 1;

    public void StartNewGame()
    {
        StartCoroutine(Level_Loader.SceneTransition("2DEnvironment"));
        Time_Manager.day = 1;
        Dialogue_Manager.StartDialogue(dialogueTxt, dialogueNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
