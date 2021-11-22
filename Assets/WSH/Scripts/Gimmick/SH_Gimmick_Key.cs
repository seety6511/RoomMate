using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MText;

//�� Ű�� �۵���Ű�� passwordField�� �Էµȴ�.
public class SH_Gimmick_Key : SH_Gimmick
{
    [HideInInspector]
    public SH_Gimmick_PasswordField passwordField;
    public string value;
    Modular3DText text;
    protected override void Awake()
    {
        base.Awake();
        reloadTime = 0.1f;
        keepState = false;
        text = GetComponentInChildren<Modular3DText>();
        text.Text = value;
    }
    protected override IEnumerator ActiveEffect()
    {
        if (passwordField == null)
            yield break;

        passwordField.Input(value);
        yield return null;
    }
}
