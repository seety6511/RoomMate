using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SH_Gimmick_Roller : SH_Gimmick
{
    Quaternion origin;
    public float speed;
    public SH_Direction dir;

    protected override void Awake()
    {
        base.Awake();
        hasActive = true;
        origin = transform.rotation;
    }

    protected override IEnumerator ReloadEvent()
    {
        while (true)
        {

            switch (dir)
            {
                case SH_Direction.Horizontal_F:
                    transform.Rotate(-speed, 0, 0);
                    break;
                case SH_Direction.Horizontal_B:
                    transform.Rotate(speed, 0, 0);
                    break;
                case SH_Direction.Vertical_F:
                    transform.Rotate(0, -speed, 0);
                    break;
                case SH_Direction.Vertical_B:
                    transform.Rotate(0, speed, 0);
                    break;
                case SH_Direction.Both_F:
                    transform.Rotate(-speed, -speed, 0);
                    break;
                case SH_Direction.Both_B:
                    transform.Rotate(speed, speed, 0);
                    break;
            }
            var rot = transform.rotation;
            var x = Mathf.Abs(rot.x - origin.x) < speed;
            var y = Mathf.Abs(rot.y - origin.y) < speed;
            var z = Mathf.Abs(rot.z - origin.z) < speed;

            if(x && y && z)
            {
                transform.rotation = origin;
                break;
            }
        }
        return base.ReloadEvent();
    }

    bool activating;
    protected override IEnumerator ActivatingEffect()
    {
        if (activating)
            yield break;

        activating = true;
        reloadTimer = 0f;
        switch (dir)
        {
            case SH_Direction.Horizontal_F:
                transform.Rotate(speed, 0, 0);
                break;
            case SH_Direction.Horizontal_B:
                transform.Rotate(-speed, 0, 0);
                break;
            case SH_Direction.Vertical_F:
                transform.Rotate(0, speed, 0);
                break;
            case SH_Direction.Vertical_B:
                transform.Rotate(0, -speed, 0);
                break;
            case SH_Direction.Both_F:
                transform.Rotate(speed, speed, 0);
                break;
            case SH_Direction.Both_B:
                transform.Rotate(-speed, -speed, 0);
                break;
        }
        yield return null;
        activating = false;
    }
}
