using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // Scripts
    public PlayerManager Player_Manager;
    public DialogueManager Dialogue_Manager;
    public TimeManager Time_Manager;

    // Viewed dialogues
    public List<string> ViewedDialogue = new List<string>();

    // Day and time
    private int day;
    private int hour;
    private int minute;

    // Player variables
    private int tenacity;
    private int cognition;
    private int influence;
    private int luck;

    private float maxHP;
    private float fleshHP;
    private float damageTemporary;
    private float damagePermanent;

    private int composure;
    private int sanity;

    // Position
    private bool inThreeDEnvironment;
    private string twoDRoom;
    private string threeDScene;
    private Vector3 threeDPosition;



    public void SaveGame()
    {
        day = Time_Manager.day;
        hour = Time_Manager.hour;
        minute = Time_Manager.minute;



        tenacity = Player_Manager.tenacityStat;
        cognition = Player_Manager.cognitionStat;
        influence = Player_Manager.influenceStat;
        luck = Player_Manager.luckStat;

        maxHP = Player_Manager.maxHP;
        fleshHP = Player_Manager.fleshHP;
        damageTemporary = Player_Manager.damageTemporary;
        damagePermanent = Player_Manager.damagePermanent;

        composure = Player_Manager.composure;
        sanity = Player_Manager.sanity;


    }

}
