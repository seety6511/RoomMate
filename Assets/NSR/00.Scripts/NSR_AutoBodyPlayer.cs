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
