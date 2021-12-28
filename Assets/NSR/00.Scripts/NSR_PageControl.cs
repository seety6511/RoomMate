using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_PageControl : MonoBehaviour
{
    NSR_GrabTest grabTest;
    private void Start()
    {
        grabTest = GetComponentInParent<NSR_GrabTest>();
    }
    public enum PageDir
    {
        Next,
        Prev,
        Close,
    }
    public PageDir dir;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

  
    }
}
