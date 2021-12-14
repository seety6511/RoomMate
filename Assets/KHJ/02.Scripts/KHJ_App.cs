using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHJ_App : MonoBehaviour
{
    public GameObject App;
    public KHJ_SmartPhone.AppName appName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        Run_App_Trigger();
    }
    public void Run_App_Trigger()
    {
        if (!KHJ_SmartPhone.instance.IsSolved)
        {
            return;
        }
        if(!KHJ_SmartPhone.instance.IsRunningApp)
            KHJ_SmartPhone.instance.StartApp(gameObject);
    }

}
