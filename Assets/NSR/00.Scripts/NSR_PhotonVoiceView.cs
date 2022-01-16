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
        transform.parent = NSR_AutoHandManager.instance.headCamera.transform;

        photonVoice = GetComponent<PhotonVoiceView>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            NSR_AutoHandManager.instance.recoderImageInTV.enabled = photonVoice.IsRecording;
            NSR_AutoHandManager.instance.speakerImageInTV.enabled = photonVoice.IsSpeaking;
        }
        else
        {
            // ¹ÂÆ®
            if (Input.GetKeyDown(KeyCode.Alpha2) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft))
                audioSource.enabled = !audioSource.enabled;
        }
    }
}
