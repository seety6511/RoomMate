using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Autohand;

[RequireComponent(typeof(AudioSource))]
public class KHJ_KeyLock : MonoBehaviour
{
    public GameObject keyColl;
    public GameObject key;
    public bool isOpen;
    public GameObject body;
    public GameObject keyPos;
    public AudioClip openSound;
    public SH_ControlLock locker;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != keyColl)
            return;

        if (isOpen)
            return;

        isOpen = true;
        source.PlayOneShot(openSound);
        key.GetComponent<Grabbable>().HandsRelease();
        key.GetComponent<Grabbable>().isGrabbable = false;
        key.GetComponent<Rigidbody>().isKinematic = true;
        key.GetComponent<BoxCollider>().isTrigger = true;
        //key.GetComponent<InvenItem>().enabled = false;
        key.transform.parent = keyPos.transform;
        //key.transform.parent = transform;
        key.transform.localPosition = Vector3.zero;
        key.transform.localRotation = Quaternion.Euler(0, 0, 0);

        key.transform.DOLocalRotate(new Vector3(0, -90f, 0), 2f);
        body.transform.DOLocalRotate(new Vector3(0, 90f, 0), 2f);
        locker.LockControl(true);
        //0.07 -> 0.035
        //0.05 -> 0.25
    }
}
