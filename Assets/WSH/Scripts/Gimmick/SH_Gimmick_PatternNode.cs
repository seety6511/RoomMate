using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SH_Gimmick_PatternNode : SH_Gimmick
{
    public bool nodeActive;
    SH_Gimmick_PatternDrawer patternDrawer;
    protected override void Awake()
    {
        base.Awake();
        patternDrawer = transform.parent.GetComponent<SH_Gimmick_PatternDrawer>();
    }

    protected override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);

        if (col.gameObject.layer != LayerMask.NameToLayer(interactiveLayer.ToString()))
        {
            patternDrawer.NodeActive(this);
            nodeActive = true;
        }
    }
}
