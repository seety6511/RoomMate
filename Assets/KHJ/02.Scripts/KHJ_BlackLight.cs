using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        other.gameObject.SetActive(false);
        Light.SetActive(!Light.activeSelf);
    }
    public void Activate()
    {
        if (!isBattery)
            return;
        Light.SetActive(!Light.activeSelf);
    }
}
