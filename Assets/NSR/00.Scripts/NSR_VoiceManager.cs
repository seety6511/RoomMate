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
        // ����ũ �ѱ�
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
        // ��ü���� ���� �ѱ�
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (AudioListener.volume == 0)
                AudioListener.volume = volume;
            else
                AudioListener.volume = 0;
        }
    }
}
