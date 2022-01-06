using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_Hand : MonoBehaviour
{
    public GameObject Pivot;
    public GameObject indexPivot;

    Grabbable grabObj;

    Hand hand;

    public float distance;

    private void Start()
    {
        hand = GetComponent<Hand>();
    }

    private void Update()
    {
        if (grabObj != null) return;

        grabObj = hand.holdingObj;

        float dis;
        dis = Vector3.Distance(transform.position, grabObj.transform.position);

        if(dis < distance)
        {
            indexPivot.SetActive(true);
            Pivot.SetActive(false);
        }
        else
        {
            indexPivot.SetActive(false);
            Pivot.SetActive(true);
        }
    }


}
