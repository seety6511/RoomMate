using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject indexPivot;

    Grabbable grabObj;

    bool isTarget;

    public GameObject[] pivots;

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
        //for (int i = 0; i < targetColl.Length; i++)
        //{
        //    if (other == targetColl[i])
        //    {
        //        isTarget = true;
        //        break;
        //    }
        //}

        if(other.gameObject.name == "Smartphone" || other.gameObject.name == "Locker")
            isTarget = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Smartphone" || collision.gameObject.name == "Locker")
            isTarget = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Smartphone" || collision.gameObject.name == "Locker")
            isTarget = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Smartphone" || other.gameObject.name == "Locker")
            isTarget = false;
    }
}
