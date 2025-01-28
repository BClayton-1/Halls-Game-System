using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MeatGame
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public int day { get; private set; } = 0;
        public int hour { get; private set; } = 0;
        public int minute { get; private set; } = 0;
        public float second { get; private set; }
        public string DOW { get; private set; }
        public int militaryTime { get; private set; } = 0000;

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
            int _militaryTime;
            int.TryParse(hour.ToString("00") + minute.ToString("00"), out _militaryTime);
            militaryTime = _militaryTime;
        }

        public void SetTime(string timeString) // In military time e.g 2215 for 10:15 pm
        {
            string hourString = timeString.Substring(0, 2);
            string minuteString = timeString.Substring(2, 2);
            int _hour;
            int.TryParse(hourString, out _hour);
            hour = _hour;
            int _minute;
            int.TryParse(minuteString, out _minute);
            minute = _minute;
            second = 0.0f;
            UpdateTime();
        }

        public void SetDay(int _day)
        {
            day = _day;
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
            while (dayInt >= 7) // Use modulo here
            {
                dayInt -= 7;
            }

            switch (dayInt)
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

        public void SetRealTimeActive(bool isRealtime)
        {
            realTimeActive = isRealtime;
        }

        public bool realTimeActive { get; private set; } = false;

    }
}