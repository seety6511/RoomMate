using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Autohand;

[RequireComponent(typeof(AudioSource))]
public class KHJ_KeyLock : MonoBehaviour
{
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

    /*old
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
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != key)
            return;

        if (isOpen)
            return;

        isOpen = true;
        source.PlayOneShot(openSound);
        key.GetComponent<Grabbable>().HandsRelease();
        key.GetComponent<Grabbable>().isGrabbable = false;
        key.GetComponent<Rigidbody>().isKinematic = true;
        key.GetComponent<MeshCollider>().isTrigger = true;
        key.GetComponent<InvenItem>().enabled = false;
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
