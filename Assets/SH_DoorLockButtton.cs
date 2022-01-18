using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_DoorLockButtton : MonoBehaviour
{
    public string number;
    SH_DoorLock doorLock;

    private void Awake()
    {
        doorLock = FindObjectOfType<SH_DoorLock>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        doorLock.AddNumber(number);
    }
}
