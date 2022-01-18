using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Autohand;

[RequireComponent(typeof(AudioSource))]
public class KHJ_SceneManager_2 : MonoBehaviour
{
    bool isEnd;
    public GameObject KeyPos;
    public GameObject EndingCredit;
    public KHJ_ScreenFade EyeCanvas;
    public AudioClip openSound;
    AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnd)
            return;
        if(other.transform.name == "Key")
        {
            //¿£µù¾À
            isEnd = true;
            other.transform.GetComponent<Grabbable>().HandsRelease();
            other.transform.GetComponent<Grabbable>().isGrabbable = false;
            other.transform.GetComponent<Rigidbody>().isKinematic = true;
            other.transform.parent = KeyPos.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            source.PlayOneShot(openSound);
            StartCoroutine(Ending());
        }
    }

    IEnumerator Ending()
    {
        //¿£µù¾À
        yield return new WaitForSeconds(2f);
        EyeCanvas.EyeClose_();
        yield return new WaitForSeconds(2f);
        EndingCredit.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        EndingCredit.transform.DOLocalMoveY(7f, 7f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(10);
        Application.Quit();
    }
}
