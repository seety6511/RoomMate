using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NSR_Flashlight : MonoBehaviourPun
{
    public GameObject blacklight;
    [HideInInspector]
    public bool grabbing;
    void Update()
    {
        if (NSR_AutoHandPlayer.instance != null)
        {

        }
    }



    public void Grabbing(bool grab)
    {
        grabbing = grab;
    }
}
