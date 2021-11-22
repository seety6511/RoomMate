using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_GimmickTest : SH_Gimmick
{
    protected override IEnumerator ActiveEffect()
    {
        Debug.Log("Test");
        yield return null;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
            Active();
    }
}
