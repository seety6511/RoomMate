using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class KHJ_MusicApp : MonoBehaviour
{
    public bool isPlaying;
    public Image NowPlayingButton;
    public Text NowPlaying;
    public Sprite[] sprites;    //0 = pause, 1 = play
    public KHJ_PlayAudio now;
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
    }
    private void Update()
    {
        if(now == null)
        {
            NowPlaying.text = "No AudioSource";
            NowPlayingButton.sprite = sprites[0];
            audioSource.clip = null;
            return;
        }

        if (isPlaying)
        {
            NowPlayingButton.sprite = sprites[1];
        }
        else
        {
            NowPlayingButton.sprite = sprites[0];
        }
    }

    public void ChangeMusic(KHJ_PlayAudio newMusic)
    {
        //일시정지 버튼
        if(newMusic.clip == null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                isPlaying = false;
            }
            else
            {
                audioSource.Play();
                isPlaying = true;
            }
            return;
        }

        //재생
        isPlaying = true;
        now = newMusic;
        NowPlaying.text = now.clipname;
        audioSource.clip = now.clip;
        audioSource.Play();
    }
    private void OnDisable()
    {
        now = null;
    }
}
