using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PageControl : MonoBehaviour
{
    AnimatedBookController abc;

    private void Awake()
    {
        abc = FindObjectOfType<AnimatedBookController>();
    }
    public enum PageDir
    {
        Next,
        Prev,
        Close,
    }
    public PageDir dir;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        switch (dir)
        {
            case PageDir.Next:
                abc.TurnToNextPage();
                break;

            case PageDir.Prev:
                abc.TurnToPreviousPage();
                break;
            case PageDir.Close:
                abc.CloseBook();
                break;
        }
    }
}
