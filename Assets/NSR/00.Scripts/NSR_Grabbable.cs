using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
public class NSR_Grabbable : MonoBehaviour
{
    public Transform leftPos;
    public Transform rightPos;

    Vector3 firstPos;
    Quaternion firstRot;

    public bool isLeft;
    public bool isRight;

    public bool isKey;
    void Start()
    {
        firstPos = transform.position;
        firstRot = transform.rotation;
    }
    void Update()
    {
        if (isLeft)
        {
            print("이동");
            transform.position = Vector3.Lerp(transform.position, leftPos.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, leftPos.rotation, 0.2f);

            //transform.position = leftPos.position;
            //transform.rotation = leftPos.rotation;
        }
        else if (isRight)
        {
            transform.position = Vector3.Lerp(transform.position, rightPos.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rightPos.rotation, 0.2f);
        }
        else
        {
            if (isKey)
                transform.localPosition = new Vector3(0, 0.223f, 0);
            else
                transform.localPosition = Vector3.zero;

            transform.localRotation = Quaternion.identity;
        }

        //Transform hand_L = NSR_AutoHandManager.instance.hand_L.transform;
        //Transform hand_R = NSR_AutoHandManager.instance.hand_R.transform;


        //if (NSR_AutoHandManager.instance.leftCatched == false)
        //{
        //    // 왼손 집기
        //    if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || Input.GetMouseButtonDown(0))
        //    {
        //        leftCatched = Grab(hand_L, leftPos, leftPivot);
        //        NSR_AutoHandManager.instance.leftCatched = leftCatched;
        //    }
        //}
        //if (NSR_AutoHandManager.instance.rightCatched == false)
        //{
        //    // 오른손 집기
        //    if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
        //    {
        //        rightCatched = Grab(hand_R, rightPos, rightPivot);
        //        NSR_AutoHandManager.instance.rightCatched = rightCatched;
        //    }
        //}
      
        ////왼손 놓기
        //if (leftCatched)
        //{
        //    if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || Input.GetMouseButtonUp(0))
        //    {
        //        leftCatched = Drop(hand_L, leftPivot);
        //        NSR_AutoHandManager.instance.leftCatched = leftCatched;

        //    }
        //}
        //// 오른손 놓기
        //if (rightCatched)
        //{
        //    if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetMouseButtonUp(1))
        //    {
        //        rightCatched = Drop(hand_R, rightPivot);
        //        NSR_AutoHandManager.instance.rightCatched = rightCatched;
        //    }
        //}
    }

    //bool Grab(Transform hand, Transform pos, GameObject pivot)
    //{
    //    float dis = Vector3.Distance(transform.position, hand.position);

    //    if (dis < range)
    //    {
    //        if (rightCatched)
    //        {
    //            NSR_AutoHandManager.instance.rightCatched = Drop(hand_R, rightPivot);
    //            rightCatched = false;
    //        }

    //        if (leftCatched)
    //        {
    //            NSR_AutoHandManager.instance.leftCatched = Drop(hand_L, leftPivot);
    //            leftCatched = false;
    //        }

    //        transform.parent = hand;
    //        transform.position = pos.position;
    //        transform.rotation = pos.rotation;
    //        rig.isKinematic = true;
    //        rig.useGravity = false;

    //        hand.Find("Pivot").gameObject.SetActive(false);
    //        pivot.SetActive(true);

    //        return true;
    //    }
    //    return false;
    //}
    //bool Drop(Transform hand, GameObject pivot)
    //{
    //    transform.parent = null;
    //    rig.useGravity = true;
    //    rig.isKinematic = false;

    //    if (hand.GetComponent<Hand>().left)
    //    {
    //        ThrowObjL();
    //    }
    //    else
    //    {
    //        ThrowObjR();
    //    }

    //    hand.Find("Pivot").gameObject.SetActive(true);
    //    pivot.SetActive(false);

    //    return false;
    //} 

    //public float throwPower = 5;
    //void ThrowObjR()
    //{
    //    if (rig != null)
    //    {
    //        rig.velocity = hand_R.GetComponent<Hand>().ThrowVelocity() * throwPower;
    //        var throwAngularVel = hand_R.GetComponent<Hand>().ThrowAngularVelocity();
    //        if (!float.IsNaN(throwAngularVel.x) && !float.IsNaN(throwAngularVel.y) && !float.IsNaN(throwAngularVel.z))
    //            rig.angularVelocity = throwAngularVel;
    //    }
    //}
    //void ThrowObjL()
    //{
    //    if (rig != null)
    //    {
    //        rig.velocity = hand_L.GetComponent<Hand>().ThrowVelocity() * throwPower;
    //        var throwAngularVel = hand_L.GetComponent<Hand>().ThrowAngularVelocity();
    //        if (!float.IsNaN(throwAngularVel.x) && !float.IsNaN(throwAngularVel.y) && !float.IsNaN(throwAngularVel.z))
    //            rig.angularVelocity = throwAngularVel;
    //    }
    //}

}

