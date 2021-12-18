using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

//놓을때 던지기
//집을때 손가락 피봇 위치 설정하기
// 잡은 손 바꾸기
public class NSR_Grabbable : MonoBehaviour
{
    public float range;
    bool leftCatched;
    bool rightCatched;

    public Transform Pos;
    Rigidbody rig;

    public Transform hand_L;
    public Transform hand_R;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Transform hand_L = NSR_AutoHandManager.instance.hand_L.transform;
        //Transform hand_R = NSR_AutoHandManager.instance.hand_R.transform;
        float dis_L = Vector3.Distance(transform.position, hand_L.position);
        float dis_R = Vector3.Distance(transform.position, hand_R.position);

        if (rightCatched == false && leftCatched == false)
        {
            
            //if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
            if (Input.GetMouseButtonDown(0))
            {
                if (dis_L < range)
                {
                    transform.parent = hand_L;
                    transform.position = Pos.position;
                    transform.rotation = Pos.rotation;
                    hand_L.GetComponent<Hand>().enabled = false;
                    rig.isKinematic = true;
                    rig.useGravity = false;

                    leftCatched = true;
                }
            }
            //if(OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))

            if (Input.GetMouseButtonDown(1))
            {
                if (dis_R < range)
                {
                    transform.parent = hand_R;
                    transform.position = Pos.position;
                    transform.rotation = Pos.rotation;
                    rig.useGravity = false;
                    rig.isKinematic = true;
                    hand_L.GetComponent<Hand>().enabled = false;

                    rightCatched = true;
                }
            }
        }

        

        if(leftCatched)
        {
            //if(OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))

            if (Input.GetMouseButtonUp(0))
            {
                transform.parent = null;

                rig.isKinematic = false;
                rig.useGravity = true;
                hand_L.GetComponent<Hand>().enabled = true;

                leftCatched = false;
            }
        }

        if (rightCatched)
        {
            //if(OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch))

            if (Input.GetMouseButtonUp(1))
            {
                transform.parent = null;

                rig.useGravity = true;
                rig.isKinematic = false;
                hand_R.GetComponent<Hand>().enabled = true;

                rightCatched = false;
            }
        }
    }
}
