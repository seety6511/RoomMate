using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Palette : MonoBehaviour
{
    BoxCollider coll;
    void Start()
    {
        coll = GetComponent<BoxCollider>();
        coll.enabled = false;
    }

    float currTime;
    void Update()
    {
        if(currTime < 2)
        {
            currTime += Time.deltaTime;
        }
        else
        {
            coll.enabled = true;
        }
    }
}
