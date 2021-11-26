using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoFracture;
[RequireComponent(typeof(PreFracturedGeometry))]
[RequireComponent(typeof(FractureOnCollision))]
public class SH_Gimmick_Packaging : SH_Gimmick
{
    FractureOnCollision foc;
    Collider coll;
    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider>();
        foc = GetComponent<FractureOnCollision>();
        coll.isTrigger = false;
    }
    //InteractibleLayer가 닿으면 부서진다.
    protected override void OnCollisionEnter(Collision col)
    {
        if (InteractibleCheck(col))
            foc.Fracture(col);
    }
}
