using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Hand : MonoBehaviour
{
    public bool left;
    public Transform leftHand;
    public Transform rightHand;
    void Update()
    {
        if (NSR_PlayerManager.instance.bodyControll) return;

        if (left)
        {
            transform.localPosition = leftHand.localPosition;
            transform.localRotation = leftHand.localRotation;

        }
        else
        {
            transform.localPosition = rightHand.localPosition;
            transform.localRotation = rightHand.localRotation;
        }
    }
}
