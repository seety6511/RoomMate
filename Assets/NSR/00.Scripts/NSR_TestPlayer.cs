using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_TestPlayer : MonoBehaviour
{
    void Update()
    {
        #region Input Manager
        //물건 잡고 놓기
        bool HandDown_L = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool HandUp_L = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool HandDown_R = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool HandUp_R = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        #endregion

        Move();
        Rotate();
        DrawLine(left_Hand, line_left);
        DrawLine(right_Hand, line_right);
        Catch(left_Hand, ref trCatched_Left, HandDown_L);
        Catch(right_Hand, ref trCatched_Right, HandDown_R);
        Drop(true, ref trCatched_Left, HandUp_L);
        Drop(false, ref trCatched_Right, HandUp_R);
        OpenDoor(left_Hand, HandDown_L);
        OpenDoor(right_Hand, HandDown_R);
    }

    public Transform left_Hand;
    public Transform right_Hand;

    public LineRenderer line_left;
    public LineRenderer line_right;
    #region 이동 및 회전
    public float speed = 5;
    void Move()
    {
        Vector2 hv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Vector3 dirH = Camera.main.transform.right * hv.x;
        Vector3 dirV = Camera.main.transform.forward * hv.y;
        Vector3 dir = dirH + dirV;
        dir.y = 0;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }

    public float rotSpeed = 40f;
    float y;
    void Rotate()
    {
        Vector2 thumb = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);
        float v = thumb.x;

        y += v * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }

    #endregion

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
                    trCatched.position = hand.position;
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

    //public GameObject finger_left;
    //public GameObject finger_right;
    //void PushAnim(Transform hand, ref GameObject finger)
    //{
    //    int layer = 1 << LayerMask.NameToLayer("push");
    //    Ray ray = new Ray(hand.position, hand.forward);
    //    RaycastHit hit;
    //    if (Physics.SphereCast(ray, 0.1f, out hit, 0.1f, layer))
    //    {
    //    }
    //}

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
            }
        }
    }
}
