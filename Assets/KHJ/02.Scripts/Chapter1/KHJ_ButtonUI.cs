using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// UI�� �ݶ��̴��� ������ Ʈ���� �浹�� ������ UI��ư�� OnClick�� ����
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
