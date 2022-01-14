using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KHJ_Clock : MonoBehaviour
{
    public string nowTime;
    public TMP_Text time_app;
    public TMP_Text time_alarm;
    public TMP_Text time_upperbar;
    public TMP_Text AMPM;
    public TMP_Text PlayTime;

    string[] tmp;
    string[] tmp1;
    void Update()
    {
        nowTime = DateTime.Now.ToString();
        tmp = nowTime.Split(' ');
        tmp1 = tmp[2].Split(':');

        if (gameObject.name == "Smartphone")
        {
            if (!GetComponent<KHJ_SmartPhone>().IsSolved)
            {
                time_alarm.text = tmp[1] + " " + tmp1[0] + ":" + tmp1[1];
                return;
            }
        }

        PlayTime.text = ((int)Time.realtimeSinceStartup / 60).ToString("D2") + ":" + ((int)Time.realtimeSinceStartup % 60).ToString("D2");

        AMPM.text = tmp[1];
        time_app.text = tmp[2];

        time_upperbar.text = tmp[1] + " " + tmp1[0] + ":" + tmp1[1];
        BatterySet();
    }

    public Image Battery;
    public Sprite[] batterysprites;
    void BatterySet()
    {
        //5분마다 하나씩 줄어듬
        if(Time.realtimeSinceStartup <= 60 * 5)
        {
            Battery.sprite = batterysprites[0];
        }
        else if(Time.realtimeSinceStartup > 60 * 5 && Time.realtimeSinceStartup <= 60 * 10)
        {
            Battery.sprite = batterysprites[1];
        }
        else if (Time.realtimeSinceStartup > 60 * 10 && Time.realtimeSinceStartup <= 60 * 15)
        {
            Battery.sprite = batterysprites[2];
        }
        else if (Time.realtimeSinceStartup > 60 * 15 && Time.realtimeSinceStartup <= 60 * 20)
        {
            Battery.sprite = batterysprites[3];
        }
        else
        {
            Battery.sprite = batterysprites[4];
        }
    }

}
