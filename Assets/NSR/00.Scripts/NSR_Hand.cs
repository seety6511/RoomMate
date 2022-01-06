using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject Pivot;
    public GameObject[] targetObj;
    public GameObject indexPivot;

    Grabbable grabObj;

    public float distance;

    bool isTargetObj;
    private void Update()
    {
        grabObj = GetComponent<Hand>().holdingObj;

        float[] dis = new float[targetObj.Length];
        for(int i = 0; i < targetObj.Length; i++)
        {
            dis[i] = Vector3.Distance(transform.position, targetObj[i].transform.position);
            if(dis[i] < distance)
            {
                isTargetObj = true;
                break;
            }
            else
            {
                isTargetObj = false;
            }
        }


        if(grabObj == null && isTargetObj)
        {
            indexPivot.SetActive(true);
        }
        else
        {
            indexPivot.SetActive(false);
        }
    }
}
