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
    public int currentValue;    //현재값
    [Range(0,35)]
    public int maxValue;        //최대값 (0~maxValue)
    public SH_Direction dir;    //도는 방향
    public bool infinity;       //true = 무한회전, false= 최댓값에서 역행

    int originValue;
    int offset;
    float dig;

    protected override void Awake()
    {
        base.Awake();
        dig = 360 / maxValue;
        isActive = true;
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
    protected override IEnumerator ActiveEvent()
    {
        reloadTimer = 0f;
        Rotate();
        yield return null;
    }
}
