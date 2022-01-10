using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NSR_Grabbable : MonoBehaviourPun
{
    public Transform leftPos;
    public Transform rightPos;

    public bool isLeft;
    public bool isRight;

    public bool isKey;

    float lerpSpeed = 50;
    void Update()
    {
        if (!NSR_AutoHandManager.instance.handPlayer && PhotonNetwork.IsConnected) return;

        if (isLeft)
        {
            print("¿Ãµø");
            transform.position = Vector3.Lerp(transform.position, leftPos.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, leftPos.rotation, lerpSpeed * Time.deltaTime);
        }
        else if (isRight)
        {
            transform.position = Vector3.Lerp(transform.position, rightPos.position, lerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, rightPos.rotation, lerpSpeed * Time.deltaTime);
        }
        else
        {
            if (isKey)
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0.223f, 0), lerpSpeed * Time.deltaTime);
            else
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, lerpSpeed * Time.deltaTime);

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, lerpSpeed * Time.deltaTime);
        }

    }
}

