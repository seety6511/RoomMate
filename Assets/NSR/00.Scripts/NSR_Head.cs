using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Head : MonoBehaviour
{
    public Transform head;

    void Update()
    {
        transform.localPosition = head.localPosition;
        transform.localRotation = head.localRotation;
    }
}
