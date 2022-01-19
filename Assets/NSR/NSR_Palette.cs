using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Palette : MonoBehaviour
{
    public GameObject[] inks;
    BoxCollider coll;
    Rigidbody body;
    void Start()
    {
        coll = GetComponent<BoxCollider>();
        body = GetComponent<Rigidbody>();

        if (NSR_AutoHandManager.instance.handPlayer) return;

        coll.enabled = false;
        body.useGravity = false;

        StartCoroutine(SetRigidbody());
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

    IEnumerator SetRigidbody()
    {
        yield return new WaitForSeconds(3f);
        coll.enabled = true;
        body.useGravity = true;
    }
}
