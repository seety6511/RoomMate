using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KHJ_Cacalator : MonoBehaviour
{
    public TMP_Text value;

    public string num1;
    public string num2;
    public string charactor;
    public string result;
    public void Input(string input)
    {
        if (isResult)
        {
            isResult = false;
            result = "";
        }
        long inputNum = 0;
        bool canConvert = long.TryParse(input, out inputNum);
        if (canConvert)         //입력값이 숫자
        {
            if (charactor == "")
            {
                if (num1 == "0")
                {
                    if (inputNum == 0)
                        return;
                    else
                    {
                        num1 = "";
                    }
                }
                num1 += inputNum;
            }
            else
            {
                if(num2 == "0")
                {
                    if (inputNum == 0)
                        return;
                    else
                    {
                        num2 = "";
                    }
                }
                num2 += inputNum;
            }
        }
        else
        {
            if(input == "C")
            {
                Init();
                return;
            }
            if(input == "=")
            {
                if (num1 == "")
                    return;
                Equal();
                return;
            }
            charactor = input;  //입력값이 문자
        }
        Visualize();
    }
    void Visualize()
    {
        if(!isResult)
            value.text = num1.ToString() + charactor + num2.ToString();
    }
    void Init()
    {
        num1 = "";
        num2 = "";
        charactor = "";
        value.text = "";
    }
    public bool isResult;
    void Equal()
    {
        float num3 = 0;
        if (num2 == "")
            num2 = num1;
        switch (charactor)
        {
            case "":
                num3 = float.Parse(num1);
                break;
            case "+":
                num3 = int.Parse(num1) + int.Parse(num2);
                break;
            case "-":
                num3 = int.Parse(num1) - int.Parse(num2);
                break;
            case "/":
                num3 = (float)(float.Parse(num1) / float.Parse(num2));
                break;
            case "*":
                num3 = int.Parse(num1) * int.Parse(num2);
                break;
        }
        Init();
        result = num3.ToString();
        value.text = result;
        isResult = true;
    }
}
