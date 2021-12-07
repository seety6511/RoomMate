using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PatternNode : MonoBehaviour
{
    SH_PatternGrid pg;
    private void Awake()
    {
        pg = transform.parent.GetComponent<SH_PatternGrid>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Finger"))
            return;

        NodeOn();
    }

    void NodeOn()
    {

    }

    void NodeOff()
    {

    }
}
