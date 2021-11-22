using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DinoFracture;
public class SH_Gimmick_Packaging : SH_Gimmick
{
    public LayerMask destroyer;    //�� ���̾ �޸� ������Ʈ�� ������ �μ�����.
    FractureOnCollision foc;
    Collider coll;
    protected override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider>();
        foc = GetComponent<FractureOnCollision>();
        coll.isTrigger = false;
    }
    protected override void OnCollisionEnter(Collision col)
    {
        foc.Fracture(col, destroyer);
    }
}
