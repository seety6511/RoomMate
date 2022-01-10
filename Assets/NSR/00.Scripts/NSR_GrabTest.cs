using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;


// 초기 콜라이더 사이즈 가져오기
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
    BoxCollider box;
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

        box = GetComponent<BoxCollider>();
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

        //왼손
        if (objL != null && objL.gameObject == gameObject && (book == null || book.getBookState() != 0))
            setGrab(Pivot_L, true, true);
        else
            setGrab(Pivot_L, false, true);

        // 오른손
        if (objR != null && objR.gameObject == gameObject && (book == null || book.getBookState() != 0))
            setGrab(Pivot_R, true, false);
        else
            setGrab(Pivot_R, false, false);

        if(isGrab == true && book.getBookState() != 0)
        {
            box.size = Vector3.zero;
        }
        else if(isGrab == false)
        {
            box.size = Vector3.one;
        }
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
        //    box.size = new Vector3(0, 0, 0);
        //else
        //    box.size = Vector3.one;
    }

    bool isGrab;
    public void OnGrab()
    {
        isGrab = true;
    }

    public void OnReleaseGrab()
    {
        isGrab = false;
    }

}
