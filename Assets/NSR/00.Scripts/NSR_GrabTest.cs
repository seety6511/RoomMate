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

    public Collider coll;
    void Start()
    {
        handL = NSR_AutoHandManager.instance.hand_L.GetComponent<Hand>();
        handR = NSR_AutoHandManager.instance.hand_R.GetComponent<Hand>();

        book = GetComponentInChildren<AnimatedBookController>();
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
                if (book != null)
                {
                    if (book.getBookState() == 0)
                    {
                        if(Pivot_L.activeSelf)
                            Pivot_L.SetActive(false);
                        gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
                        coll.enabled = true;
                    }
                    else
                    {
                        if (Pivot_L.activeSelf == false)
                            Pivot_L.SetActive(true);
                        coll.enabled = false;
                        NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                        if (child.isLeft == false)
                        {
                            child.isLeft = true;
                        }
                    }
                }
                else
                {
                    coll.enabled = false;
                    if (Pivot_L.activeSelf == false)
                        Pivot_L.SetActive(true);
                    NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                    if (child.isLeft == false)
                    {
                        child.isLeft = true;
                    }
                } 
            }
            else
            {
                if (Pivot_L.activeSelf)

                    Pivot_L.SetActive(false);
                gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
                coll.enabled = true;
            }
        }
        else
        {
            if (Pivot_L.activeSelf)

                Pivot_L.SetActive(false);
            gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
            coll.enabled = true;
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
                        if (Pivot_R.activeSelf)

                            Pivot_R.SetActive(false);
                        gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
                        coll.enabled = true;
                    }
                    else
                    {
                        coll.enabled = false;
                        if (Pivot_R.activeSelf == false)

                            Pivot_R.SetActive(true);
                        NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                        if (child.isRight == false)
                        {
                            child.isRight = true;
                        }
                    }
                }
                else
                {
                    coll.enabled = false;
                    if (Pivot_R.activeSelf == false)

                        Pivot_R.SetActive(true);
                    NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                    if (child.isRight == false)
                    {
                        child.isRight = true;
                    }
                }
                   
            }
            else
            {
                coll.enabled = true;
                if (Pivot_R.activeSelf)

                    Pivot_R.SetActive(false);
                gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
            }
        }
        else
        {
            coll.enabled = true;
            if (Pivot_R.activeSelf)

                Pivot_R.SetActive(false);
            gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
        }
    }
}
