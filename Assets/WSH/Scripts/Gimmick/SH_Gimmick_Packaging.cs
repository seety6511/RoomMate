using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoFracture;
using System;

[RequireComponent(typeof(PreFracturedGeometry))]
[RequireComponent(typeof(FractureOnCollision))]
public class SH_Gimmick_Packaging : SH_Gimmick
{
    public SH_Gimmick_Destroyer destroyer;
    FractureOnCollision foc;
    Collider coll;
    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider>();
        foc = GetComponent<FractureOnCollision>();
        coll.isTrigger = false;
    }
    //InteractibleLayer가 닿으면 destroyer 체크를 진행한다.
    protected override void OnCollisionEnter(Collision col)
    {
        if (!InteractibleCheck(col))
            return;

        var gimmick = col.gameObject.GetComponent<SH_Gimmick>();

        if (gimmick == null)
            return;


        if (gimmick.GetType() != destroyer.GetType())
            return;

            foc.Fracture(col);
    }
}
