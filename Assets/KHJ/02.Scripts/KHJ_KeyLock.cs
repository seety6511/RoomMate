using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class KHJ_KeyLock : MonoBehaviour
{
    public GameObject key;
    public bool isOpen;
    public GameObject body;
    public GameObject keyPos;
    public AudioClip openSound;
    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == key && !isOpen)
        {
            isOpen = true;
            source.PlayOneShot(openSound);
        }
    }
    private void Update()
    {
        if (isOpen)
        {
            key.transform.parent = this.transform;
            key.transform.localPosition = keyPos.transform.localPosition;

            key.transform.localRotation = Quaternion.Lerp(key.transform.localRotation, Quaternion.Euler(0,90f,0), Time.deltaTime * 3);
            body.transform.localRotation = Quaternion.Lerp(body.transform.localRotation, Quaternion.Euler(0,90f,0), Time.deltaTime * 3);
        }
    }

}
