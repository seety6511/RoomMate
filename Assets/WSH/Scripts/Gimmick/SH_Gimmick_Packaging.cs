using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Packaging : SH_Gimmick
{
    public LayerMask broker;    //이 레이어가 달린 오브젝트가 닿으면 부서진다.
    Rigidbody rb;
    Collider coll;
    protected override void Awake()
    {
        base.Awake();
        //rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
       // rb.isKinematic = false;
        //rb.useGravity = true;
        coll.isTrigger = false;
    }
    protected override void OnCollisionEnter()
    {
    }
}
