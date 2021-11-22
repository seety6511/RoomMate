using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_HandPlayer : MonoBehaviour
{
    void Update()
    {
        if (NSR_PlayerManager.instance.bodyControll) return;

        NSR_PlayerManager.instance.HandDown_L = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        NSR_PlayerManager.instance.HandUp_L = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        NSR_PlayerManager.instance.HandDown_R = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        NSR_PlayerManager.instance.HandUp_R = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);

        DrawLine(left_Hand, line_left);
        DrawLine(right_Hand, line_right);
        Catch(left_Hand, ref trCatched_Left, NSR_PlayerManager.instance.HandDown_L);
        Catch(right_Hand, ref trCatched_Right, NSR_PlayerManager.instance.HandDown_R);
        Drop(true, ref trCatched_Left, NSR_PlayerManager.instance.HandUp_L);
        Drop(false, ref trCatched_Right, NSR_PlayerManager.instance.HandUp_R);
        OpenDoor(left_Hand, NSR_PlayerManager.instance.HandDown_L);
        OpenDoor(right_Hand, NSR_PlayerManager.instance.HandDown_R);
    }

    public Transform left_Hand;
    public Transform right_Hand;

    public LineRenderer line_left;
    public LineRenderer line_right;

    #region 선그리기
    void DrawLine(Transform hand, LineRenderer line)
    {
        int layer1 = 1 << LayerMask.NameToLayer("item");
        int layer2 = 1 << LayerMask.NameToLayer("door");
        int layer = layer1 + layer2;
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, layer))
        {
            line.gameObject.SetActive(true);
            line.SetPosition(0, hand.position);
            line.SetPosition(1, hit.point);
        }
        else if (line.gameObject.activeSelf) line.gameObject.SetActive(false);
    }
    #endregion

    #region 물건 집기
    Transform trCatched_Left = null;
    Transform trCatched_Right = null;
    public float throwPower = 5;
    void Catch(Transform hand, ref Transform trCatched, bool catchInput)
    {
        int layer = 1 << LayerMask.NameToLayer("item");

        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, layer))
        {
            if (trCatched == null)
            {
                if (catchInput)
                {
                    trCatched = hit.transform;
                    trCatched.parent = hand;
                    trCatched.localPosition = new Vector3(0.04f,0, 0.04f);
                    trCatched.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
    #endregion

    #region 물건 놓기
    void Drop(bool leftHand, ref Transform trCatched, bool dropInput)
    {
        if (dropInput)
        {
            if (trCatched != null)
            {
                Rigidbody rb = trCatched.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                //던진 힘
                Vector3 power;
                //던진 돌림힘
                Vector3 torque;
                if (leftHand)
                {
                    power = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                    torque = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
                }
                else
                {
                    power = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
                    torque = OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
                }

                rb.velocity = power * throwPower;
                rb.angularVelocity = torque;

                trCatched.parent = null;
                trCatched = null;
            }
        }
    }
    #endregion

    void OpenDoor(Transform hand, bool openInput)
    {
        int layer = 1 << LayerMask.NameToLayer("door");
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.1f, out hit, 10, layer))
        {
            if (openInput)
            {
                NSR_Door door = hit.transform.GetComponent<NSR_Door>();
                door.open = !door.open;
                if (door.open)
                {
                    print("문열림");
                }
                else
                {
                    print("문닫힘");
                }
            }
        }
    }
}
