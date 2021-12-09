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
    public Vector3 recieve_headCamera_Pos;
    [HideInInspector]
    public Quaternion recieve_headCamera_Rot;

    [HideInInspector]
    public bool recieve_lightInput;
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(OVRInput.Get(moveAxis, moveController));
            stream.SendNext(OVRInput.Get(turnAxis, turnController).x);
            stream.SendNext(NSR_AutoHandManager.instance.headCamera.transform.position);
            stream.SendNext(NSR_AutoHandManager.instance.headCamera.transform.rotation);

            stream.SendNext(OVRInput.Get(OVRInput.Button.One, moveController));
        }
        if (stream.IsReading)
        {
            recieve_moveInput = (Vector2)stream.ReceiveNext();
            recieve_turnInput = (float)stream.ReceiveNext();
            recieve_headCamera_Pos = (Vector3)stream.ReceiveNext();
            recieve_headCamera_Rot = (Quaternion)stream.ReceiveNext();

            recieve_lightInput = (bool)stream.ReceiveNext();
        }
    }
}
