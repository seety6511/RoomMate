using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_Gimmick_Packaging : SH_Gimmick
{
    public LayerMask broker;    //�� ���̾ �޸� ������Ʈ�� ������ �μ�����.
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
