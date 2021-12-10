using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KHJ_Clock : MonoBehaviour
{
    public string nowTime;
    public TMP_Text text;
    public TMP_Text AMPM;
    public TMP_Text PlayTime;
    void Start()
    {
    }

    public string[] tmp;
    void Update()
    {
        PlayTime.text = ((int)Time.realtimeSinceStartup / 60).ToString("D2") + ":" + ((int)Time.realtimeSinceStartup % 60).ToString("D2");

        nowTime = DateTime.Now.ToString();
        tmp = nowTime.Split(' ');

        AMPM.text = tmp[1];
        text.text = tmp[2];
    }
}
