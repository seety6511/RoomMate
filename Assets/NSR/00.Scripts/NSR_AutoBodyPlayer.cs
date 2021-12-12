using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_AutoBodyPlayer : MonoBehaviourPun, IPunObservable
{
    public static NSR_AutoBodyPlayer instance;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //public GameObject hand_R;
    //public GameObject hand_L;

    //public Transform[] leftFingers;
    ////public Transform[] rightFingers;
    //void Update()
    //{
    //    // 마스터가 아니라면 = bodyPlayer
    //    if (PhotonNetwork.IsMasterClient == false)
    //    {

    //        hand_L.SetActive(true);
    //        hand_R.SetActive(true);


    //        if (NSR_AutoHandPlayer.instance != null)
    //        {
    //            // Tracking 위치 받기
    //            NSR_AutoHandManager.instance.trackingContainer.transform.position = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Pos;
    //            NSR_AutoHandManager.instance.trackingContainer.transform.rotation = NSR_AutoHandPlayer.instance.recieve_trackingContainer_Rot;

    //            // 손 위치 받기
    //            body_hand_R.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_R_Pos;
    //            body_hand_R.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_R_Rot;
    //            body_hand_L.transform.position = NSR_AutoHandPlayer.instance.recieve_hand_L_Pos;
    //            body_hand_L.transform.rotation = NSR_AutoHandPlayer.instance.recieve_hand_L_Rot;

    //            //손가락 위치 받기
    //            for (int i = 0; i < 15; i++)
    //            {
    //                leftFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_left_finger_Pos[i];
    //                leftFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_left_finger_Rot[i];
    //                rightFingers[i].transform.position = NSR_AutoHandPlayer.instance.recieve_right_finger_Pos[i];
    //                rightFingers[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_right_finger_Rot[i];
    //            }
    //            //오브젝트 위치 받기
    //            for (int i = 0; i < NSR_AutoHandManager.instance.body_zone_objects.Length; i++)
    //            {
    //                NSR_AutoHandManager.instance.body_zone_objects[i].transform.position = NSR_AutoHandPlayer.instance.recieve_objects_Pos[i];
    //                NSR_AutoHandManager.instance.body_zone_objects[i].transform.rotation = NSR_AutoHandPlayer.instance.recieve_objects_Rot[i];
    //            }
    //        }
    //    }
    //    else
    //    {
    //        hand_L.SetActive(false);
    //        hand_R.SetActive(false);

    //        NSR_AutoHandManager.instance.tv_camera.position = recieve_headCamera_Pos;
    //        NSR_AutoHandManager.instance.tv_camera.rotation = recieve_headCamera_Rot;
    //    }
    //}

    public OVRInput.Controller moveController;
    public OVRInput.Axis2D moveAxis;

    public OVRInput.Controller turnController;
    public OVRInput.Axis2D turnAxis;

    [HideInInspector]
    public Vector2 recieve_moveInput;
    [HideInInspector]
    public float recieve_turnInput;

    [HideInInspector]
    public Vector3 recieve_tv_camera_pos;
    [HideInInspector]
    public Quaternion recieve_tv_camera_Rot;

    public bool recieve_lightInput;
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(OVRInput.Get(moveAxis, moveController));
            stream.SendNext(OVRInput.Get(turnAxis, turnController).x);
            stream.SendNext(NSR_AutoHandManager.instance.tv_camera_pos.position);
            stream.SendNext(NSR_AutoHandManager.instance.tv_camera_pos.rotation);

            stream.SendNext(OVRInput.Get(OVRInput.Button.One, moveController));
        }
        if (stream.IsReading)
        {
            recieve_moveInput = (Vector2)stream.ReceiveNext();
            recieve_turnInput = (float)stream.ReceiveNext();
            recieve_tv_camera_pos = (Vector3)stream.ReceiveNext();
            recieve_tv_camera_Rot = (Quaternion)stream.ReceiveNext();

            recieve_lightInput = (bool)stream.ReceiveNext();
        }
    }
}
