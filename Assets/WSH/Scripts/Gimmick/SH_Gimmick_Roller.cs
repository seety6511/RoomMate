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
        origin = transform.rotation;
    }

    protected override void Update()
    {
        base.Update();

        if (!keepState)
            return;

        if (reloadTime <= reloadTimer)
        {
            Debug.Log("A");
            StateChange(SH_GimmickState.Waiting);
            transform.DORotate(origin.eulerAngles,speed);
        }
        else
            reloadTimer += Time.deltaTime;
    }

    protected override IEnumerator ActivatingEffect()
    {
        Debug.Log("B");
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
    }
}
