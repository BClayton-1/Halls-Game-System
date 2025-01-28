using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeatGame.Dialogue;

namespace MeatGame
{
    public class MainMenuButtons : MonoBehaviour
    {
        /* Script Dependencies
		DialogueManager
		TimeManager
		*/

        public LevelLoader Level_Loader;

        public int dialogueTxt = 0;
        public int dialogueNumber = 1;

        public void StartNewGame()
        {
            StartCoroutine(Level_Loader.SceneTransition("2DEnvironment"));
            TimeManager.Instance.SetDay(1);
            DialogueManager.Instance.StartDialogue(dialogueTxt, dialogueNumber);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}