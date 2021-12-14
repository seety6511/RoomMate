using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KHJ_PlayAudio : MonoBehaviour
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
        OnTrigger();
    }
    public void OnTrigger()
    {
        music.ChangeMusic(this);
    }
}
