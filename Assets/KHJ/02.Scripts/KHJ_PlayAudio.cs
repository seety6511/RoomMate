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
    public void OnTrigger()
    {
        music.ChangeMusic(this);
    }
}
