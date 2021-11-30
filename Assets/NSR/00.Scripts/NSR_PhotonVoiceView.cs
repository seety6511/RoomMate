using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;


public class NSR_PhotonVoiceView : MonoBehaviourPun
{
    AudioSource audioSource;
    PhotonVoiceView photonVoice;
    void Start()
    {
        transform.parent = NSR_PlayerManager.instance.CenterEyeAnchor;

        photonVoice = GetComponent<PhotonVoiceView>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            NSR_PlayerManager.instance.recoderImageInTV.enabled = photonVoice.IsRecording;
            NSR_PlayerManager.instance.speakerImageInTV.enabled = photonVoice.IsSpeaking;
        }
        else
        {
            // ¹ÂÆ®
            if (Input.GetKeyDown(KeyCode.Alpha2))
                audioSource.enabled = !audioSource.enabled;
        }
    }
}
