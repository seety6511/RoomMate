using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    void Update()
    {
        #region Input Manager
        //물건 잡고 놓기
        bool catch_left = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool drop_left = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool catch_right = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool drop_right = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        #endregion

        Move();
        Rotate();
        Catch(left_Hand, ref trCatched_Left, catch_left,  line_left);
        Catch(right_Hand, ref trCatched_Right, catch_right,  line_right);
        Drop(true, ref trCatched_Left, drop_left);
        Drop(false, ref trCatched_Right, drop_right);
        Push(left_Hand, ref finger_left);
        Push(right_Hand, ref finger_right);
    }

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

    #region 물건 집기
    public Transform left_Hand;
    public Transform right_Hand;
    Transform trCatched_Left = null;
    Transform trCatched_Right = null;
    public LineRenderer line_left;
    public LineRenderer line_right;
    public float throwPower = 5;
    void Catch(Transform hand, ref Transform trCatched, bool catchInput, LineRenderer line)
    {
        int layer = 1 << LayerMask.NameToLayer("item");
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5, layer))
        {
            if (trCatched == null)
            {
                line.gameObject.SetActive(true);
                line.SetPosition(0, hand.position);
                line.SetPosition(1, hit.point);

                if (catchInput)
                {
                    trCatched = hit.transform;
                    trCatched.parent = hand;
                    trCatched.position = hand.position;
                    trCatched.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
            else if (line.gameObject.activeSelf) line.gameObject.SetActive(false);
        }
        else if (line.gameObject.activeSelf) line.gameObject.SetActive(false);
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

    public GameObject finger_left;
    public GameObject finger_right;
    void Push(Transform hand, ref GameObject finger)
    {
        int layer = 1 << LayerMask.NameToLayer("push");
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;
        if(Physics.SphereCast(ray, 0.1f, out hit, 0.1f, layer))
        {
            finger.SetActive(true);
        }
        else
        {
            finger.SetActive(false);
        }
    }
}
