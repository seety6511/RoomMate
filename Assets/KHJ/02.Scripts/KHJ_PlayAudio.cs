using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class KHJ_PlayAudio : MonoBehaviourPun
{
    public KHJ_MusicApp music;
    public AudioClip clip;
    public string clipname;

    private void Start()
    {
        music = GetComponentInParent<KHJ_MusicApp>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        photonView.RPC("OnTrigger", RpcTarget.All);
        //OnTrigger();
    }
    [PunRPC]
    public void OnTrigger()
    {
        music.ChangeMusic(this);
    }
}
