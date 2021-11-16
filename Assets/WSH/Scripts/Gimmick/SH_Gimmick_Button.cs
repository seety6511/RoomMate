using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Button : SH_Gimmick
{
    public bool hasActive;
    public bool hasKeep;

    protected override void Awake()
    {
        base.Awake();
        hasActive = false;
    }

    protected override void Update()
    {
        base.Update();

        if (!hasKeep)
            return;

        if (!hasActive)
            return;

        Reloading();
    }

    protected override IEnumerator ReloadEvent()
    {
        hasActive = false;
        return base.ReloadEvent();
    }

    public override IEnumerator SpecialEffect()
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
