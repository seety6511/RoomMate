using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
public class KHJ_TriggerNameCheck : MonoBehaviour
{
    public bool OnTouch;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Auto Hand Player")
        {
            StartCoroutine(VibrateController(0.05f, 0.3f, 1, OVRInput.Controller.LTouch));
            StartCoroutine(VibrateController(0.05f, 0.3f, 1, OVRInput.Controller.RTouch));
            OnTouch = true;
        }
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        other.gameObject.GetComponentInParent<Hand>().PlayHapticVibration(0.1f);
        GetComponentInParent<KHJ_Footprint>().AnswerCheck();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        if (OVRInput.Get(other.gameObject.GetComponentInParent<Autohand.Demo.OVRHandControllerLink>().grabButton, other.gameObject.GetComponentInParent<Autohand.Demo.OVRHandControllerLink>().controller))
        {
            OnTouch = true;
            other.gameObject.GetComponentInParent<Hand>().PlayHapticVibration(0.1f);
        }
        else
            OnTouch = false;
        GetComponentInParent<KHJ_Footprint>().AnswerCheck();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Auto Hand Player")
        {
            OnTouch = false;
        }
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        OnTouch = false;
        GetComponentInParent<KHJ_Footprint>().AnswerCheck();
    }
    protected IEnumerator VibrateController(float waitTime, float frequency, float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, controller);
        yield return new WaitForSeconds(waitTime);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
