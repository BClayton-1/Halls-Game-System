using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MeatGame.Possession;

namespace MeatGame.Dialogue
{
    public class ConditionChecker : MonoBehaviour
    {
        public static ConditionChecker Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        /* Script Dependencies
        DialogueManager
        TimeManager
        //PlayerManager (Not Yet Implemented)
        SaveData
        InventoryManager
        */

        void Start()
        {

        }



        public bool CheckConditions(string conditions)
        {
            string[] separators = new string[] { "(", ")", " || ", " && ", " AND ", " OR " };
            string[] individualConditions = conditions.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string individualCondition in individualConditions)
            {
                conditions = conditions.Replace(individualCondition, CheckConditionSingle(individualCondition).ToString());
                //Debug.Log(conditions);
            }

            //Debug.Log(conditions);
            return SimplifyBools(conditions);
        }

        public bool SimplifyBools(string conditions) // Monkey business
        {
            int i = 0;
            while (conditions.Contains("("))
            {
                if (conditions.Contains("(True)"))
                {
                    conditions = conditions.Replace("(True)", "True");
                }
                if (conditions.Contains("(False)"))
                {
                    conditions = conditions.Replace("(False)", "False");
                }
                if (conditions.Contains("(True && True)"))
                {
                    conditions = conditions.Replace("(True && True)", "True");
                }
                if (conditions.Contains("(True && False)"))
                {
                    conditions = conditions.Replace("(True && False)", "False");
                }
                if (conditions.Contains("(False && True)"))
                {
                    conditions = conditions.Replace("(False && True)", "False");
                }
                if (conditions.Contains("(False && False)"))
                {
                    conditions = conditions.Replace("(False && False)", "False");
                }
                if (conditions.Contains("(True || True)"))
                {
                    conditions = conditions.Replace("(True || True)", "True");
                }
                if (conditions.Contains("(True || False)"))
                {
                    conditions = conditions.Replace("(True || False)", "True");
                }
                if (conditions.Contains("(False || True)"))
                {
                    conditions = conditions.Replace("(False || True)", "True");
                }
                if (conditions.Contains("(False || False)"))
                {
                    conditions = conditions.Replace("(False || False)", "False");
                }
                i += 1;
                if (i >= 10)
                {
                    Debug.Log("Infinite loop detected in ConditionChecker. Conditions: " + conditions);
                    conditions = conditions.Replace("(", string.Empty);
                    conditions = conditions.Replace(")", string.Empty);
                }
            }
            if (conditions.Contains("True"))
            {
                //Debug.Log("Simplified Bool: " + conditions);
                return true;
            }
            //Debug.Log("Simplified Bool: " + conditions);
            return false;
        }

        public bool CheckConditionSingle(string condition)
        {
            string[] stringKeywords = new string[] { "DOW" };
            string[] intKeywords = new string[] { "DAY", "TIME", "Day", "Time", "TENACITY", "Tenacity", "COGNITION", "Cognition", "INFLUENCE", "Influence", "LUCK", "Luck", "ITEM[" };
            string[] boolKeywords = new string[] { "&&", "&", "AND", "||", "|", "OR" };
            string signA = "";

            if (stringKeywords.Any(condition.StartsWith)) // String conditions
            {
                string x = "";
                string y = "";
                if (condition.IndexOf(" ") > 0)
                {
                    x = condition.Substring(0, condition.IndexOf(" "));
                    y = condition.Substring(condition.LastIndexOf(" ") + 1);
                }

                if (condition.StartsWith("DOW")) // Day of week condition
                {
                    x = TimeManager.Instance.DOW;
                    if (condition.IndexOf("[") > 0 && condition.IndexOf("]") > 0)
                    {
                        return CheckStringList(x, condition);
                    }
                }
                if (condition.EndsWith("DOW")) // Day of week condition
                {
                    y = TimeManager.Instance.DOW;
                }

                if (condition.IndexOf(" == ") > 0)
                {
                    signA = "==";
                }
                else if (condition.IndexOf(" != ") > 0)
                {
                    signA = "!=";
                }
                return CompareString(x, signA, y);

            }

            else if (intKeywords.Any(condition.Contains)) // Int conditions
            {
                int x = 0;
                int y = 0;
                if (condition.IndexOf(" ") > 0)
                {
                    int.TryParse(condition.Substring(0, condition.IndexOf(" ")), out x);
                    int.TryParse(condition.Substring(condition.LastIndexOf(" ") + 1), out y);
                }
                if (condition.StartsWith("DAY")) // Day condition
                {
                    x = TimeManager.Instance.day;
                    if (condition.Contains("[") && condition.Contains("-"))
                    {
                        return CheckIntRange(x, condition); // e.g DAY[5-10]
                    }
                }
                else if (condition.StartsWith("ITEM["))
                {
                    string _condition = condition.Substring("ITEM[".Length);
                    string itemID = _condition.Substring(0, _condition.IndexOf("]"));
                    x = InventoryManager.Instance.GetQuantity(itemID);
                    _condition = _condition.Substring(0, _condition.IndexOf("]"));
                    if (_condition.Contains("ITEM[")) // e.g (ITEM[Bones] > ITEM[Cheese])
                    {
                        string _itemID = _condition.Substring(_condition.IndexOf("ITEM[") + "ITEM[".Length);
                        _itemID = _itemID.Substring(0, _itemID.IndexOf("]"));
                        y = InventoryManager.Instance.GetQuantity(_itemID);
                    }

                }
                else if (condition.StartsWith("TIME")) // Time condition
                {
                    x = TimeManager.Instance.militaryTime;
                    if (condition.Contains("[") && condition.Contains("-"))
                    {
                        return CheckIntRange(x, condition); // e.g TIME[1030-1230]
                    }
                }
                /*else if (condition.StartsWith("TENACITY") || condition.StartsWith("Tenacity"))
                {
                    x = Player_Manager.tenacityStat;
                }
                else if (condition.StartsWith("COGNITION") || condition.StartsWith("Cognition"))
                {
                    x = Player_Manager.cognitionStat;
                }
                else if (condition.StartsWith("INFLUENCE") || condition.StartsWith("Influence"))
                {
                    x = Player_Manager.influenceStat;
                }
                else if (condition.StartsWith("LUCK") || condition.StartsWith("Luck"))
                {
                    x = Player_Manager.luckStat;
                }*/

                if (condition.EndsWith("DAY")) // Day condition
                {
                    y = TimeManager.Instance.day;
                }
                else if (condition.EndsWith("TIME")) // Time condition
                {
                    y = TimeManager.Instance.militaryTime;
                }
                /*else if (condition.EndsWith("TENACITY") || condition.EndsWith("Tenacity"))
                {
                    y = Player_Manager.tenacityStat;
                }
                else if (condition.EndsWith("COGNITION") || condition.EndsWith("Cognition"))
                {
                    y = Player_Manager.cognitionStat;
                }
                else if (condition.EndsWith("INFLUENCE") || condition.EndsWith("Influence"))
                {
                    y = Player_Manager.influenceStat;
                }
                else if (condition.EndsWith("LUCK") || condition.EndsWith("Luck"))
                {
                    y = Player_Manager.luckStat;
                }*/

                signA = condition.Substring(condition.IndexOf(" ") + 1, condition.LastIndexOf(" ") - condition.IndexOf(" ") - 1);
                return CompareInt(x, signA, y);
            }

            else if (condition.Contains("VIEWED")) // Viewed dialogue conditions
            {
                string dialogueID = condition.Substring(condition.IndexOf("[") + 1, condition.IndexOf("]") - condition.IndexOf("[") - 1);
                if (condition.StartsWith("!"))
                {
                    return !CheckViewed(dialogueID);
                }
                return CheckViewed(dialogueID);
            }

            else if (condition.Contains("NOCOOLDOWN"))
            {
                string cooldownSubject = condition.Substring(condition.IndexOf("NOCOOLDOWN[") + "NOCOOLDOWN[".Length);
                cooldownSubject = cooldownSubject.Substring(0, cooldownSubject.LastIndexOf("]"));
                return !CheckCooldown(cooldownSubject);
            }

            /*else if (condition.Contains("EQUIPPED"))
            {

                string equipment = condition.Substring(condition.IndexOf("EQUIPPED[") + "EQUIPPED[".Length);
                equipment = equipment.Substring(0, equipment.LastIndexOf("]"));
                if (condition.StartsWith("!"))
                {
                    return !Player_Manager.IsEquipped(equipment);
                }
                return Player_Manager.IsEquipped(equipment);
            }*/
            return false;

        }

