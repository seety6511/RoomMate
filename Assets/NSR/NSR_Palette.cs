using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Palette : MonoBehaviour
{
    BoxCollider coll;
    void Start()
    {
        coll = GetComponent<BoxCollider>();
    }

    float currTime;
    void Update()
    {

        if (NSR_AutoHandManager.instance.handPlayer) return;

        if(currTime < 2)
        {
            currTime += Time.deltaTime;
            coll.enabled = false;
        }
        else
        {
            coll.enabled = true;
        }
    }
}
