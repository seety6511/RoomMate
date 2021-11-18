using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_PlayerManager : MonoBehaviour
{
    public static NSR_PlayerManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public bool bodyControll;

    public Transform body;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform bodyPlayer;
    public Transform handPlayer;

    public bool HandDown_L;
    public bool HandUp_L;
    public bool HandDown_R;
    public bool HandUp_R;

    void Update()
    {
        if (bodyControll)
        {
            body.parent = bodyPlayer;
            body.localPosition = new Vector3(0, 1.6f, 0);
            bodyPlayer.GetComponent<NSR_HandPlayer>().enabled = true;
            handPlayer.GetComponent<NSR_HandPlayer>().enabled = false;
        }
        else
        {
            body.parent = handPlayer;
            body.localPosition = new Vector3(0, 1.6f, 0);
            bodyPlayer.GetComponent<NSR_HandPlayer>().enabled = false;
            handPlayer.GetComponent<NSR_HandPlayer>().enabled = true;
        }
    }
}
