using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NSR_Grabbable : MonoBehaviourPun
{
    public bool isLeft;
    public bool isRight;

    public Transform leftPos;
    public Transform rightPos;
    public float lerpSpeed = 10;
    NSR_GrabTest grab;
    private void Start()
    {
        grab = GetComponentInParent<NSR_GrabTest>();
    }
    void Update()
    {
        if (!NSR_AutoHandManager.instance.handPlayer && PhotonNetwork.IsConnected) return;

        //leftPos = NSR_AutoHandManager.instance.hand_L.GetComponent<NSR_Hand>().pos[(int)grab.pivot];
        //rightPos = NSR_AutoHandManager.instance.hand_R.GetComponent<NSR_Hand>().pos[(int)grab.pivot];

        if (isLeft)
        {
            print("¿Ãµø");
            transform.position = Vector3.Lerp(transform.position, leftPos.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, leftPos.rotation, lerpSpeed * Time.deltaTime);
        }
        else if (isRight)
        {
            ////print(rightPos.position);
            transform.position = Vector3.Lerp(transform.position, rightPos.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rightPos.rotation, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, lerpSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, lerpSpeed * Time.deltaTime);
        }

    }
}

