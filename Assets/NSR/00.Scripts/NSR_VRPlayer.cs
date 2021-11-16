using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_VRPlayer : MonoBehaviourPun, IPunObservable
{
    public bool handPlayer;
    public bool bodyPlayer;
    //============================= Start =============================
    public GameObject OVRCameraRig;
    void Start()
    {
        if (photonView.IsMine)
        {
            // NSR_GameManager 에 나의 photonViewv저장
            NSR_GameManager.instance.myPhotonView = photonView;

            // OVRCameraRig 켜기
            OVRCameraRig.SetActive(true);

            cc = GetComponent<CharacterController>();
        }
    }
    //============================= Update =============================
    void Update()
    {
        #region Input Manager
        bool catch_left = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool drop_left = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
        bool catch_right = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        bool drop_right = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        #endregion

        if (photonView.IsMine) 
        {
            Rotate();
            Move();
            Catch_and_Drop(left_Hand, ref trCatched_Left, catch_left, drop_left, line_left);
            Catch_and_Drop(right_Hand, ref trCatched_Right, catch_right, drop_right, line_right);
        } 
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, 0.2f);
        }
    }
    //====================================================================
    #region 이동 및 회전
    CharacterController cc;
    public float speed = 5;
    void Move()
    {
        Vector2 hv = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Vector3 dirH = Camera.main.transform.right * hv.x;
        Vector3 dirV = Camera.main.transform.forward * hv.y;
        Vector3 dir = dirH + dirV;
        dir.y = 0;
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
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

    Vector3 receivePos;
    Quaternion receiveRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        //만약에 읽을 수 있는 상태라면
        if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
    #endregion

    #region 물건 집고 놓기
    public Transform left_Hand;
    public Transform right_Hand;
    Transform trCatched_Left = null;
    Transform trCatched_Right = null;
    public LineRenderer line_left;
    public LineRenderer line_right;
    void Catch_and_Drop(Transform hand, ref Transform trCatched, bool catchInput, bool dropInput, LineRenderer line)
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


        if (dropInput)
        {
            if (trCatched != null)
            {
                trCatched.GetComponent<Rigidbody>().isKinematic = false;
                trCatched.parent = null;
                trCatched = null;
            }
        }
    }
    #endregion
}
