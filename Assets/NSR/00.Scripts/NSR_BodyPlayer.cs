using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_BodyPlayer : MonoBehaviourPun, IPunObservable
{
    public static NSR_BodyPlayer instance;
    private void Awake()
    {
        instance = this;
    }
    public Transform head;

    public Transform left_Hand;
    public Transform right_Hand;

    public LineRenderer line_left;
    public LineRenderer line_right;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Transform OVRCameraRig = GameObject.FindObjectOfType<OVRCameraRig>().transform;
            OVRCameraRig.parent = transform;
            OVRCameraRig.localPosition = Vector3.zero;
        }

        photonView.RPC("CheckIn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void CheckIn()
    {
        NSR_PlayerManager.instance.BodyIn = true;
    }

    Vector2 hv;
    Vector2 thumb;
    void Update()
    {

        if (NSR_PlayerManager.instance.HandIn == false) return;

        // 미스터는 BodyPlayer => 이 photon 주인
        if (photonView.IsMine)
        {
            // 머리 위치
            head.localPosition = NSR_PlayerManager.instance.CenterEyeAnchor.localPosition;
            head.localRotation = NSR_PlayerManager.instance.CenterEyeAnchor.localRotation;

            // 인풋
            hv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
            thumb = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

            // HandPlayer한테 손 위치 받기
            left_Hand.localPosition = Vector3.Lerp(left_Hand.localPosition, NSR_HandPlayer.instance.receiveLeftHandPos, 0.2f);
            left_Hand.localRotation = Quaternion.Lerp(left_Hand.localRotation, NSR_HandPlayer.instance.receiveLeftHandRot, 0.2f);
            right_Hand.localPosition = Vector3.Lerp(right_Hand.localPosition, NSR_HandPlayer.instance.receiveRightHandPos, 0.2f);
            right_Hand.localRotation = Quaternion.Lerp(right_Hand.localRotation, NSR_HandPlayer.instance.receiveRightHandRot, 0.2f);
        }
        // 아니면 HandPlayer면
        else
        {
            //머리 위치 받기
            head.localPosition = Vector3.Lerp(head.localPosition, receiveHeadPos, 0.2f);
            head.localRotation = Quaternion.Lerp(head.localRotation, receiveHeadRot, 0.2f);

            //인풋 받기
            hv = receiveHv;
            thumb = receiveThumb;

            // 손 위치
            left_Hand.localPosition = NSR_PlayerManager.instance.LeftHandAnchor.localPosition;
            left_Hand.localRotation = NSR_PlayerManager.instance.LeftHandAnchor.localRotation;
            right_Hand.localPosition = NSR_PlayerManager.instance.RightHandAnchor.localPosition;
            right_Hand.localRotation = NSR_PlayerManager.instance.RightHandAnchor.localRotation;
        }

        // BodyPlayer 가 하는 일
        if (NSR_HandPlayer.instance != null)
        {
            Move(hv);
            Rotate(thumb);
        }

        // 스페이스바 누르면 컨트롤 바꾸기
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            photonView.RPC("ChangeControl", RpcTarget.All);
        }

    }

    [PunRPC]
    void ChangeControl(PhotonMessageInfo info)
    {
        PhotonNetwork.SetMasterClient(PhotonNetwork.MasterClient.GetNext());

        Player you = null;
        Player me = null;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                me = PhotonNetwork.PlayerList[i];
            }
            else
            {
                you = PhotonNetwork.PlayerList[i];
            }
        }

        if (me == null || you == null)
            return;

        if (me == NSR_BodyPlayer.instance.photonView.Owner)
        {
            // 내가 바디 -> 핸드
            NSR_BodyPlayer.instance.photonView.TransferOwnership(you);
            NSR_HandPlayer.instance.photonView.TransferOwnership(me);

            NSR_PlayerManager.instance.OVRCameraRig.parent = NSR_HandPlayer.instance.transform;
            NSR_PlayerManager.instance.OVRCameraRig.localPosition = Vector3.zero;
        }
        else
        {
            // 내가 핸드 -> 바디
            NSR_BodyPlayer.instance.photonView.TransferOwnership(me);
            NSR_HandPlayer.instance.photonView.TransferOwnership(you);

            NSR_PlayerManager.instance.OVRCameraRig.parent = transform;
            NSR_PlayerManager.instance.OVRCameraRig.localPosition = Vector3.zero;
        }
    }


    #region 이동 및 회전
    public float speed = 5;

    void Move(Vector2 hv)
    {

        Vector3 dirH = Camera.main.transform.right * hv.x;
        Vector3 dirV = Camera.main.transform.forward * hv.y;
        Vector3 dir = dirH + dirV;
        dir.y = 0;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    }

    public float rotSpeed = 40f;
    float y;

    void Rotate(Vector2 thumb)
    {
        float v = thumb.x;

        y += v * rotSpeed * Time.deltaTime;

        transform.localEulerAngles = new Vector3(0, y, 0);
    }

    // =========================== OnPhotonSerializeView ==================================
    Vector3 receiveHeadPos;
    Quaternion receiveHeadRot;

    Vector2 receiveHv;
    Vector2 receiveThumb;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(NSR_PlayerManager.instance.CenterEyeAnchor.localPosition);
            stream.SendNext(NSR_PlayerManager.instance.CenterEyeAnchor.localRotation);

            stream.SendNext(hv);
            stream.SendNext(thumb);
        }
        //만약에 읽을 수 있는 상태라면
        if (stream.IsReading)
        {
            receiveHeadPos = (Vector3)stream.ReceiveNext();
            receiveHeadRot = (Quaternion)stream.ReceiveNext();

            receiveHv = (Vector2)stream.ReceiveNext();
            receiveThumb = (Vector2)stream.ReceiveNext();
        }
    }
    #endregion

}
