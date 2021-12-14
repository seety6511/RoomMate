using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_HomeButton : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        OnTrigger();
    }
    public void OnTrigger()
    {
        if (KHJ_SmartPhone.instance.IsRunningApp && KHJ_SmartPhone.instance.IsSolved)
        {
            KHJ_SmartPhone.instance.EndApp();
        }
    }
}
