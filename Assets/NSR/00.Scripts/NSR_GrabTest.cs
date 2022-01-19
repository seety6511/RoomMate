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
    Vector3 initCollSize;
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

        Pivot_L = NSR_AutoHandManager.instance.hand_L.GetComponent<NSR_Hand>().pivots[(int)pivot];
        Pivot_R = NSR_AutoHandManager.instance.hand_R.GetComponent<NSR_Hand>().pivots[(int)pivot];

        box = GetComponent<BoxCollider>();
        initCollSize = box.size;
    }


    private void OnDisable()
    {
        objL = handL.holdingObj;
        objR = handR.holdingObj;

        setGrab(Pivot_L, false, true);
        setGrab(Pivot_R, false, false);

        isGrab = false;
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

        if(isGrab == true && (book == null || book.getBookState() != 0) && box != null)
            box.size = Vector3.zero;
        else if(isGrab == false)
            box.size = initCollSize;
    }

    void setGrab(GameObject pivot, bool grab, bool left)
    {
        if (pivot !=null && pivot.activeSelf == !grab)
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
