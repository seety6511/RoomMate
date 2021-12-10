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
    void Start()
    {
    }

    public string[] tmp;
    void Update()
    {
        nowTime = DateTime.Now.ToString();
        //2021-12-09 PM 12:51:07
        tmp = nowTime.Split(' ');

        AMPM.text = tmp[1];
        text.text = tmp[2];
    }
}
