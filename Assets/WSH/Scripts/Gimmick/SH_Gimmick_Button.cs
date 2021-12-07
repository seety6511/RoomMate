using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Button : SH_Gimmick
{
    protected override IEnumerator ActiveEvent()
    {
        if(isActive)
        {
            isActive = false;
            StateChange(SH_GimmickState.Waiting);
        }
        yield return base.ActiveEvent();
    }
}
