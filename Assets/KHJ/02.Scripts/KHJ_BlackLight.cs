using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class KHJ_BlackLight : MonoBehaviour
{
    public bool isBattery;
    public bool isHolding;
    public GameObject Light;
    public AudioClip clickSound;
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        Light.SetActive(false);
    }
    public void Activate()
    {
        //source.PlayOneShot(clickSound);
        if (!isBattery)
            return;
        Light.SetActive(!Light.activeSelf);
    }
}
