using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
[RequireComponent(typeof(AudioSource))]
public class KHJ_BlackLight : MonoBehaviour
{
    public bool isBattery;
    public GameObject Light;

    public GameObject obj;
    public GameObject coll;

    private void Start()
    {
        Light.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != coll)
            return;

        isBattery = true;
        obj.GetComponent<Grabbable>().HandsRelease();
        obj.gameObject.SetActive(false);
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
