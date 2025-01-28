using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using MeatGame.Dialogue;

namespace MeatGame.ThreeD
{
	public class EventManager : MonoBehaviour
	{
		public static EventManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;

			characterList = new List<string>()
			{
				"Bastard"
			};
		}

		/* Script Dependencies
		DialogueManager
		ConditionChecker
		SaveData
		*/

		[SerializeField] private GameObject NPC_Prefab;

		private List<string> characterList = new List<string>(); // Contains the names of all characters that can appear in 3D space

		public void UpdateEvents()
		{

		}

		public void UpdateDialogueEvents()
		{
			List<string> checkedCharacters = new List<string>();

			List<string> idAndPriority = new List<string>();
			List<string> idAndPriorityB = new List<string>();

			int dialogueFileID = 0;
			int dialogueID = 0;
			int highestPriority = 6;

			foreach (string character in characterList)
			{
				foreach (string checkedCharacter in checkedCharacters)
				{
					if (checkedCharacter == character)
					{
						continue;
					}
				}


				var dialogueTextAssets = Resources.LoadAll("Dialogue", typeof(TextAsset)).Cast<TextAsset>().ToArray();
				goto StartHereA;
			NextCharacter:
				continue;
			StartHereA:
				foreach (TextAsset dialogueTextAsset in dialogueTextAssets)
				{
					int.TryParse(dialogueTextAsset.name.Substring("dialogue_".Length), out dialogueFileID);
					string textFileContents = dialogueTextAsset.text;
					string[] dialogueEvents = textFileContents.Split(new string[] { "DIALOGUE = " }, StringSplitOptions.None);
					foreach (string dialogueEvent in dialogueEvents)
					{
						goto StartHere;
					NextDialogueEvent:
						continue;
					StartHere:

						if (dialogueEvent.Contains("CHARACTER = " + character))
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

							int.TryParse(header.Substring(0, header.IndexOf(System.Environment.NewLine)), out dialogueID);

							string fullDID = (dialogueFileID.ToString() + "-" + dialogueID.ToString()); // e.g '1-2' for dialogue 2 of dialogue_1.txt

							if (header.Contains("IGNORECOOLDOWN = True") == false && header.Contains("IGNORECOOLDOWN = TRUE") == false && header.Contains("IGNORECOOLDOWN = true") == false)
							{
								foreach (string cooldownID in DialogueManager.Instance.cooldownList)
								{
									if (cooldownID.StartsWith(character))
									{
										goto NextDialogueEvent;
									}
								}
							}

							if (header.IndexOf("REPEATABLE = TRUE") == -1 && header.IndexOf("REPEATABLE = True") == -1) // If the dialogue is non-repeatable
							{
								foreach (string viewedDialogueID in SaveData.Instance.ViewedDialogue) // Check viewed dialogues
								{
									if (viewedDialogueID == fullDID)
									{
										goto NextDialogueEvent; // If the dialogue has already been viewed, ignore it
									}
								}
							}

							if (header.IndexOf("CONDITIONS =") > 0)
							{
								string conditions = header.Substring(header.IndexOf("CONDITIONS ="), header.IndexOf(System.Environment.NewLine, header.IndexOf("CONDITIONS =")) - header.IndexOf("CONDITIONS ="));
								conditions = header.Substring(header.IndexOf('('), header.LastIndexOf(')') + 1 - header.IndexOf('('));
								//Debug.Log("'" + conditions + "'");
								if (ConditionChecker.Instance.CheckConditions(conditions) == false)
								{
									goto NextDialogueEvent; // If conditions are not met, ignore this dialogue
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
								//Debug.Log("'" + priority + "'");
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

				if (idAndPriorityB.Count == 0)
				{
					Debug.Log("No dialogues found for " + character);
					return;

				}

				string str = idAndPriorityB[UnityEngine.Random.Range(0, (idAndPriorityB.Count))]; // e.g 1-3[4]
				Debug.Log(str);

				str = str.Substring(0, str.IndexOf("["));

				dialogueFileID = 0;
				dialogueID = 1;

				int.TryParse(str.Substring(0, str.IndexOf("-")), out dialogueFileID);
				int.TryParse(str.Substring(str.IndexOf("-") + 1), out dialogueID);

				string _header = Resources.Load<TextAsset>("Dialogue/dialogue_" + dialogueFileID).text;
				_header = _header.Substring(_header.IndexOf("DIALOGUE = " + dialogueID));
				_header = _header.Substring(0, _header.IndexOf("START_DIALOGUE"));

				string _headerA = _header;
				while (_headerA.Contains("CHARACTER = "))
				{
					_headerA = _headerA.Substring(_headerA.IndexOf("CHARACTER = ") + "CHARACTER = ".Length);
					string _charName = _headerA.Substring(0, _headerA.IndexOf(System.Environment.NewLine));
					if (_charName.IndexOf("//") > 0)
					{
						_charName = _charName.Substring(0, _charName.IndexOf("//"));
					}
					_charName = _charName.Replace(" ", string.Empty);
					_charName = _charName.Replace("	", string.Empty);
					checkedCharacters.Add(_charName);
				}

				if (_header.Contains("3DSCENE = " + SceneManager.GetActiveScene().name) == false)
				{
					goto NextCharacter;
				}
				while (_header.Contains("PLACENPC("))
				{
					string npcName;
					string npcPos;
					Sprite npcSprite;
					npcName = _header.Substring(_header.IndexOf("PLACENPC(") + "PLACENPC(".Length);
					npcName = npcName.Substring(0, npcName.IndexOf(","));

					npcPos = _header.Substring(_header.IndexOf("PLACENPC("));
					npcPos = npcPos.Substring(npcPos.IndexOf(",") + 1);
					npcPos = npcPos.Substring(0, npcPos.IndexOf(")"));

					Transform _npcPos = GameObject.Find(npcPos).GetComponent<Transform>();

					GameObject NPCObject = Instantiate(NPC_Prefab, _npcPos.position, _npcPos.rotation);
					ThreeDNPC Script_ThreeDNPC = NPCObject.GetComponent<ThreeDNPC>();
					Script_ThreeDNPC.NPCName = npcName;
					Script_ThreeDNPC.dialogueTxt = dialogueFileID;
					Script_ThreeDNPC.dialogueNumber = dialogueID;
					_header = _header.Substring(_header.IndexOf("PLACENPC(") + "PLACENPC(".Length);
				}

			}
		}


		public void StartInteractDialogue(string objectName)
		{

			List<string> idAndPriority = new List<string>();
			List<string> idAndPriorityB = new List<string>();

			int dialogueFileID = 0;
			int dialogueID = 0;
			int highestPriority = 6;


			var dialogueTextAssets = Resources.LoadAll("Dialogue", typeof(TextAsset)).Cast<TextAsset>().ToArray();

			foreach (TextAsset dialogueTextAsset in dialogueTextAssets)
			{
				int.TryParse(dialogueTextAsset.name.Substring("dialogue_".Length), out dialogueFileID);
				string textFileContents = dialogueTextAsset.text;
				string[] dialogueEvents = textFileContents.Split(new string[] { "DIALOGUE = " }, StringSplitOptions.None);
				foreach (string dialogueEvent in dialogueEvents)
				{
					goto StartHere;
				NextDialogueEvent:
					continue;
				StartHere:

					if (dialogueEvent.Contains("OBJECT = " + objectName))
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

						int.TryParse(header.Substring(0, header.IndexOf(System.Environment.NewLine)), out dialogueID);

						string fullDID = (dialogueFileID.ToString() + "-" + dialogueID.ToString()); // e.g '1-2' for dialogue 2 of dialogue_1.txt

						if (header.IndexOf("REPEATABLE = TRUE") == -1 && header.IndexOf("REPEATABLE = True") == -1) // If the dialogue is non-repeatable
						{
							foreach (string viewedDialogueID in SaveData.Instance.ViewedDialogue) // Check viewed dialogues
							{
								if (viewedDialogueID == fullDID)
								{
									goto NextDialogueEvent; // If the dialogue has already been viewed, ignore it
								}
							}
						}

						if (header.IndexOf("CONDITIONS =") > 0)
						{
							string conditions = header.Substring(header.IndexOf("CONDITIONS ="), header.IndexOf(System.Environment.NewLine, header.IndexOf("CONDITIONS =")) - header.IndexOf("CONDITIONS ="));
							conditions = header.Substring(header.IndexOf('('), header.LastIndexOf(')') + 1 - header.IndexOf('('));
							//Debug.Log("'" + conditions + "'");
							if (ConditionChecker.Instance.CheckConditions(conditions) == false)
							{
								goto NextDialogueEvent; // If conditions are not met, ignore this dialogue
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
							//Debug.Log("'" + priority + "'");
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

			if (idAndPriorityB.Count == 0)
			{
				Debug.Log("No dialogues found for " + objectName);
				return;
			}

			string str = idAndPriorityB[UnityEngine.Random.Range(0, (idAndPriorityB.Count))]; // e.g 1-3[4]
			Debug.Log(str);

			str = str.Substring(0, str.IndexOf("["));

			dialogueFileID = 0;
			dialogueID = 1;

			int.TryParse(str.Substring(0, str.IndexOf("-")), out dialogueFileID);
			int.TryParse(str.Substring(str.IndexOf("-") + 1), out dialogueID);


			DialogueManager.Instance.StartDialogue(dialogueFileID, dialogueID);


		}

	}
}