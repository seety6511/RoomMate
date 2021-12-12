using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_InvenHand : MonoBehaviour
{
    public bool left;
    void Update()
    {
        if (NSR_AutoHandManager.instance.isMaster)
        {
            if (left)
            {
                transform.rotation = NSR_AutoHandManager.instance.hand_L.transform.rotation;
                transform.position = NSR_AutoHandManager.instance.hand_L.transform.position;
            }
            else
            {
                transform.rotation = NSR_AutoHandManager.instance.hand_R.transform.rotation;
                transform.position = NSR_AutoHandManager.instance.hand_R.transform.position;
            }
        }
        else
        {
            if (left)
            {
                transform.rotation = NSR_AutoHandManager.instance.body_hand_L.transform.rotation;
                transform.position = NSR_AutoHandManager.instance.body_hand_L.transform.position;
            }
            else
            {
                transform.position = NSR_AutoHandManager.instance.body_hand_R.transform.position;
                transform.rotation = NSR_AutoHandManager.instance.body_hand_R.transform.rotation;
            }
        }
    }
}
