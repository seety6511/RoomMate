using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_HandZone : MonoBehaviour
{
    public Transform autoHandPlayer;
    public Transform OVRCameraRig;
    void Update()
    {
        transform.position = OVRCameraRig.position;
        transform.rotation = OVRCameraRig.rotation;
    }
}
