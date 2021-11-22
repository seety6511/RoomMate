using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum SH_Direction
{
    Horizontal_F,
    Horizontal_B,
    Vertical_F,
    Vertical_B,
    Both_F,
    Both_B,
}

public class SH_Gimmick_Dial : SH_Gimmick
{
    
    [Range(0,35)]
    public int currentValue;    //���簪
    [Range(0,35)]
    public int maxValue;        //�ִ밪 (0~maxValue)
    public SH_Direction dir;    //���� ����
    public bool infinity;       //true = ����ȸ��, false= �ִ񰪿��� ����

    Quaternion originRot;
    int originValue;
    int offset;
    float dig;

    protected override void Awake()
    {
        base.Awake();
        dig = 360 / maxValue;
        hasActive = true;
        originRot = transform.rotation;
        originValue = currentValue;
    }

    protected override IEnumerator ReloadEvent()
    {
        if (currentValue == originValue)
            yield break;

        while (currentValue != originValue)
        {
            Rotate();
            yield return null;
        }
        yield return base.ReloadEvent();
    }

    void Rotate()
    {
        dig = 360 / maxValue;

        if (infinity)
        {
            currentValue++;
            if (currentValue > maxValue)
                currentValue = 0;
        }
        else
        {
            if (currentValue == maxValue)
                offset = -1;
            else if (currentValue == 0)
                offset = 1;

            currentValue += offset;
            dig *= offset;
        }

        switch (dir)
        {
            case SH_Direction.Horizontal_F:
                transform.Rotate(dig, 0, 0);
                break;
            case SH_Direction.Horizontal_B:
                transform.Rotate(-dig, 0, 0);
                break;
            case SH_Direction.Vertical_F:
                transform.Rotate(0, dig, 0);
                break;
            case SH_Direction.Vertical_B:
                transform.Rotate(0, -dig, 0);
                break;
            case SH_Direction.Both_F:
                transform.Rotate(dig, dig, 0);
                break;
            case SH_Direction.Both_B:
                transform.Rotate(-dig, -dig, 0);
                break;
        }
    }

    public override IEnumerator ActiveEffect()
    {
        reloadTimer = 0f;
        Rotate();
        yield return null;
    }
}
