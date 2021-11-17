using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Player : MonoBehaviourPun, IPunObservable
{
    //============================= Start =============================
    public GameObject mainCamera;
    void Start()
    {
        if (photonView.IsMine)
        {
            // NSR_GameManager 에 나의 photonViewv저장
            NSR_GameManager.instance.myPhotonView = photonView;

            // 카메라 켜기
            mainCamera.SetActive(true);

            cc = GetComponent<CharacterController>();

            // 로테이션 초기값 설정
            rotY = transform.localEulerAngles.y;
            rotX = Camera.main.transform.localEulerAngles.x;

            if (NSR_GameManager.instance.bodyPlayer)
            {
                transform.position = GameObject.Find("BodyPlayerPos").transform.position;
            }
            else
            {
                transform.position = GameObject.Find("HandPlayerPos").transform.position;
            }
        }
    }

    //============================= Update =============================
    bool catch_left;
    bool drop_left;
    bool catch_right;
    bool drop_right;
    void Update()
    {
        if (NSR_GameManager.instance.changeBody)
        {
            if (photonView.IsMine)
            {
                if (NSR_GameManager.instance.bodyPlayer)
                {
                    transform.position = Vector3.zero;
                }
                else
                {
                    transform.position = GameObject.Find("HandPlayerPos").transform.position;
                }
            }
            else
            {
                NSR_GameManager.instance.bodyPlayer = !NSR_GameManager.instance.bodyPlayer;
            }

            NSR_GameManager.instance.changeBody = false;
        }

        if (photonView.IsMine)
        {
            if (NSR_GameManager.instance.bodyPlayer)
            {
                left_Hand.position = receiveLeftHandPos;
                left_Hand.rotation = receiveLeftHandRot;
                right_Hand.position = receiveRightHandPos;
                right_Hand.rotation = receiveRightHandRot;

                Move();
                Rotate();
                Catch_and_Drop(left_Hand, ref trCatched_Left, receive_catch_left, receive_drop_left, line_left);
                Catch_and_Drop(right_Hand, ref trCatched_Right, receive_catch_right, receive_drop_right, line_right);
            }
            else
            {
                catch_left = Input.GetMouseButtonDown(0);
                drop_left = Input.GetMouseButtonUp(0);
                catch_right = Input.GetMouseButtonDown(1);
                drop_right = Input.GetMouseButtonUp(1);
            }
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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dirH = transform.right * h;
        Vector3 dirV = transform.forward * v;
        Vector3 dir = dirH + dirV;
        dir.Normalize();

        cc.Move(dir * speed * Time.deltaTime);
    }

    public float rotSpeed = 40f;
    float rotY;
    float rotX;
    void Rotate()
    {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");

            rotX += my * rotSpeed * Time.deltaTime;
            rotY += mx * rotSpeed * Time.deltaTime;
            rotX = Mathf.Clamp(rotX, -60, 60);

            transform.localEulerAngles = new Vector3(0, rotY, 0);
            Camera.main.transform.localEulerAngles = new Vector3(-rotX, 0, 0);
        
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
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
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
        else if(line.gameObject.activeSelf)  line.gameObject.SetActive(false);
        

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

    //============================= OnPhotonSerializeView =============================

    Vector3 receivePos;
    Quaternion receiveRot;

    bool receive_catch_left;
    bool receive_drop_left;
    bool receive_catch_right;
    bool receive_drop_right;

    Vector3 receiveLeftHandPos;
    Quaternion receiveLeftHandRot;
    Vector3 receiveRightHandPos;
    Quaternion receiveRightHandRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 쓸 수 있는 상태라면
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            stream.SendNext(catch_left);
            stream.SendNext(drop_left);
            stream.SendNext(catch_right);
            stream.SendNext(drop_right);

            stream.SendNext(left_Hand.position);
            stream.SendNext(left_Hand.rotation);
            stream.SendNext(right_Hand.position);
            stream.SendNext(right_Hand.rotation);
        }
        //만약에 읽을 수 있는 상태라면
        if (stream.IsReading)
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();

            receive_catch_left = (bool)stream.ReceiveNext();
            receive_drop_left = (bool)stream.ReceiveNext();
            receive_catch_right = (bool)stream.ReceiveNext();
            receive_drop_right = (bool)stream.ReceiveNext();

            receiveLeftHandPos = (Vector3)stream.ReceiveNext();
            receiveLeftHandRot = (Quaternion)stream.ReceiveNext();
            receiveRightHandPos = (Vector3)stream.ReceiveNext();
            receiveRightHandRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
