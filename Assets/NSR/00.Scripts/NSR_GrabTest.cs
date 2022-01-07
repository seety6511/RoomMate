using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;

public class NSR_GrabTest : MonoBehaviour
{
    public Hand handL;
    public Hand handR;

    public GameObject Pivot_L;
    public GameObject Pivot_R;

    Grabbable objL;
    Grabbable objR;

    AnimatedBookController book;
    NSR_Grabbable grabbable;

    Collider coll;



     public Transform leftPos;
     public Transform rightPos;
    public bool isLeft;
     public bool isRight;
    void Start()
    {
        if(handL == null)
        handL = NSR_AutoHandManager.instance.hand_L.GetComponent<Hand>();
        if(handR == null)
        handR = NSR_AutoHandManager.instance.hand_R.GetComponent<Hand>();

        book = GetComponentInChildren<AnimatedBookController>();

        coll = GetComponent<Collider>();
        grabbable = GetComponentInChildren<NSR_Grabbable>();
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
    }

    void setGrab(GameObject pivot, bool grab, bool left)
    {
        if (pivot.activeSelf == !grab)
            pivot.SetActive(grab);

        //coll.isTrigger = grab;

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

    private void LateUpdate()
    {
        if (isLeft)
        {
            print("이동");
            transform.position = Vector3.Lerp(transform.position, leftPos.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, leftPos.rotation, 0.2f);
        }
        else if (isRight)
        {
            transform.position = Vector3.Lerp(transform.position, rightPos.position, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rightPos.rotation, 0.2f);
        }
    }
}
