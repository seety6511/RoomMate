using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_BoardRefill : MonoBehaviourPun
{
    Vector3 originPos;
    Quaternion originRot;

    [SerializeField]
    float refillTimer = 1f;

    static List<SH_BoardRefill> refillBoards = new List<SH_BoardRefill>();
    private void Awake()
    {
        if(refillBoards.Count>10)
        {
            Destroy(gameObject);
            return;
        }
        refillBoards.Add(this);
        originPos = transform.position;
        originRot = transform.rotation;
    }

    public void Refill()
    {
        StartCoroutine(Count());
        photonView.RPC("Rpc_Refill", RpcTarget.Others);
    }

    [PunRPC]
    void Rpc_Refill()
    {
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        var copy = PhotonNetwork.Instantiate("SketchBoard", originPos, originRot);
        //copy.transform.position = originPos;
        //copy.transform.rotation = originRot;

        NSR_AutoHandManager.instance.hand_zone_objects.Add(copy.transform);
        copy.SetActive(false);
        yield return new WaitForSeconds(refillTimer);
        copy.SetActive(true);
        copy.GetComponent<SH_BoardRefill>().originPos = copy.transform.position;
        copy.GetComponent<SH_BoardRefill>().originRot = copy.transform.rotation;
        Destroy(this);
    }
}
