using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class DateArray
{
    public TMP_Text[] day;
}
public class KHJ_Calendar : MonoBehaviour
{
    public GameObject[] objects;
    public DateArray[] week;
    public TMP_Text Month;

    private DateTime _dateTime;
    public static KHJ_Calendar _calendarInstance;
    private void Awake()
    {
        if (_calendarInstance == null)
        {
            _calendarInstance = this;
        }
    }
    void Start()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            week[i / 7].day[i % 7] = objects[i].GetComponentInChildren<TMP_Text>();
        }
        _dateTime = DateTime.Now;
        CreateCalendar();
    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < objects.Length; i++)
        {
            week[i / 7].day[i % 7].text = "";
            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    week[i / 7].day[i % 7].text = (date + 1).ToString();
                    date++;
                }
            }
        }
        Month.text = _dateTime.Year.ToString("D2") + "." + _dateTime.Month.ToString("D2");
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }
        return 0;
    }
    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

}
