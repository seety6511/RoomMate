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
        //타켓 오브젝트와 이름 비교해서
        if(other.transform.name == "MiniatureCollider")
        {
            //잡고 있던거 놓기
            other.GetComponentInParent<Grabbable>().HandsRelease();
            //위치 임시로 고정시키기
            other.transform.parent.position = transform.position;
            other.transform.parent.rotation = Quaternion.Euler(0,0,0);
            //같으면
            if(TargetObj != null)
            {
                if(other.transform.parent.name == TargetObj.name)
                {
                    GameObject obj = Instantiate(Particle);
                    obj.transform.position = transform.position;
                    //못만지게 고정시키기
                    other.GetComponentInParent<Rigidbody>().useGravity = false;
                    other.GetComponentInParent<Rigidbody>().isKinematic = true;
                    var cols = other.GetComponents<BoxCollider>();
                    foreach (BoxCollider collider in cols)
                        collider.enabled = false;
                    //other.GetComponent<MeshRenderer>().enabled = false;
                    other.GetComponentInParent<Grabbable>().isGrabbable = false;
                    
                    //셋팅 완료 후 힌트 보여주기
                    isSet = true;
                    GetComponent<KHJ_Hint>().Hint();
                    //미니어쳐 완성하면 완성 이펙트
                    GetComponentInParent<SH_MiniatureBase>().NumPlus();
                }
            }
        }
    }
}
