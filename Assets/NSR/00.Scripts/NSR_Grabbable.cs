using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

//������ ������
// ���� �� �ٲٱ�
public class NSR_Grabbable : MonoBehaviour
{
    public float range;
    Rigidbody rig;

    public Transform leftPos;
    public Transform rightPos;

    public Transform hand_L;
    public Transform hand_R;

    public GameObject leftPivot;
    public GameObject rightPivot;

    bool leftCatched;
    bool rightCatched;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Transform hand_L = NSR_AutoHandManager.instance.hand_L.transform;
        //Transform hand_R = NSR_AutoHandManager.instance.hand_R.transform;


        if (leftCatched == false && rightCatched == false)
        {
            // �޼� ����
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || Input.GetMouseButtonDown(0))
            {
                leftCatched = Grab(hand_L, leftPos, leftPivot);
            }
            // ������ ����
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
            {
                rightCatched = Grab(hand_R, rightPos, rightPivot);
            }
        }
        //�޼� ����
        if (leftCatched)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || Input.GetMouseButtonUp(0))
            {
                Drop(hand_L, leftPivot);
                leftCatched = false;
            }
        }
        // ������ ����
        else if (rightCatched)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetMouseButtonUp(1))
            {
                Drop(hand_R, rightPivot);
                rightCatched = false;
            }
        }
    }

    bool Grab(Transform hand, Transform pos, GameObject pivot)
    {
        float dis = Vector3.Distance(transform.position, hand.position);

        if (dis < range)
        {
            transform.parent = hand;
            transform.position = pos.position;
            transform.rotation = pos.rotation;
            rig.isKinematic = true;
            rig.useGravity = false;

            hand.Find("Pivot").gameObject.SetActive(false);
            pivot.SetActive(true);

            return true;
        }
        return false;
    }
    void Drop(Transform hand, GameObject pivot)
    {
        transform.parent = null;
        rig.useGravity = true;
        rig.isKinematic = false;

        if (hand.GetComponent<Hand>().left)
        {
            ThrowObjL();
        }
        else
        {
            ThrowObjR();
        }

        hand.Find("Pivot").gameObject.SetActive(true);
        pivot.SetActive(false);
    } 

    public float throwPower = 5;
    void ThrowObjR()
    {

        //���� ��
        Vector3 dir = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
        //���� ������
        Vector3 torque = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);

        rig.velocity = dir * throwPower;
        //������ Rigidbody �� angularVelocity ���� angularDir �� ����
        rig.angularVelocity = torque;
    }

    //�޼� ���� �� ������
    void ThrowObjL()
    {

        //���� ��
        Vector3 dir = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
        //���� ������
        Vector3 torque = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);

        rig.velocity = dir * throwPower;
        //������ Rigidbody �� angularVelocity ���� angularDir �� ����
        rig.angularVelocity = torque;
    }

}

