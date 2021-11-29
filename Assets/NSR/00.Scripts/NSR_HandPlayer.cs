using UnityEngine;
using Photon.Pun;
public class NSR_HandPlayer : MonoBehaviourPun, IPunObservable
{
    public static NSR_HandPlayer instance;
    private void Awake()
    {
        instance = this;
    }

    bool HandDown_L;
    bool HandUp_L;
    bool HandDown_R;
    bool HandUp_R;

    public Transform left_Hand;
    public Transform right_Hand;

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
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
        NSR_PlayerManager.instance.HandIn = true;
    }
    void Update()
    {
        if (NSR_PlayerManager.instance.BodyIn == false) return;

        // �����Ͱ� �ƴϸ� HandPlayer => �� photon ����
        if (PhotonNetwork.IsMasterClient == false)
        {
            // �� ��ġ
            left_Hand.localPosition = NSR_PlayerManager.instance.LeftHandAnchor.localPosition;
            left_Hand.localRotation = NSR_PlayerManager.instance.LeftHandAnchor.localRotation;
            right_Hand.localPosition = NSR_PlayerManager.instance.RightHandAnchor.localPosition;
            right_Hand.localRotation = NSR_PlayerManager.instance.RightHandAnchor.localRotation;

            // ��ǲ
            HandDown_L = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            HandUp_L = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            HandDown_R = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
            HandUp_R = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
        }
        // �׷��� �ʰ� �����͸� BodyPlayer
        else
        {
            // ����ġ �ޱ�
            left_Hand.localPosition = Vector3.Lerp(left_Hand.localPosition, receiveLeftHandPos, 0.2f);
            left_Hand.localRotation = Quaternion.Lerp(left_Hand.localRotation, receiveLeftHandRot, 0.2f);
            right_Hand.localPosition = Vector3.Lerp(right_Hand.localPosition, receiveRightHandPos, 0.2f);
            right_Hand.localRotation = Quaternion.Lerp(right_Hand.localRotation, receiveRightHandRot, 0.2f);

            // ��ǲ �ޱ�
            HandDown_L = receiveHandDown_L;
            HandUp_L = receiveHandUp_L;
            HandDown_R = receiveHandDown_R;
            HandUp_R = receiveHandUp_R;
        }

        // HandPlayer �� �ϴ� ��
        if (NSR_BodyPlayer.instance != null)
        {
            NSR_BodyPlayer bodyPlayer = NSR_BodyPlayer.instance;
            DrawLine(bodyPlayer.left_Hand, bodyPlayer.line_left);
            DrawLine(bodyPlayer.right_Hand, bodyPlayer.line_right);
            Catch(bodyPlayer.left_Hand, ref trCatched_Left, HandDown_L);
            Catch(bodyPlayer.right_Hand, ref trCatched_Right, HandDown_R);
            Drop(true, ref trCatched_Left, HandUp_L);
            Drop(false, ref trCatched_Right, HandUp_R);
            OpenDoor(bodyPlayer.left_Hand, HandDown_L);
            OpenDoor(bodyPlayer.right_Hand, HandDown_R);
        }
    }

    #region ���׸���
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
    #endregion

    #region ���� ����
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
                    trCatched.localPosition = new Vector3(0.04f,0, 0.04f);
                    trCatched.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
    }
    #endregion

    #region ���� ����
    void Drop(bool leftHand, ref Transform trCatched, bool dropInput)
    {
        if (dropInput)
        {
            if (trCatched != null)
            {
                Rigidbody rb = trCatched.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                //���� ��
                Vector3 power;
                //���� ������
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
                if (door.open)
                {
                    print("������");
                }
                else
                {
                    print("������");
                }
            }
        }
    }

    // ================== OnPhotonSerializeView =============================
    bool receiveHandDown_L;
    bool receiveHandUp_L;
    bool receiveHandDown_R;
    bool receiveHandUp_R;

    public Vector3 receiveLeftHandPos;
    public Quaternion receiveLeftHandRot;
    public Vector3 receiveRightHandPos;
    public Quaternion receiveRightHandRot;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //���࿡ �� �� �ִ� ���¶��
        if (stream.IsWriting)
        {
            stream.SendNext(left_Hand.localPosition);
            stream.SendNext(left_Hand.localRotation);
            stream.SendNext(right_Hand.localPosition);
            stream.SendNext(right_Hand.localRotation);

            stream.SendNext(HandDown_L);
            stream.SendNext(HandUp_L);
            stream.SendNext(HandDown_R);
            stream.SendNext(HandUp_R);
        }
        //���࿡ ���� �� �ִ� ���¶��
        if (stream.IsReading)
        {
            receiveLeftHandPos = (Vector3)stream.ReceiveNext();
            receiveLeftHandRot = (Quaternion)stream.ReceiveNext();
            receiveRightHandPos = (Vector3)stream.ReceiveNext();
            receiveRightHandRot = (Quaternion)stream.ReceiveNext();

            receiveHandDown_L = (bool)stream.ReceiveNext();
            receiveHandUp_L = (bool)stream.ReceiveNext();
            receiveHandDown_R = (bool)stream.ReceiveNext();
            receiveHandUp_R = (bool)stream.ReceiveNext();
        }
    }
}
