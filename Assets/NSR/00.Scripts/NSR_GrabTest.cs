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

        coll = GetComponent<BoxCollider>();
    }


    private void OnDisable()
    {
        if (objL != null)
            setGrab(Pivot_L, false, true);

        if (objR != null)
            setGrab(Pivot_R, false, false);
    }
    void Update()
    {
        objL = handL.holdingObj;
        objR = handR.holdingObj;

        //¿Þ¼Õ
        if (objL != null && objL.gameObject.name == gameObject.name && (book == null || (book != null && book.getBookState() == 0)))
            setGrab(Pivot_L, false, true);
        else
            setGrab(Pivot_L, false, true);

        // ¿À¸¥¼Õ
        if (objR != null && objR.gameObject.name == gameObject.name && (book == null || (book != null && book.getBookState() == 0)))
            setGrab(Pivot_R, false, false);
        else
            setGrab(Pivot_R, false, false);
    }

    void setGrab(GameObject pivot, bool grab, bool left)
    {
        if (pivot.activeSelf == !grab)
            pivot.SetActive(grab);
        coll.isTrigger = grab;
        NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
        if (child.isLeft == !left)
        {
            child.isLeft = left;
        }
    }
}