        private bool CheckIntRange(int x, string condition)
        {
            int lowerRange;
            int upperRange;

            string st = condition.Substring(condition.IndexOf("[") + 1);
            st = st.Substring(0, st.IndexOf("]"));
            int.TryParse(st.Substring(0, st.IndexOf("-")), out lowerRange);
            int.TryParse(st.Substring(st.IndexOf("-") + 1), out upperRange);

            if (x >= lowerRange && x <= upperRange)
            {
                return true;
            }
            return false;
        }

        private bool CheckStringList(string x, string condition)
        {
            string st = condition.Substring(condition.IndexOf("[") + 1);
            st = st.Substring(0, st.IndexOf("]"));
            st = st.Replace(" ", string.Empty);
            string[] values = st.Split(',');
            foreach (string value in values)
            {
                if (value == x)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CompareInt(int x, string signA, int y)
        {
            switch (signA)
            {
                case ">": // x is greater than y
                    if (x > y)
                    {
                        return true;
                    }
                    break;

                case "<": // x is less than y
                    if (x < y)
                    {
                        return true;
                    }
                    break;

                case ">=": // x is greater than or equal to y
                    if (x >= y)
                    {
                        return true;
                    }
                    break;

                case "<=": // x is less than or equal to y
                    if (x <= y)
                    {
                        return true;
                    }
                    break;

                case "==": // x is equal to y
                    if (x == y)
                    {
                        return true;
                    }
                    break;

                case "!=": // x is not equal to y
                    if (x != y)
                    {
                        return true;
                    }
                    break;

                default:
                    Debug.Log("Sign '" + signA + "' not recognised.");
                    return false;
            }
            return false;
        }

        private bool CompareString(string x, string signA, string y)
        {
            if (signA == "==")
            {
                if (x == y)
                {
                    return true;
                }
            }
            else if (signA == "!=")
            {
                if (x != y)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CompareBool(bool x, string signB, bool y)
        {
            if (signB == "&&" || signB == "&" || signB == "AND")
            {
                if (x == true && y == true)
                {
                    return true;
                }
            }
            if (signB == "||" || signB == "|" || signB == "OR")
            {
                if (x == true || y == true)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckViewed(string dialogueID) // Returns "True" if the input dialogue has been viewed
        {
            foreach (string viewedDialogueID in SaveData.Instance.ViewedDialogue)
            {
                if (viewedDialogueID == dialogueID)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckCooldown(string cooldownSubject)
        {
            Debug.Log(cooldownSubject);
            foreach (string cooldownID in DialogueManager.Instance.cooldownList)
            {
                if (cooldownID.StartsWith(cooldownSubject))
                {
                    return true;
                }
            }
            return false;
        }

    }
}