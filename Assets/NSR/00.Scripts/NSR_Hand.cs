using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public bool left;
    public GameObject mainPivot;
    //public GameObject indexPivot;

    Grabbable grabObj;

    bool isTarget;

    public GameObject[] pivots;
    public Transform[] pos;

    private void Update()
    {
        if (GetComponent<Hand>())
        {
            grabObj = GetComponent<Hand>().holdingObj;

            if (isTarget && grabObj == null)
                pivots[pivots.Length - 1].SetActive(true);
            else
                pivots[pivots.Length - 1].SetActive(false);
        }

        bool off = false;
        for (int i = 0; i < pivots.Length; i++)
        {
            if (pivots[i].activeSelf)
            {
                mainPivot.SetActive(false);
                off = true;
                break;
            }
        }

        if (off == false)
        {
            mainPivot.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "Smartphone" || other.gameObject.name == "Locker")
            isTarget = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Smartphone" || other.gameObject.name == "Locker")
            isTarget = false;
    }

    public void OnGrab()
    {
        print("ÀâÀ½");
        NSR_AutoHandManager.instance.OnGrab(left);
    }
}
