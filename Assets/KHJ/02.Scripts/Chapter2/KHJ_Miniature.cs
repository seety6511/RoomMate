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
        //Ÿ�� ������Ʈ�� �̸� ���ؼ�
        if(other.transform.parent.name == TargetObj.name)
        {
            //������ Grabbable ���ϰ� �����
            other.GetComponentInParent<Grabbable>().HandsRelease();
            //other.GetComponentInParent<Grabbable>().isGrabbable = false;
            other.GetComponentInParent<Rigidbody>().isKinematic = true;
            //��ġ ������Ű��
            other.transform.parent.position = transform.position;
            other.transform.parent.rotation = transform.rotation;
            isSet = true;

            GetComponent<KHJ_Hint>().Hint();
        }
    }
}
