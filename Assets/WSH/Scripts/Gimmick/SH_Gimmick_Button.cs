using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Button : SH_Gimmick
{
    public bool on;

    protected override IEnumerator ReloadEvent()
    {
        on = false;
        return base.ReloadEvent();
    }

    protected override IEnumerator ActiveEffect()
    {
        if (on)
        {
            on = false;
            Waiting();
        }
        else
            on = true;
        yield return null;
    }

}
