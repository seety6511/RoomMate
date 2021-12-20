using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//놓을때 던지기
//집을때 손가락 피봇 위치 설정하기
// 잡은 손 바꾸기
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

            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || Input.GetMouseButtonDown(0))
            {
                Grab(hand_L, leftPos, leftPivot);
                leftCatched = true;
            }
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetMouseButtonDown(1))
            {
                Grab(hand_R, rightPos, rightPivot);
                rightCatched = true;
            }
        }

        if (leftCatched)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) || Input.GetMouseButtonUp(0))
            {
                Drop(hand_L, leftPivot);
                leftCatched = false;
            }
        }
        else if (rightCatched)
        {
            if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch) || Input.GetMouseButtonUp(1))
            {
                Drop(hand_R, rightPivot);
                rightCatched = false;
            }
        }

        void Grab(Transform hand, Transform pos, GameObject pivot)
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
            }
        }

        void Drop(Transform hand, GameObject pivot)
        {
            transform.parent = null;
            rig.useGravity = true;
            rig.isKinematic = false;

            hand.Find("Pivot").gameObject.SetActive(true);
            pivot.SetActive(false);
        }
    }
}
