using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;

public class SH_Gimmick_PasswordField : SH_Gimmick
{
    Modular3DText text;
    public string inputField;
    public string answer;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponent<Modular3DText>();
    }

    public void Input(string value)
    {
        inputField += value;
        text.UpdateText(inputField);

        if(answer == inputField)
        {
            Clear();
        }
    }
}
