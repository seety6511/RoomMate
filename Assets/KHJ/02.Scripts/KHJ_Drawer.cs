using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KHJ_Drawer : MonoBehaviour
{
    public bool Open;
    public bool Moving;
    public void OnOffDrawer()
    {
        Moving = true;
        if (!Open)
        {
            Open = true;
            gameObject.transform.DOPause();
            gameObject.transform.DOMoveX(-1.130167f, 1).OnComplete(notMoving);
        }
        else
        {
            Open = false;
            gameObject.transform.DOPause();
            gameObject.transform.DOMoveX(-1.400167f, 1).OnComplete(notMoving);
        }
    }
    void notMoving()
    {
        Moving = false;
    }
}
