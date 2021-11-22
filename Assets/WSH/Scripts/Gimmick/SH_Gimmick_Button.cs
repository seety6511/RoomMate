using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Button : SH_Gimmick
{
    public bool on;
    protected override void Reloading()
    {
        base.Reloading();
    }
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
        {
            on = true;
        }
        yield return null;
    }

    protected override void OnCollisionEnter()
    {
        if (gimmickState != SH_GimmickState.Active)
            base.OnCollisionEnter();
    }
    protected override void OnCollisionExit()
    {
        if (gimmickState != SH_GimmickState.Active)
            base.OnCollisionExit();
    }
}
