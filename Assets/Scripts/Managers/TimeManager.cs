using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public DialogueManager Dialogue_Manager;



    public int day = 0;
    public int hour = 0;
    public int minute = 0;
    public float second;
    public bool realTimeActive = false;
    public string DOW;
    public int militaryTime = 0000;

    public TextMeshProUGUI readableTimeUI;
    public TextMeshProUGUI readableDOWUI;



    // Start is called before the first frame update
    void Start()
    {
        SetReadableTime();
        UpdateTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (realTimeActive == true)
        {
            second += Time.deltaTime;
        }
        UpdateTime();
    }

    public void UpdateTime()
    {
        while (second >= 60)
        {
            second -= 60;
            minute += 1;
        }
        while (minute >= 60)
        {
            minute -= 60;
            hour += 1;
        }
        while (hour >= 24)
        {
            hour -= 24;
            day += 1;
        }
        SetDOW();
        SetReadableTime();
        int.TryParse(hour.ToString("00") + minute.ToString("00"), out militaryTime);
    }

    public void SetTime(string timeString) // In military time e.g 2215 for 10:15 pm
    {
        string hourString = timeString.Substring(0,2);
        string minuteString = timeString.Substring(2, 2);
        int.TryParse(hourString, out hour);
        int.TryParse(minuteString, out minute);
        second = 0.0f;
        UpdateTime();
        Dialogue_Manager.CheckCooldowns();
    }

    public void AddTime(string unit, int amount)
    {
        if (unit == "day" || unit == "Day" || unit == "DAY")
        {
            day += amount;
        }
        else if (unit == "hour" || unit == "Hour" || unit == "HOUR")
        {
            hour += amount;
        }
        else if (unit == "minute" || unit == "Minute" || unit == "MINUTE")
        {
            minute += amount;
        }
        second = 0.0f;
        UpdateTime();
        Dialogue_Manager.CheckCooldowns();
    }

    public void SetReadableTime()
    {
        if (hour > 12)
        {
            readableTimeUI.text = (hour - 12).ToString() + ":" + minute.ToString("00") + " pm";
        }
        else if (hour == 0)
        {
            readableTimeUI.text = "12:" + minute.ToString("00") + " am";
        }
        else
        {
            readableTimeUI.text = hour.ToString() + ":" + minute.ToString("00") + " am";
        }
        readableDOWUI.text = DOW;
    }

    public void SetDOW()
    {
        int dayInt = day;
        while (dayInt >= 7)
        {
            dayInt -= 7;
        }

        switch(dayInt)
        {
            case 0:
                DOW = "Sunday";
                break;
            case 1:
                DOW = "Monday";
                break;
            case 2:
                DOW = "Tuesday";
                break;
            case 3:
                DOW = "Wednesday";
                break;
            case 4:
                DOW = "Thursday";
                break;
            case 5:
                DOW = "Friday";
                break;
            case 6:
                DOW = "Saturday";
                break;
            default:
                DOW = "Null";
                break;
        }
    }
    
}
