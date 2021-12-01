using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_BlackLight : SH_Gimmick
{
    public bool isBattery;
    protected override IEnumerator ActiveEffect()
    {
        base.ActiveEffect();
        if (isActive)
        {
            isActive = false;
            Waiting();
            yield break;
        }
    }
    protected override void Active()
    {
        if (!isBattery)
            return;

        base.Active();
    }
    protected override void Activating()
    {
        base.Activating();
    }
}
