using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_GrabTest : MonoBehaviour
{
    Hand handL;
    Hand handR;

    public GameObject Pivot_L;
    public GameObject Pivot_R;

    Grabbable objL;
    Grabbable objR;

    AnimatedBookController book;

    Collider coll;
    void Start()
    {
        handL = NSR_AutoHandManager.instance.hand_L.GetComponent<Hand>();
        handR = NSR_AutoHandManager.instance.hand_R.GetComponent<Hand>();

        book = GetComponentInChildren<AnimatedBookController>();

        coll = GetComponent<Collider>();
    }


    private void OnDisable()
    {
        if (objL.gameObject == gameObject)
            setGrab(Pivot_L, false, true);

        if (objR.gameObject == gameObject)
            setGrab(Pivot_R, false, false);
    }
    void Update()
    {
        objL = handL.holdingObj;
        objR = handR.holdingObj;

        //¿Þ¼Õ
        if (objL != null && objL.gameObject == gameObject && (book == null || book.getBookState() != 0))
            setGrab(Pivot_L, true, true);
        else
            setGrab(Pivot_L, false, true);

        // ¿À¸¥¼Õ
        if (objR != null && objR.gameObject == gameObject && (book == null || book.getBookState() != 0))
            setGrab(Pivot_R, true, false);
        else
            setGrab(Pivot_R, false, false);
    }

    void setGrab(GameObject pivot, bool grab, bool left)
    {
        if (pivot.activeSelf == !grab)
            pivot.SetActive(grab);
        coll.isTrigger = grab;

        NSR_Grabbable pos = gameObject.GetComponentInChildren<NSR_Grabbable>();
        if (left)
        {
            SetGrabbable(pos.isLeft, grab);
        }
        else
        {
            SetGrabbable(pos.isRight, grab);
        }
    }

    void SetGrabbable(bool grabbable, bool set)
    {
        if (grabbable == !set)
            grabbable = set;
    }
}
