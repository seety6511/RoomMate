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

        if(book != null)
        {
            if (objR != null)
            {
                if (objR.name == gameObject.name)
                {
                    if (book.getBookState() == 0)
                    {
                        Pivot_R.SetActive(false);
                        gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
                    }
                    else
                    {
                        Pivot_R.SetActive(true);

                        NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                        if (child.isLeft == false)
                        {
                            child.isLeft = true;
                        }
                    }
                     
                }
            }
            else
            {
                Pivot_R.SetActive(false);
                gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
            }

            if (objL != null)
            {
                if (objL.name == gameObject.name)
                {
                    if (book.getBookState() == 0)
                    {
                        Pivot_L.SetActive(false);
                        gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
                    }
                    else
                    {
                        Pivot_L.SetActive(true);

                        NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                        if (child.isLeft == false)
                        {
                            child.isLeft = true;
                        }
                    }

                }
            }
            else
            {
                Pivot_L.SetActive(false);
                gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
            }

            return;
        }
        


        if (objL != null)
        {
            if (objL.gameObject.name == gameObject.name)
            {
                Pivot_L.SetActive(true);

                NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                if (child.isLeft == false)
                {
                    child.isLeft = true;
                }
            }
            else
            {
                Pivot_L.SetActive(false);
                gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;

            }
        }
        else
        {
            Pivot_L.SetActive(false);
            gameObject.GetComponentInChildren<NSR_Grabbable>().isLeft = false;
        }

        if (objR != null)
        {
            if (objR.name == gameObject.name)
            {
                Pivot_R.SetActive(true);

                NSR_Grabbable child = gameObject.GetComponentInChildren<NSR_Grabbable>();
                if (child.isRight == false)
                {
                    child.isRight = true;
                }

            }
            else
            {
                Pivot_R.SetActive(false);
                gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;

            }
        }
        else
        {
            Pivot_R.SetActive(false);
            gameObject.GetComponentInChildren<NSR_Grabbable>().isRight = false;
        }

    }
}
