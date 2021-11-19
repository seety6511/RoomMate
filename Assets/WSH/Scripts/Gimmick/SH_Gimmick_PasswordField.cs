using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;

public class SH_Gimmick_PasswordField : SH_Gimmick
{
    SH_InputField text;
    SH_Gimmick_Key[] keys;
    public string inputField;
    public string answer;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<SH_InputField>();
        keys = transform.parent.GetComponentsInChildren<SH_Gimmick_Key>();

        foreach(var k in keys)
        {
            k.passwordField = this;
        }
    }

    public void Input(string value)
    {
        inputField += value;
        text.inputField.UpdateText(inputField);

        if(answer == inputField)
        {
            Clear();
        }
    }
}
