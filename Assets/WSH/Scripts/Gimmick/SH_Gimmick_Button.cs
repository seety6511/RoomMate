using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Button : SH_Gimmick
{
    protected override IEnumerator ReloadEvent()
    {
        hasActive = false;
        return base.ReloadEvent();
    }

    protected override IEnumerator ActiveEffect()
    {
        if (hasActive)
        {
            hasActive = false;
            StateChange(SH_GimmickState.Waiting, true);
        }
        else
        {
            hasActive = true;
            StateChange(SH_GimmickState.Active);
        }
        yield return null;
    }
}
