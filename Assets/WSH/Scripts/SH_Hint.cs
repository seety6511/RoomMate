using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// 원하는 오브젝트에 붙인다.
/// 원하는 설명을 인스펙터의 info에 적어넣는다.
/// 레이어마스크를 설정한다.
/// 잡을수 있는 오브젝트만 가능하다.
/// 끝. 
/// </summary>
public class SH_Hint : MonoBehaviourPun
{
    protected SH_HintManager hintManager;
    public Sprite portrait;
    [SerializeField]
    LayerMask interactorLayer;
    public List<string> info = new List<string>();
    public int infoIndex;
    public bool hasOn; //한번이라도 발동되었나?
    void Start()
    {
        hintManager = FindObjectOfType<SH_HintManager>();

        var col = GetComponent<Collider>();
        if (col == null)
        {
            gameObject.AddComponent<MeshCollider>().convex = true;
        }
        infoIndex = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Rpc_OnCollisionEnter(collision);
        //if (hasOn)
        //    return;
        //if ((1 << collision.gameObject.layer) != interactorLayer)
        //    return;
        //if(!hintManager.alreadyInfo)
        //    Hint();

        photonView.RPC("Rpc_OnCollisionEnter", RpcTarget.Others, collision);
    }

    [PunRPC]
    void Rpc_OnCollisionEnter(Collision collision)
    {
        if (hasOn)
            return;
        if ((1 << collision.gameObject.layer) != interactorLayer)
            return;
        if (!hintManager.alreadyInfo)
            Hint();
    }

    public void Hint()
    {
        print("Hint");
        hasOn = true;
        hintManager.EnableHint(this);
    }

    ///만약 infoText를 여러번 쓰고 싶다면
    ///아래와 같이 사용한다.
    ///1. info List에 순서대로 텍스트를 집어넣는다.
    ///2. 최초로 텍스트가 띄워진다.
    ///3. 텍스트가 닫힌다
    ///4. ResetInfo() 를 호출한다.
    ///5. 다시 조건이 맞으면 텍스트를 띄운다.
    public void ResetInfo()
    {
        hasOn = false;
    }
}
