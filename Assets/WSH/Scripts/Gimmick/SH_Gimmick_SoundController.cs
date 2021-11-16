using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SH_Gimmick_SoundController : MonoBehaviour
{
    SH_Gimmick parent;
    public AudioClip activeSound;
    public AudioClip activaingSound;
    public AudioClip disableSound;
    public AudioClip waitingSound;
    public AudioClip hoveringSound;
    public AudioClip clearSound;
    public AudioClip reloadSound;

    AudioSource source;
    public void Init()
    {
        parent = GetComponent<SH_Gimmick>();
        source = GetComponent<AudioSource>();
    }

    void ClipPlay(AudioClip clip, bool continues = false)
    {
        if (clip == null)
            return;

        if (continues)
        {
            if (source.isPlaying)
                return;
        }
        source.PlayOneShot(activeSound);
    }

    public void StateUpdate()
    {
        switch (parent.gimmickState)
        {
            case SH_GimmickState.Active:
                ClipPlay(activeSound);
                break;

            case SH_GimmickState.Activating:
                ClipPlay(activaingSound, true);
                break;

            case SH_GimmickState.Waiting:
                ClipPlay(waitingSound, true);
                break;

            case SH_GimmickState.Disable:
                ClipPlay(disableSound);
                break;

            case SH_GimmickState.Hovering:
                ClipPlay(hoveringSound, true);
                break;

            case SH_GimmickState.Clear:
                ClipPlay(clearSound);
                break;

            case SH_GimmickState.Reload:
                ClipPlay(reloadSound);
                break;

            case SH_GimmickState.None:
                break;
        }
    }

    public void VolumeControl(float value)
    {
        value = Mathf.Clamp(value, 0, 1f);
        source.volume = value;
    }
}
