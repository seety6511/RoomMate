using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
using Photon.Pun;
[RequireComponent(typeof(AudioSource))]
public class KHJ_BlackLight : MonoBehaviourPun
{
    public bool isBattery;
    public GameObject Light;

    public GameObject obj;
    public GameObject coll;

    private void Start()
    {
        Light.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != coll)
            return;

        isBattery = true;
        GetComponent<SH_Hint>().hasOn = false;
        obj.GetComponent<Grabbable>().HandsRelease();
        obj.gameObject.SetActive(false);
        if (GetComponent<Grabbable>().IsHeld())
            if (PhotonNetwork.IsConnected)
                photonView.RPC("setLight", RpcTarget.All);
            else
                Light.SetActive(!Light.activeSelf);
    }
    public void Activate()
    {
        if (!isBattery)
            return;

        if (PhotonNetwork.IsConnected)
            photonView.RPC("setLight", RpcTarget.All);
        else
            Light.SetActive(!Light.activeSelf);
    }

    [PunRPC]
    void setLight()
    {
        Light.SetActive(!Light.activeSelf);
    }

}
