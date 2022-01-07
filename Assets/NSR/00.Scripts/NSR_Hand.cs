using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public BoxCollider[] targetColl;
    public GameObject indexPivot;

    Grabbable grabObj;

    bool isTarget;

    private void Update()
    {
        grabObj = GetComponent<Hand>().holdingObj;

        if (isTarget && grabObj == null)
            indexPivot.SetActive(true);
        else
            indexPivot.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < targetColl.Length; i++)
        {
            if (other == targetColl[i])
            {
                isTarget = true;
                break;
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < targetColl.Length; i++)
        {
            if (collision.collider == targetColl[i])
            {
                isTarget = true;
                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        for (int i = 0; i < targetColl.Length; i++)
        {
            if (collision.collider == targetColl[i])
            {
                isTarget = false;
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < targetColl.Length; i++)
        {
            if (other == targetColl[i])
            {
                isTarget = false;
                break;
            }
        }
    }
}
