using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;

public class NSR_VoiceManager : MonoBehaviour
{
    Recorder recorder;
    float volume;
    void Start()
    {
        recorder = GetComponent<Recorder>();
        volume = AudioListener.volume;
    }

    void Update()
    {
        // 마이크 켜고 끄기
        if (Input.GetKeyDown(KeyCode.Alpha1) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight))
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
        // 전체음향 켜고 끄기
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    if (AudioListener.volume == 0)
        //        AudioListener.volume = volume;
        //    else
        //        AudioListener.volume = 0;
        //}
    }
}
