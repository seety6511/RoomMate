using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_EffectController : MonoBehaviour
{
    SH_Gimmick parent;

    public SH_Effect activeEffect;
    public SH_Effect activatingEffect;
    public SH_Effect disableEffect;
    public SH_Effect clearEffect;
    public SH_Effect waitingEffect;
    public SH_Effect hoveringEffect;

    public void Init()
    {
        parent = GetComponent<SH_Gimmick>();
    }

    void CreateEffect(SH_Effect ef)
    {
        if (ef == null)
            return;

        var effect = Instantiate(ef);
        effect.transform.position = parent.transform.position;
    }

    public void StateUpdate()
    {
        switch (parent.gimmickState)
        {
            case SH_GimmickState.Active:
                CreateEffect(activeEffect);
                break;

            case SH_GimmickState.Activating:
                CreateEffect(activatingEffect);
                break;

            case SH_GimmickState.Waiting:
                CreateEffect(waitingEffect);
                break;

            case SH_GimmickState.Disable:
                CreateEffect(disableEffect);
                break;

            case SH_GimmickState.Hovering:
                CreateEffect(hoveringEffect);
                break;

            case SH_GimmickState.Clear:
                CreateEffect(clearEffect);
                break;

            case SH_GimmickState.None:
                break;
        }
    }
}

