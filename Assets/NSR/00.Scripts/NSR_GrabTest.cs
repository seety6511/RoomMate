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
    }


    private void OnDisable()
    {
        if (objL != null)
        {
            setGrab(Pivot_L, false);
            //if (Pivot_L.activeSelf)
            //    Pivot_L.SetActive(false);
            //gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
        }

        if (objR != null)
        {
            setGrab(Pivot_R, false);
            //if (Pivot_R.activeSelf)
            //    Pivot_R.SetActive(false);
            //gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
        }
    }
    void Update()
    {
        objL = handL.holdingObj;
        objR = handR.holdingObj;

        //¿Þ¼Õ
        if (objL != null)
        {
            if (objL.gameObject.name == gameObject.name)
            {
                coll = GetComponent<BoxCollider>();
                if (book != null)
                {
                    if (book.getBookState() == 0)
                    {
                        //if(Pivot_L.activeSelf)
                        //    Pivot_L.SetActive(false);
                        //gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
                        //coll.isTrigger = true;
                        setGrab(Pivot_L, false);
                    }
                    else
                    {
                        setGrab(Pivot_L, true);
                        //if (Pivot_L.activeSelf == false)
                        //    Pivot_L.SetActive(true);
                        //coll.isTrigger = false;
                        //NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                        //if (child.isLeft == false)
                        //{
                        //    child.isLeft = true;
                        //}
                    }
                }
                else
                {
                    setGrab(Pivot_L, true);
                    //coll.isTrigger = false;
                    //if (Pivot_L.activeSelf == false)
                    //    Pivot_L.SetActive(true);
                    //NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                    //if (child.isLeft == false)
                    //{
                    //    child.isLeft = true;
                    //}
                } 
            }
            else
            {
                setGrab(Pivot_L, false);
                //if (Pivot_L.activeSelf)
                //    Pivot_L.SetActive(false);
                //gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
                ////coll.enabled = true;
                //coll.isTrigger = true;
            }
        }
        else
        {
            setGrab(Pivot_L, false);
            //if (Pivot_L.activeSelf)
            //    Pivot_L.SetActive(false);
            //gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
            ////coll.enabled = true;
            //coll.isTrigger = true;
        }

        // ¿À¸¥¼Õ
        if (objR != null)
        {
            if (objR.name == gameObject.name)
            {
                if (book != null)
                {
                    if (book.getBookState() == 0)
                    {
                        setGrab(Pivot_R, false);
                        //if (Pivot_R.activeSelf)

                        //    Pivot_R.SetActive(false);
                        //gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
                        ////coll.enabled = true;
                        //coll.isTrigger = true;
                    }
                    else
                    {
                        setGrab(Pivot_R, true);
                        ////coll.enabled = false;
                        //coll.isTrigger = false;
                        //if (Pivot_R.activeSelf == false)

                        //    Pivot_R.SetActive(true);
                        //NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                        //if (child.isRight == false)
                        //{
                        //    child.isRight = true;
                        //}
                    }
                }
                else
                {
                    setGrab(Pivot_R, true);
                    ////coll.enabled = false;
                    //coll.isTrigger = false;
                    //if (Pivot_R.activeSelf == false)

                    //    Pivot_R.SetActive(true);
                    //NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                    //if (child.isRight == false)
                    //{
                    //    child.isRight = true;
                    //}
                }
                   
            }
            else
            {
                setGrab(Pivot_R, false);
                //coll.isTrigger = true;
                //if (Pivot_R.activeSelf)

                //    Pivot_R.SetActive(false);
                //gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
            }
        }
        else
        {
            setGrab(Pivot_R, false);
            ////coll.enabled = true;
            //coll.isTrigger = true;
            //if (Pivot_R.activeSelf)

            //    Pivot_R.SetActive(false);
            //gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
        }
    }

    void setGrab(GameObject pivot, bool grab)
    {
        if (pivot.activeSelf == !grab)
            pivot.SetActive(grab);
        coll.isTrigger = !grab;
        NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
        if (child.isLeft == !grab)
        {
            child.isLeft = grab;
        }
    }
}
