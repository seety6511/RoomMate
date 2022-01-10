using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_GrabTest : MonoBehaviour
{
    Hand handL;
    Hand handR;

    GameObject Pivot_L;
    GameObject Pivot_R;

    Grabbable objL;
    Grabbable objR;

    AnimatedBookController book;
    NSR_Grabbable grabbable;
    Vector3 startScale;

    public enum PIVOT
    {
        key,
        battery,
        diary
    }

    public PIVOT pivot;
    void Start()
    {
        handL = NSR_AutoHandManager.instance.hand_L.GetComponent<Hand>();
        handR = NSR_AutoHandManager.instance.hand_R.GetComponent<Hand>();

        book = GetComponentInChildren<AnimatedBookController>();
        grabbable = GetComponentInChildren<NSR_Grabbable>();
        startScale = transform.localScale;

        Pivot_L = NSR_AutoHandManager.instance.hand_L.GetComponent<NSR_Hand>().pivots[(int)pivot];
        Pivot_R = NSR_AutoHandManager.instance.hand_R.GetComponent<NSR_Hand>().pivots[(int)pivot];
    }


    private void OnDisable()
    {
        objL = handL.holdingObj;
        objR = handR.holdingObj;
        if(objL != null)
        {
            if (objL.gameObject == gameObject)
                setGrab(Pivot_L, false, true);
        }

        if(objR != null)
        {
            if (objR.gameObject == gameObject)
                setGrab(Pivot_R, false, false);
        }

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

        if (left)
        {
            if (grabbable.isLeft == !grab)
                grabbable.isLeft = grab;
        }
        else
        {
            if (grabbable.isRight == !grab)
                grabbable.isRight = grab;
        }

        //if (grab)
        //    transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        //else
        //    transform.localScale = startScale;
    }

}
