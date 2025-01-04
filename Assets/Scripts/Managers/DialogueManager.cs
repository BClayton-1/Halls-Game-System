using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{

	public SaveData Save_Data;
	public EventManager Event_Manager;
	public ConditionChecker Condition_Checker;
	public TimeManager Time_Manager;
	public InventoryUIManager Inventory_UI_Manager;
	public PlayerManager Player_Manager;
	public AudioManager Audio_Manager;
	[SerializeField] private StatusText Status_Text;

	public TextMeshProUGUI dialogueTextUI;
	public TextMeshProUGUI nameTextUI;
	public TextAsset activeDialogueTextAsset;
	private string dialogueTextWhole;
	private Queue<string> sentences;
	private string sentence;
	public GameObject DialogueUI;
	public GameObject DialogueBox;
	public GameObject BranchButtonPrefab;
	public GameObject DialogueBranchPanel;
	private bool newBranch = false;
	public Button advanceDialogueButton;
	private bool sentenceIsTyping = false;
	private Coroutine typeSentenceCoroutine;

	public bool dialoguePanelOpen = true;

	int dialogueFileID;
	int dialogueID;
	private string activeChar;
	public GameObject Char_Left;
	public GameObject Char_Right;
	public GameObject Char_Center;
	public GameObject DialogueImages;

	public Animator Transition_Animator;

	public AudioSource gameAudio;
	[SerializeField] private AudioSource effectAudio;

	private AudioClip typeSound;

	public List<string> cooldownList = new List<string>();



	// Start is called before the first frame update
	void Start()
    {
		sentences = new Queue<string>();
		typeSound = Resources.Load<AudioClip>("Sounds/UI/Typesound");
	}

	public void StartDialogue(int _dialogueFileID, int _dialogueID)
	{
		TextMeshProUGUI interactTextUI = GameObject.Find("InteractText").GetComponent<TextMeshProUGUI>();
		interactTextUI.text = string.Empty;
		Cursor.lockState = CursorLockMode.None;
		DialogueUI.SetActive(true);
		dialogueFileID = _dialogueFileID;
		dialogueID = _dialogueID;
		Player_Manager.CheckMenuFreezePM();
		string dialogueFileIDString = dialogueFileID.ToString();
		string dialogueIDString = dialogueID.ToString();
		string dialogueFullIDString = dialogueFileIDString + "-" + dialogueIDString;
		if (Save_Data.ViewedDialogue.Contains(dialogueFullIDString) == false) // Add this dialogue to the list of viewed dialogues
		{
			Save_Data.ViewedDialogue.Add(dialogueFullIDString);
			Debug.Log(dialogueFullIDString + " viewed.");
		}
		advanceDialogueButton.gameObject.SetActive(true);
		activeDialogueTextAsset = Resources.Load<TextAsset>("Dialogue/dialogue_" + dialogueFileIDString); // Load text file
		dialogueTextWhole = activeDialogueTextAsset.text;
		dialogueTextWhole = dialogueTextWhole.Substring(dialogueTextWhole.IndexOf("DIALOGUE = " + dialogueIDString) + ("DIALOGUE = ".Length + dialogueIDString.Length)); // Isolate a single dialogue
		if (dialogueTextWhole.IndexOf("DIALOGUE =" ) > 0)
		{
			dialogueTextWhole = dialogueTextWhole.Substring(0, dialogueTextWhole.IndexOf("DIALOGUE ="));
		}

		string[] lines = dialogueTextWhole.Split(System.Environment.NewLine.ToCharArray());

		bool dialogueStarted = false;

		foreach (string line in lines) // for every line of dialogue
		{
			if (line.StartsWith("START_DIALOGUE"))
			{
				dialogueStarted = true;
			}
			if (dialogueStarted == true && !string.IsNullOrEmpty(line) && !line.StartsWith("//")) // ignore empty lines of dialogue and comments
			{
				// making a new substring of all content before comments in a given line
				string lineNoComments = line;
				if (line.IndexOf("//") > 0)
				{
					lineNoComments = line.Substring(0, line.IndexOf("//"));
				}
				sentences.Enqueue(lineNoComments); // adds to the dialogue to be printed
			}
		}
		dialogueStarted = false;
		AdvanceDialogue();
	}

	public void AdvanceDialogue()
	{
		// option to interrupt dialogue and go to next sentence (advance dialogue by clicking basically)
		if (sentenceIsTyping == true)
        {
			StopCoroutine(typeSentenceCoroutine);
			sentenceIsTyping = false;
			dialogueTextUI.text = sentence;
			if (sentences.Count > 0 && sentences.Peek().StartsWith("NEWBRANCH"))
            {
				AdvanceDialogue();
            }
			return;
        }
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		sentence = sentences.Dequeue();

		ParseSentence(sentence);
	}

	IEnumerator TypeSentence(string sentence) // Types out the sentence character by character
	{
		bool richTextCommand = false;
		sentenceIsTyping = true;
		dialogueTextUI.text = "";
		char[] sentenceAsChars = sentence.ToCharArray();
		foreach (char letter in sentenceAsChars)
		{
			if (letter == '>')
            {
				richTextCommand = false;
				dialogueTextUI.text += letter;
				continue;

			}
			if (richTextCommand == true || letter == '<')
            {
				richTextCommand = true;
				dialogueTextUI.text += letter;
				continue;
			}
			yield return new WaitForSeconds(0.04f);
			dialogueTextUI.text += letter;
			gameAudio.PlayOneShot(typeSound, 0.5f);
			yield return null;
		}
		sentenceIsTyping = false;
		if (sentences.Count != 0 && sentences.Peek().StartsWith("NEWBRANCH"))
		{
			AdvanceDialogue();
		}
	}

	public void ParseSentence(string sentence) // Responsible for interpreting text files
    {
		if (sentence.StartsWith("NEWBRANCH"))
        {
			newBranch = true;
			DialogueBranchPanel.SetActive(true);
			advanceDialogueButton.gameObject.SetActive(false);
			//Debug.Log("Starting New Branch.");
			AdvanceDialogue();
        }
		else if (newBranch == true)
        {
			if (sentence.StartsWith("BRANCH"))
			{
				newBranch = false; // End branch
				//Debug.Log("Branch Completed.");
				return;
			}
			string branchID = "";
			string buttonText = "";
			if (sentence.IndexOf(":") > 0)
			{
				branchID = sentence.Substring(0, sentence.IndexOf(":")); // Get branch ID e.g "A" in "A: Hello"
				buttonText = sentence.Substring(sentence.IndexOf(": ") + 2); // Get button text e.g "Hello" in "A: Hello"
			}
			if (buttonText.IndexOf(" | ") > 0) // Do not display conditions in button text
            {
				buttonText = buttonText.Substring(0, buttonText.IndexOf(" | "));
            }

			string optionDetails = sentence.Substring(sentence.IndexOf(" | ") + " | ".Length);
			string isolatedConditions;
			GameObject OptionButton;
			if (sentence.IndexOf("OPTIONCONDITIONS") > 0)
			{
				isolatedConditions = optionDetails.Substring(optionDetails.IndexOf("OPTIONCONDITIONS") + "OPTIONCONDITIONS".Length);
				isolatedConditions = isolatedConditions.Substring(0, isolatedConditions.LastIndexOf(")") + 1);
				//Debug.Log(isolatedConditions);
				bool conditionsTrue = Condition_Checker.CheckConditions(isolatedConditions);
				if (!(conditionsTrue != true && sentence.Contains("[H]")))
				{
					OptionButton = Instantiate(BranchButtonPrefab, DialogueBranchPanel.transform);
					OptionButton.GetComponent<DialogueOption>().buttonTextUI.text = buttonText;
					OptionButton.GetComponent<DialogueOption>().branchID = branchID;
				}
			}
            else
            {
				OptionButton = Instantiate(BranchButtonPrefab, DialogueBranchPanel.transform);
				OptionButton.GetComponent<DialogueOption>().buttonTextUI.text = buttonText;
				OptionButton.GetComponent<DialogueOption>().branchID = branchID;
			}
			

			//Debug.Log("Creating button for option " + branchID);
			AdvanceDialogue();
        }
		else if (sentence.StartsWith("END_DIALOGUE"))
        {
			EndDialogue();
        }
		else if (sentence.StartsWith("CHAR = ")) // e.g CHAR = Player
		{
			activeChar = sentence.Substring(sentence.IndexOf("CHAR = ")+7); // activeChar == "Player"
			nameTextUI.text = activeChar;
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("ENTERROOM")) // e.g ENTERROOM(Home) or ENTERROOM(Home,Fadeblack_Start,Fadeblack_End)
		{
			string room = sentence.Substring(sentence.IndexOf("(") + 1);
			string transitionStart = "Fadeblack_Start";
			string transitionEnd = "Fadeblack_End";
			if (room.IndexOf(",") > 0)
            {
				room = room.Replace(" ", string.Empty);

				transitionStart = room.Substring(room.IndexOf(",") + 1);
				transitionStart = transitionStart.Substring(0, transitionStart.LastIndexOf(","));

				transitionEnd = room.Substring(room.LastIndexOf(",") + 1);
				transitionEnd = transitionEnd.Substring(0, transitionEnd.IndexOf(")"));

				room = room.Substring(0, room.IndexOf(","));
            }
            else
            {
				room = room.Substring(0, room.IndexOf(")"));
			}
			StartCoroutine(EnterRoom(room, transitionStart, transitionEnd));
        }
		else if (sentence.StartsWith("ENTER3DROOM")) // e.g ENTER3DROOM(Courtyard,DoorTransform_Crossroads) or ENTER3DROOM(Courtyard,DoorTransform_Crossroads,Fadeblack_Start,Fadeblack_End)
		{
			sentence = sentence.Replace(" ", string.Empty);
			string room = sentence.Substring(sentence.IndexOf("(") + 1);
			room = room.Substring(0, room.IndexOf(","));
			sentence = sentence.Substring(sentence.IndexOf(",") + 1);
			string doorTransform = sentence;
			if (doorTransform.IndexOf(",") > 0)
            {
				doorTransform = doorTransform.Substring(0, doorTransform.IndexOf(","));
            }
            else
            {
				doorTransform = doorTransform.Substring(0, doorTransform.IndexOf(")"));
			}
			string transitionStart = "Fadeblack_Start";
			string transitionEnd = "Fadeblack_End";
			if (sentence.IndexOf(",") > 0)
			{
				transitionStart = sentence.Substring(sentence.IndexOf(",") + 1);
				transitionStart = transitionStart.Substring(0, transitionStart.LastIndexOf(","));

				transitionEnd = sentence.Substring(sentence.LastIndexOf(",") + 1);
				transitionEnd = transitionEnd.Substring(0, transitionEnd.IndexOf(")"));
			}

			StartCoroutine(Enter3DRoom(room, doorTransform, transitionStart, transitionEnd));
		}
		else if (sentence.StartsWith("JUMP ")) // e.g JUMP A
        {
			sentence = sentence.Substring("JUMP ".Length);
			if (sentence.IndexOf("	") > 0)
            {
				sentence = sentence.Substring(0, sentence.IndexOf("	"));
            }
			if (sentence.IndexOf(" ") > 0)
			{
				sentence = sentence.Substring(0, sentence.IndexOf(" "));

			}
			Jump(sentence);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("BACKGROUND_MUSIC =")) // Sets the background music
        {
			if (sentence.StartsWith("BACKGROUND_MUSIC = NONE") || sentence.StartsWith("BACKGROUND_MUSIC = None") || sentence.StartsWith("BACKGROUND_MUSIC = none"))
            {
				gameAudio.clip = null;
				gameAudio.Stop();
            }
			else
            {
				string clipA = sentence.Substring("BACKGROUND_MUSIC = ".Length);
				if (clipA.IndexOf("	") > 0)
				{
					clipA = clipA.Substring(0, clipA.IndexOf("	"));
				}
				if (clipA.IndexOf(" ") > 0)
				{
					clipA = clipA.Substring(0, clipA.IndexOf(" "));

				}

				Audio_Manager.PlayMusic(clipA);
			}
			AdvanceDialogue();
        }
		else if (sentence.StartsWith("PLAYSOUND")) // e.g PLAYSOUND(Damage_Sound)
        {
			string soundName = sentence.Substring("PLAYSOUND(".Length);
			soundName = soundName.Substring(0, soundName.IndexOf(")"));
			AudioClip audioClip = Resources.Load<AudioClip>("Sounds/SFX/" + soundName);
			effectAudio.PlayOneShot(audioClip, 1f);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("LEFT") || sentence.StartsWith("RIGHT") || sentence.StartsWith("CENTER")) // Set character image in the given position
        {
			string imgPos = sentence.Substring(0,sentence.IndexOf(" ="));
			string spriteS = sentence.Substring(sentence.IndexOf(" =") + " = ".Length);
			if (spriteS.IndexOf("	") > 0)
			{
				spriteS = spriteS.Substring(0, spriteS.IndexOf("	"));
			}
			if (spriteS.IndexOf(" ") > 0)
			{
				spriteS = spriteS.Substring(0, spriteS.IndexOf(" "));

			}
			Sprite newSprite = null;
			if (spriteS != "None" && Resources.Load<Sprite>("Images/Portrait/" + spriteS) != null)
			{
				newSprite = Resources.Load<Sprite>("Images/Portrait/" + spriteS);
			}
			SetImage(imgPos,newSprite);
			AdvanceDialogue();
        }
		else if (sentence.StartsWith("CUTLEFT") || sentence.StartsWith("CUTRIGHT") || sentence.StartsWith("CUTCENTER")) // Set character image in the given position without a fade transition
		{
			string imgPos = sentence.Substring(0, sentence.IndexOf(" ="));
			string spriteS = sentence.Substring(sentence.IndexOf(" =") + " = ".Length);
			if (spriteS.IndexOf("	") > 0)
			{
				spriteS = spriteS.Substring(0, spriteS.IndexOf("	"));
			}
			if (spriteS.IndexOf(" ") > 0)
			{
				spriteS = spriteS.Substring(0, spriteS.IndexOf(" "));

			}
			Sprite newSprite = null;
			if (spriteS != "None" && Resources.Load<Sprite>("Images/Portrait/" + spriteS) != null)
			{
				newSprite = Resources.Load<Sprite>("Images/Portrait/" + spriteS);
			}

			Image origImage;

			switch (imgPos)
			{
				case "CUTLEFT":
					origImage = Char_Left.GetComponent<Image>();
					origImage.sprite = newSprite;
					break;

				case "CUTRIGHT":
					origImage = Char_Right.GetComponent<Image>();
					origImage.sprite = newSprite;
					break;

				case "CUTCENTER":
					origImage = Char_Center.GetComponent<Image>();
					origImage.sprite = newSprite;
					break;

				default:
					break;
			}

			AdvanceDialogue();
		}
		else if (sentence.StartsWith("BACKGROUND =")) // Changes background
        {
			string spriteS = sentence.Substring(sentence.IndexOf(" =") + " = ".Length);
			if (spriteS.IndexOf("	") > 0)
			{
				spriteS = spriteS.Substring(0, spriteS.IndexOf("	"));
			}
			if (spriteS.IndexOf(" ") > 0)
			{
				spriteS = spriteS.Substring(0, spriteS.IndexOf(" "));

			}
			Sprite newSprite = null;
			if (spriteS != "None" && Resources.Load<Sprite>("Images/Background/" + spriteS) != null)
			{
				newSprite = Resources.Load<Sprite>("Images/Background/" + spriteS);
			}
			SetBackground(newSprite);
			AdvanceDialogue();

		}
		else if (sentence.StartsWith("WAIT"))
        {
			float waitTime = 1f;

			sentence = sentence.Substring("WAIT(".Length);
			sentence = sentence.Substring(0, sentence.IndexOf(")"));
			float.TryParse(sentence, out waitTime);
			StartCoroutine(DialogueWait(waitTime));
        }
		else if (sentence.StartsWith("ROLL(")) // e.g ROLL(Fortitude,50,A,B) (Roll addition, Difficulty (x% failure chance), fail branch, partial fail branch (optional)) ROLL(!Tenacity,50,A)
		{
			int rollAddition = 0; // +0
			int rollDifficulty = 50; // 50% chance
			bool partialBranchTrue = false;

			sentence = sentence.Substring("ROLL(".Length);
			string rollAdditionString = sentence.Substring(0, sentence.IndexOf(","));

			bool rollSubtraction = false;
			if (rollAdditionString.StartsWith("!"))
            {
				rollAdditionString = rollAdditionString.Substring(1);
				rollSubtraction = true;
			}

            switch (rollAdditionString)
            {
				case "Tenacity":
					rollAddition = Player_Manager.tenacityStat;
					break;
				case "NatTenacity":
					rollAddition = Player_Manager.naturalTenacity;
					break;
				case "Cognition":
					rollAddition = Player_Manager.cognitionStat;
					break;
				case "NatCognition":
					rollAddition = Player_Manager.naturalCognition;
					break;
				case "Influence":
					rollAddition = Player_Manager.influenceStat;
					break;
				case "NatInfluence":
					rollAddition = Player_Manager.naturalInfluence;
					break;
				case "Luck":
					rollAddition = Player_Manager.luckStat;
					break;
				case "NatLuck":
					rollAddition = Player_Manager.naturalLuck;
					break;
				case "Composure":
					rollAddition = Player_Manager.composure;
					break;
				case "Sanity":
					rollAddition = Player_Manager.sanity;
					break;
				case "Healthiness":
					rollAddition = (int)(Player_Manager.healthiness * 100);
					break;
				case "Unhealthiness":
					rollAddition = (int)(100 - (Player_Manager.healthiness * 100));
					break;
				default:
					if (rollAdditionString.StartsWith("ITEM["))
					{
						string itemID = rollAdditionString.Substring("Item[".Length);
						itemID = itemID.Substring(0, itemID.IndexOf("]"));
						rollAddition = Inventory_UI_Manager.Inventory.GetTotalQuantity(itemID);
						rollAdditionString = null;
					}
                    else
                    {
						int.TryParse(rollAdditionString, out rollAddition);
						rollAdditionString = null;

					}
					break;

			}

			if (rollSubtraction == true)
            {
				rollAddition = rollAddition * -1;
			}

			sentence = sentence.Substring(sentence.IndexOf(",") + 1);
			int.TryParse(sentence.Substring(0, sentence.IndexOf(",")), out rollDifficulty);
			sentence = sentence.Substring(sentence.IndexOf(",") + 1);

			string failBranch = "";
			string partialBranch = "";

			if (sentence.IndexOf(",") > 0)
            {
				partialBranchTrue = true;
				failBranch = sentence.Substring(0, sentence.IndexOf(","));
				sentence = sentence.Substring(sentence.IndexOf(",") + 1);
				partialBranch = sentence.Substring(0, sentence.IndexOf(")"));
			}
            else
            {
				failBranch = sentence.Substring(0, sentence.IndexOf(")"));
			}

			int rollValue = UnityEngine.Random.Range(1, 100) + rollAddition;

			Debug.Log("Rolled " + rollValue + "(" + rollAddition + "). Need " + rollDifficulty);

			if (rollAdditionString != null)
            {
				rollAdditionString = rollAdditionString + " roll";
			}
            else
            {
				rollAdditionString = "roll";
			}

			if (rollValue >= rollDifficulty)
            {
				Status_Text.DisplayStatus("<color=green>Succeeded " + rollAdditionString);
				AdvanceDialogue(); // Success
            }
			else if (partialBranchTrue == true && (rollValue + 5) >= rollDifficulty)
            {
				Status_Text.DisplayStatus("<color=orange>Partially succeeded " + rollAdditionString);
				Jump(partialBranch); // Partial success
				AdvanceDialogue();
			}
            else
            {
				Status_Text.DisplayStatus("<color=red>Failed " + rollAdditionString);
				Jump(failBranch); // Failure
				AdvanceDialogue();
			}

        }
		else if (sentence.StartsWith("DISPLAYSTATUS"))
        {
			sentence = sentence.Substring("DISPLAYSTATUS(".Length);
			sentence = sentence.Substring(0, sentence.IndexOf(")"));
			Status_Text.DisplayStatus(sentence);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("REALTIME"))
        {
			bool isRealtime;
			sentence = sentence.Substring("REALTIME(".Length);
			sentence = sentence.Substring(0, sentence.IndexOf(")"));
			bool.TryParse(sentence, out isRealtime);
			Time_Manager.realTimeActive = isRealtime;
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("ADDTIME")) // e.g ADDTIME(Hour,2)
		{
			sentence = sentence.Replace(" ", string.Empty);
			string unit = sentence.Substring("ADDTIME(".Length);
			unit = unit.Substring(0, unit.IndexOf(","));
			sentence = sentence.Substring(sentence.IndexOf(",") + 1);
			sentence = sentence.Substring(0,sentence.IndexOf(")"));
			int amount = 0;
			int.TryParse(sentence, out amount);

			Time_Manager.AddTime(unit,amount);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("SETTIME")) // e.g SETTIME(1430) for 2:30
        {
			string timeString = sentence.Substring("SETTIME(".Length);
			timeString = timeString.Substring(0, timeString.IndexOf(")"));
			Time_Manager.SetTime(timeString);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("ADDEQUIPMENT")) // e.g ADDEQUIPMENT(Stinky_Cheese)
		{
			string itemID = sentence.Substring(sentence.IndexOf("(") + 1);
			itemID = itemID.Substring(0, itemID.IndexOf(")"));
			Inventory_UI_Manager.AddEquipment(itemID);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("ADDITEM")) // e.g ADDITEM(Bones,1)
		{
			sentence = sentence.Replace(" ", string.Empty);
			string itemID = sentence.Substring(sentence.IndexOf("(") + 1);
			itemID = itemID.Substring(0, itemID.IndexOf(","));
			int quantity = 1;
			sentence = sentence.Substring(sentence.IndexOf(",") + 1);
			sentence = sentence.Substring(0, sentence.IndexOf(")"));
			int.TryParse(sentence, out quantity);
			Inventory_UI_Manager.AddItem(itemID,quantity);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("REMOVEITEM")) // e.g REMOVEITEM(Bones,1)
		{
			sentence = sentence.Replace(" ", string.Empty);
			string itemID = sentence.Substring(sentence.IndexOf("(") + 1);
			itemID = itemID.Substring(0, itemID.IndexOf(","));
			int quantity = 1;
			sentence = sentence.Substring(sentence.IndexOf(",") + 1);
			sentence = sentence.Substring(0, sentence.IndexOf(")"));
			int.TryParse(sentence, out quantity);
			Inventory_UI_Manager.RemoveItem(itemID, quantity);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("TAKEDAMAGE")) // e.g TAKEDAMAGE(Temporary,15)
        {
			sentence = sentence.Replace(" ", string.Empty);
			string damageType = sentence.Substring("TAKEDAMAGE(".Length);
			damageType = damageType.Substring(0, damageType.IndexOf(","));
			int damageAmount = 0;
			sentence = sentence.Substring(sentence.IndexOf(",") + 1);
			int.TryParse(sentence.Substring(0, sentence.IndexOf(")")), out damageAmount);
			Player_Manager.TakeDamage(damageType, damageAmount);
			AdvanceDialogue();
		}
		else if (sentence.StartsWith("CONDITIONBRANCH")) // e.g CONDITIONBRANCH((DOW == Friday),B)
		{
			string conditions = sentence.Substring("CONDITIONBRANCH(".Length);
			conditions = conditions.Substring(0, conditions.LastIndexOf(","));
			string branchName = sentence.Substring(sentence.LastIndexOf(",") + 1);
			branchName = branchName.Substring(0, branchName.IndexOf(")"));
			branchName = branchName.Replace(" ", string.Empty);
			Debug.Log(conditions);
			Debug.Log("Branch ID: '" + branchName + "'");
			if (Condition_Checker.CheckConditions(conditions) == true)
            {
				AdvanceDialogue();
            }
			else
            {
				Jump(branchName);
				AdvanceDialogue();
            }
        }
		else if (sentence.StartsWith("BRANCH") || sentence.StartsWith("START_DIALOGUE")) // Do not type these lines
        {
			AdvanceDialogue();
        }
		else
		{
			typeSentenceCoroutine = StartCoroutine(TypeSentence(sentence));
		}
    }

	public void EndDialogue()
	{
		DialogueUI.SetActive(false);
		Time_Manager.realTimeActive = false;
		Player_Manager.CheckMenuFreezePM();
		Debug.Log("Dialogue has ended.");
		sentences.Clear();
		gameAudio.clip = null;
		gameAudio.Stop();
		SetImage("LEFT", null);
		SetImage("RIGHT", null);
		SetImage("CENTER", null);
		SetBackground(null);
		CheckCooldowns();

		string cooldownID = "";
		if (dialogueTextWhole.Contains("ROOM = "))
        {
			string roomName = dialogueTextWhole.Substring(dialogueTextWhole.IndexOf("ROOM = ") + "ROOM = ".Length);
			roomName = roomName.Substring(0, roomName.IndexOf(System.Environment.NewLine));
			roomName = roomName.Replace(" ", string.Empty);
			roomName = roomName.Replace("	", string.Empty);
			for (int i = cooldownList.Count - 1; i >= 0; i--)
            {
				if (cooldownList[i].StartsWith(roomName))
                {
					return;
				}
            }
			cooldownID = (roomName + "|" + Time_Manager.day.ToString() + (Time_Manager.hour + 3).ToString("00") + Time_Manager.minute.ToString("00"));
			cooldownList.Add(cooldownID);
			//Debug.Log(cooldownID);
		}
		if (dialogueTextWhole.Contains("CHARACTER = "))
		{
			string dialogueTextCut = dialogueTextWhole.Substring(dialogueTextWhole.IndexOf("CHARACTER = "));
			dialogueTextCut = dialogueTextCut.Substring(0, dialogueTextCut.IndexOf("START_DIALOGUE"));
			string[] lines = dialogueTextCut.Split(System.Environment.NewLine.ToCharArray());

			foreach (string line in lines) // for every line of dialogue
			{

				if (line.StartsWith("CHARACTER = "))
				{
					string characterName = line.Substring("CHARACTER = ".Length);
					if (line.IndexOf("//") > 0)
					{
						characterName = characterName.Substring(0, characterName.IndexOf("//"));
					}
					characterName = characterName.Replace(" ", string.Empty);
					characterName = characterName.Replace("	", string.Empty);
					cooldownID = (characterName + "|" + Time_Manager.day.ToString() + (Time_Manager.hour + 3).ToString("00") + Time_Manager.minute.ToString("00"));
					for (int i = cooldownList.Count - 1; i >= 0; i--)
					{
						if (cooldownList[i].StartsWith(characterName))
						{
							return;
						}
					}
					cooldownList.Add(cooldownID);
					//Debug.Log(cooldownID);

				}
			}
		}

	}

	public void CheckCooldowns()
    {
		string currentTimeID = (Time_Manager.day.ToString() + Time_Manager.hour.ToString("00") + Time_Manager.minute.ToString("00"));
		NextCooldown:
		if (cooldownList.Count > 0)
        {
			string cooldownIDA = cooldownList[0].Substring(cooldownList[0].IndexOf("|") + 1);
			int currentTime;
			int cooldownTime;
			int.TryParse(cooldownIDA, out cooldownTime);
			int.TryParse(currentTimeID, out currentTime);
			if (currentTime >= cooldownTime)
            {
				cooldownList.Remove(cooldownList[0]);
				goto NextCooldown;
            }
		}
    }

	public void Jump(string x)
    {
		if (Save_Data.ViewedDialogue.Contains(dialogueFileID.ToString() + "-" + dialogueID.ToString() + "-" + x) == false) // Add this dialogue branch to the list of viewed dialogues
        {
			Save_Data.ViewedDialogue.Add(dialogueFileID.ToString() + "-" + dialogueID.ToString() + "-" + x);
			//Debug.Log(dialogueFileID.ToString() + "-" + dialogueID.ToString() + "-" + x + " viewed.");
		}
		sentences.Clear();
		string dialogueTextCut = dialogueTextWhole;

		string[] lines = dialogueTextCut.Split(System.Environment.NewLine.ToCharArray());
		bool dialogueHasStarted = false;
		foreach (string line in lines) // For every line of dialogue
		{
			if (!string.IsNullOrEmpty(line) && !line.StartsWith("//")) // Ignore empty lines of dialogue and comments
			{
				string line0 = line;
				if (line.IndexOf("//") > 0)
				{
					line0 = line.Substring(0, line.IndexOf("//"));
				}
				if (line0.StartsWith("BRANCH " + x))
                {
					dialogueHasStarted = true;

				}
				if (dialogueHasStarted == true)
				{
					sentences.Enqueue(line0); // Adds to the dialogue to be printed
				}
			}
		}
	}

	public void SetImage(string imgPos, Sprite newSprite)
    {
		Image origImage = Char_Center.GetComponent<Image>();
		Image tempImage = Char_Center.transform.GetChild(0).gameObject.GetComponent<Image>();
		switch (imgPos)
        {
			case "LEFT":
				origImage = Char_Left.GetComponent<Image>();
				tempImage = Char_Left.transform.GetChild(0).gameObject.GetComponent<Image>();
				break;

			case "RIGHT":
				origImage = Char_Right.GetComponent<Image>();
				tempImage = Char_Right.transform.GetChild(0).gameObject.GetComponent<Image>();
				break;

			case "CENTER":
				origImage = Char_Center.GetComponent<Image>();
				tempImage = Char_Center.transform.GetChild(0).gameObject.GetComponent<Image>();
				break;

			default:
				break;
		}

		tempImage.sprite = newSprite;
		StartCoroutine(FadeOut(origImage, newSprite));
		StartCoroutine(FadeIn(tempImage));
	}

	public void SetBackground(Sprite newSprite)
    {
		Image bgImage = DialogueImages.GetComponent<Image>();
		Image bgImageTemp = GameObject.Find("Temp_Background").GetComponent<Image>();
		bgImageTemp.sprite = bgImage.sprite;
		bgImage.sprite = newSprite;
		StartCoroutine(FadeOut(bgImageTemp, null));
		if (newSprite != null)
        {
			Color temp = bgImage.color;
			temp.a = 1.0f;
			bgImage.color = temp;
        }
		else
        {
			Color temp = bgImage.color;
			temp.a = 0.0f;
			bgImage.color = temp;
		}

	}

	IEnumerator DialogueWait(float waitTime)
    {
		advanceDialogueButton.gameObject.SetActive(false);
		DialogueUI.SetActive(false);
		yield return new WaitForSeconds(waitTime);
		DialogueUI.SetActive(true);
		advanceDialogueButton.gameObject.SetActive(true);
		AdvanceDialogue();
	}

	IEnumerator FadeIn(Image i)
    {
		Color temp = i.color;
		for (float a = 0.0f; a <= 1; a += 0.05f)
        {
			temp.a = a;
			i.color = temp;
			yield return new WaitForSeconds(0.03f);
        }
		temp.a = 0.0f;
		i.color = temp;
		i.sprite = null;
    }

	IEnumerator FadeOut(Image i, Sprite newSprite)
    {
		Color temp = i.color;
		if (i.sprite == null)
        {
			goto SkipFadeout;
        }
		for (float a = 1.0f; a >= 0; a -= 0.05f)
		{
			temp.a = a;
			i.color = temp;
			yield return new WaitForSeconds(0.03f);
		}
		SkipFadeout:
		i.sprite = newSprite;
		if (newSprite == null)
        {
			temp.a = 0.0f;
        }
        else
        {
			temp.a = 1.0f;
		}
		i.color = temp;
	}



	public IEnumerator EnterRoom(string room, string startTransition = "Fadeblack_Start", string endTransition = "Fadeblack_End") // Play a 2d dialogue event whose conditions are met with a given room id
	{
		if (startTransition == null)
        {
			startTransition = "Fadeblack_Start";
		}
		if (endTransition == null)
		{
			endTransition = "Fadeblack_End";
		}
		Transition_Animator.Play(startTransition);
		EndDialogue();
		yield return new WaitForSeconds(Transition_Animator.GetCurrentAnimatorStateInfo(0).length);
		EnterRoomThenTransitionEnd(room, endTransition);
		yield return null;

	}

	public void EnterRoomThenTransitionEnd(string room, string endTransition = "Fadeblack_End")
	{
		EndDialogue();
		if (SceneManager.GetActiveScene().name != "2DEnvironment")
        {
			SceneManager.LoadScene("2DEnvironment");
		}
		Debug.Log("Entering room: " + room);
		List<string> idAndPriority = new List<string>();
		List<string> idAndPriorityB = new List<string>();

		int dF_ID = 0;
		int d_ID = 0;
		int highestPriority = 6;
		var dialogueTextAssets = Resources.LoadAll("Dialogue", typeof(TextAsset)).Cast<TextAsset>().ToArray();
		foreach (TextAsset dialogueTextAsset in dialogueTextAssets)
        {
			int.TryParse(dialogueTextAsset.name.Substring("dialogue_".Length), out dF_ID);
			string textFileContents = dialogueTextAsset.text;
			string[] dialogueEvents = textFileContents.Split(new string[] { "DIALOGUE = " }, StringSplitOptions.None);
			foreach (string dialogueEvent in dialogueEvents)
            {
				goto StartHere;
			ContinueLoop:
				continue;
			StartHere:

				if (dialogueEvent.Contains("ROOM = " + room))
                {
					string header = dialogueEvent;
					if (header.IndexOf("START_DIALOGUE") > 0)
					{
						header = header.Substring(0, header.IndexOf("START_DIALOGUE")); // Trim header to the first instance of 'START_DIALOGUE'
					}
					else
                    {
						header = header.Substring(0, header.IndexOf("START_EVENT"));
					}

					int.TryParse(header.Substring(0,header.IndexOf(System.Environment.NewLine)), out d_ID);

					string fullDID = (dF_ID.ToString() + "-" + d_ID.ToString()); // e.g '1-2' for dialogue 2 of dialogue_1.txt
					//Debug.Log(fullDID);

					if (header.IndexOf("REPEATABLE = TRUE") == -1 && header.IndexOf("REPEATABLE = True") == -1) // If the dialogue is non-repeatable
                    {
						foreach (string viewedDialogueID in Save_Data.ViewedDialogue) // Check viewed dialogues
						{
							if (viewedDialogueID == fullDID)
							{
								goto ContinueLoop; // If the dialogue has already been viewed, ignore it
							}
						}
					}
					
					if (header.IndexOf("CONDITIONS =") > 0)
                    {
						string conditions = header.Substring(header.IndexOf("CONDITIONS ="), header.IndexOf(System.Environment.NewLine, header.IndexOf("CONDITIONS =")) - header.IndexOf("CONDITIONS ="));
						conditions = header.Substring(header.IndexOf('('), header.LastIndexOf(')') + 1 - header.IndexOf('('));
						//Debug.Log("'" + conditions + "'");
						if (Condition_Checker.CheckConditions(conditions) == false)
                        {
							goto ContinueLoop; // If conditions are not met, ignore this dialogue
                        }
					}

					string priority = "LOWEST";

					if (header.IndexOf("PRIORITY =") > 0)
                    {
						priority = header.Substring(header.IndexOf("PRIORITY =") + 11);
						priority = priority.Substring(0, priority.IndexOf(System.Environment.NewLine));
						if (priority.IndexOf(" ") > 0)
                        {
							priority = priority.Substring(0, priority.IndexOf(" "));
                        }
						if (priority.IndexOf("	") > 0)
						{
							priority = priority.Substring(0, priority.IndexOf("	"));
						}
						Debug.Log("'" + priority + "'");
                    }

					int priorityID = 5;

					switch (priority) // Convert priority to an integer
                    {
						case "LOWEST":
							priorityID = 5;
							break;
						case "Lowest":
							priorityID = 5;
							break;
						case "LOW":
							priorityID = 4;
							break;
						case "Low":
							priorityID = 4;
							break;
						case "MEDIUM":
							priorityID = 3;
							break;
						case "Medium":
							priorityID = 3;
							break;
						case "HIGH":
							priorityID = 2;
							break;
						case "High":
							priorityID = 2;
							break;
						case "HIGHEST":
							priorityID = 1;
							break;
						case "Highest":
							priorityID = 1;
							break;
						default:
							Debug.Log("Priority '" + priority + "' not recognized");
							break;
					}

					if (priorityID < highestPriority) // Keep track of the highest priority in the list
                    {
						highestPriority = priorityID;
                    }

					idAndPriority.Add(fullDID + "[" + priorityID.ToString() + "]");

				}
			}

		}

		foreach (string iDAP in idAndPriority)
		{
			//Debug.Log(iDAP);
			if (iDAP.IndexOf("[" + highestPriority.ToString()) > 0)
			{
				idAndPriorityB.Add(iDAP);
			}
		}

		string str = idAndPriorityB[UnityEngine.Random.Range(0, (idAndPriorityB.Count))]; // e.g 1-3[4]
		Debug.Log(str);

		str = str.Substring(0, str.IndexOf("["));

		dF_ID = 0;
		d_ID = 1;

		int.TryParse(str.Substring(0,str.IndexOf("-")), out dF_ID);
		int.TryParse(str.Substring(str.IndexOf("-") + 1), out d_ID);
		StartDialogue(dF_ID, d_ID);

		Transition_Animator.Play(endTransition);

	}

	public IEnumerator Enter3DRoom(string room, string doorTransform, string startTransition = "Fadeblack_Start", string endTransition = "Fadeblack_End")
    {
		Transition_Animator.Play(startTransition);
		EndDialogue();
		yield return new WaitForSeconds(Transition_Animator.GetCurrentAnimatorStateInfo(0).length);
		StartCoroutine(_Enter3DRoom(room,doorTransform,startTransition,endTransition));
		yield return null;
	}

	private IEnumerator _Enter3DRoom(string room, string doorTransform, string startTransition = "Fadeblack_Start", string endTransition = "Fadeblack_End")
    {
		SceneManager.LoadScene(room);
		yield return new WaitForSeconds(0.1f);
		GameObject playerObject = GameObject.Find("FirstPersonPlayer");
		Transform destinationTransform = GameObject.Find(doorTransform).transform;
		CharacterController controller = playerObject.GetComponent<CharacterController>();
		controller.enabled = false; controller.enabled = false;
		playerObject.transform.position = destinationTransform.position;
		playerObject.transform.rotation = destinationTransform.rotation;
		GameObject.Find("Main Camera").transform.rotation = destinationTransform.rotation;
		controller.enabled = true;
		Event_Manager.UpdateDialogueEvents();
		Transition_Animator.Play(endTransition);
		yield return null;
	}


}
