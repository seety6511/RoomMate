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
    public enum HandleType
    {
        Door,
        Cabinet,
    }

    public HandleType handleType;

    public SH_Direction axis;
    public float min;
    public float max;
    Vector3 originPos;
    Quaternion originRot;
    protected override void Awake()
    {
        base.Awake();
        switch (handleType)
        {
            case HandleType.Door:
                DoorSetting();
                break;
            case HandleType.Cabinet:
                CabinetSetting();
                break;
        }
    }

    void SetOrigin()
    {
        door = transform.parent.gameObject;
        door.isStatic = false;

        originRot = transform.rotation;
        originPos = transform.position;

        var a = door.GetComponent<Rigidbody>();

        if (a == null)
            a = door.AddComponent<Rigidbody>();

        a.useGravity = false;

        fj = GetComponent<FixedJoint>();
        fj.connectedBody = a;

        if (door.GetComponent<Collider>() != null)
        {
            var d = door.GetComponent<Collider>();
            d.isTrigger = true;
        }

        GetComponent<Collider>().isTrigger = true;
    }

    void CabinetSetting()
    {
        SetOrigin();
        originPos = door.transform.localPosition;
    }

    void DoorSetting()
    {
        SetOrigin();

        hinge = door.GetComponent<HingeJoint>();
        if (hinge == null)
            hinge = door.AddComponent<HingeJoint>();
        else
        {
            min = hinge.limits.min;
            max = hinge.limits.max;
        }

        hinge.useLimits = true;
        hinge.anchor = Vector3.zero;
        hinge.connectedAnchor = Vector3.zero;

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

        if (handleType == HandleType.Cabinet)
        {
            var pos = transform.position;
            pos.x = originPos.x;
            pos.y = originPos.y;
            pos.z = Mathf.Clamp(pos.z, min, max);
            transform.localPosition = pos;
        }

        base.Update();
    }
}
