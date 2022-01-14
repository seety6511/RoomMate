using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
public class KHJ_Miniature : MonoBehaviour
{
    public bool isSet;
    public GameObject TargetObj;
    private void OnTriggerEnter(Collider other)
    {
        if (isSet)
            return;
        //타켓 오브젝트와 이름 비교해서
        if(other.transform.parent.name == TargetObj.name)
        {
            //같으면 Grabbable 못하게 만들고
            other.GetComponentInParent<Grabbable>().HandsRelease();
            //other.GetComponentInParent<Grabbable>().isGrabbable = false;
            other.GetComponentInParent<Rigidbody>().isKinematic = true;
            //위치 고정시키기
            other.transform.parent.position = transform.position;
            other.transform.parent.rotation = transform.rotation;
            isSet = true;

            GetComponent<KHJ_Hint>().Hint();
        }
    }
}
