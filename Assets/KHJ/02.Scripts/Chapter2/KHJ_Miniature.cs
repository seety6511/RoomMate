using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
public class KHJ_Miniature : MonoBehaviour
{
    public bool isSet;
    public GameObject TargetObj;
    public GameObject Particle;
    private void OnTriggerEnter(Collider other)
    {
        if (isSet)
            return;
        //Ÿ�� ������Ʈ�� �̸� ���ؼ�
        if(other.transform.name == "MiniatureCollider")
        {
            //��� �ִ��� ����
            other.GetComponentInParent<Grabbable>().HandsRelease();
            //��ġ �ӽ÷� ������Ű��
            other.transform.parent.position = transform.position;
            other.transform.parent.rotation = Quaternion.Euler(0,0,0);
            //������
            if(TargetObj != null)
            {
                if(other.transform.parent.name == TargetObj.name)
                {
                    GameObject obj = Instantiate(Particle);
                    obj.transform.position = transform.position;
                    //�������� ������Ű��
                    other.GetComponentInParent<Rigidbody>().useGravity = false;
                    other.GetComponentInParent<Rigidbody>().isKinematic = true;
                    var cols = other.GetComponents<BoxCollider>();
                    foreach (BoxCollider collider in cols)
                        collider.enabled = false;
                    //other.GetComponent<MeshRenderer>().enabled = false;
                    other.GetComponentInParent<Grabbable>().isGrabbable = false;
                    
                    //���� �Ϸ� �� ��Ʈ �����ֱ�
                    isSet = true;
                    GetComponent<KHJ_Hint>().Hint();
                    //�̴Ͼ��� �ϼ��ϸ� �ϼ� ����Ʈ
                    GetComponentInParent<SH_MiniatureBase>().NumPlus();
                }
            }
        }
    }
}
