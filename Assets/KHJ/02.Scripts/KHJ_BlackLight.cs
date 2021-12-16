using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
[RequireComponent(typeof(AudioSource))]
public class KHJ_BlackLight : MonoBehaviour
{
    public bool isBattery;
    public GameObject Light;

    private void Start()
    {
        Light.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Battery")
            return;

        isBattery = true;
        other.GetComponent<Grabbable>().HandsRelease();
        other.gameObject.SetActive(false);
        if(GetComponent<Grabbable>().IsHeld())
            Light.SetActive(!Light.activeSelf);
    }
    public void Activate()
    {
        if (!isBattery)
            return;
        Light.SetActive(!Light.activeSelf);
    }

}
