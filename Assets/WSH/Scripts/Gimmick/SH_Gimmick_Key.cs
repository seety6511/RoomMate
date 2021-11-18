using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//이 키를 작동시키면 passwordField에 입력된다.
public class SH_Gimmick_Key : SH_Gimmick
{
    public SH_Gimmick_PasswordField passwordField;  
    public string value;

    protected override void Awake()
    {
        base.Awake();
        reloadTime = 0.1f;
        keepState = false;
    }
    protected override IEnumerator ActiveEffect()
    {
        if (passwordField == null)
            yield break;

        passwordField.Input(value);
        yield return null;
    }
}
