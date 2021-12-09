using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class KHJ_Cacalator : MonoBehaviour
{
    public float NowInput;
    public TMP_Text value;
    void Start()
    {
        
    }

    void Update()
    {
        value.text = num.ToString() + charactor;
    }

    public long num;
    public string charactor;
    public void Input(string input)
    {
        long number1 = 0;
        bool canConvert = long.TryParse(input, out number1);
        if (canConvert)
            num = number1;
        else
            charactor = input;
    }
}
