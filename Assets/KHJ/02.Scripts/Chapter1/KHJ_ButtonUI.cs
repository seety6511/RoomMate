using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// UI에 콜라이더를 입혀서 트리거 충돌이 있으면 UI버튼의 OnClick을 실행
/// </summary>
public class KHJ_ButtonUI : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip" || (PhotonNetwork.IsConnected && NSR_AutoHandManager.instance.handPlayer == false))
            return;

        if (PhotonNetwork.IsConnected)
            photonView.RPC("OnClick", RpcTarget.All);
        else
            OnClick();

    }
    [PunRPC]
    public void OnClick()
    {
        GetComponent<Button>().onClick.Invoke();
    }
}
