using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(FixedJoint))]
public class SH_Gimmick_Handle : SH_Gimmick
{
    GameObject door;
    FixedJoint fj;
    HingeJoint hj;
    public SH_Direction axis;
    public float min;
    public float max;
    protected override void Awake()
    {
        base.Awake();
        door = transform.parent.gameObject;
        fj = GetComponent<FixedJoint>();
        fj.connectedBody = door.GetComponent<Rigidbody>();

        if (door.GetComponent<HingeJoint>() == null)
            hj = door.AddComponent<HingeJoint>();
        else
        {
            min = hj.limits.min;
            max = hj.limits.max;
        }
        
        var limit = new JointLimits();
        limit.min = min;
        limit.max = max;
        hj.limits = limit;

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

        hj.axis = new Vector3(x, y, 0);
    }
}
