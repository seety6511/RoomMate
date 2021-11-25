using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(FixedJoint))]
public class SH_Gimmick_Handle : SH_Gimmick
{
    GameObject door;
    FixedJoint fj;
    HingeJoint hinge;
    public SH_Direction axis;
    public float min;
    public float max;
    Vector3 originPos;
    Quaternion originRot;
    protected override void Awake()
    {
        base.Awake();
        originPos = gameObject.transform.position;
        door = transform.parent.gameObject;
        originRot = door.transform.rotation;
        door.isStatic = false;
        fj = GetComponent<FixedJoint>();
        var a = door.GetComponent<Rigidbody>();
        if (a == null)
            a = door.AddComponent<Rigidbody>();
        fj.connectedBody = a;

        if (door.GetComponent<Collider>() != null)
        {
            door.GetComponent<Collider>().isTrigger = true;
        }

        hinge = door.GetComponent<HingeJoint>();
        if (hinge == null)
            hinge = door.AddComponent<HingeJoint>();
        else
        {
            min = hinge.limits.min;
            max = hinge.limits.max;
        }
        hinge.connectedAnchor = Vector3.zero;
        hinge.anchor = Vector3.zero;
        hinge.useLimits = true;
        var limit = new JointLimits();
        limit.min = min;
        limit.max = max;
        hinge.limits = limit;

        var x = 0;
        var y = 0;
        switch (axis)
        {
            case SH_Direction.Vertical_B:
                y = -1;
                break;
            case SH_Direction.Vertical_F:
                y = 1;
                break;

            case SH_Direction.Horizontal_B:
                x = -1;
                break;
            case SH_Direction.Horizontal_F:
                x = 1;
                break;
            case SH_Direction.Both_B:
                x = -1;
                y = -1;
                break;
            case SH_Direction.Both_F:
                y = 1;
                x = 1;
                break;
        }

        hinge.axis = new Vector3(x, y, 0);
    }

    protected override void Update()
    {
        if (gimmickState == SH_GimmickState.Disable)
        {
            door.transform.rotation = originRot;
            gameObject.transform.position = originPos;
        }

        base.Update();
    }
}
