using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SH_Sound : MonoBehaviour
{
    AudioSource source;
    public AudioClip clip;

    public int repeat;
    int repeatCount;

    [Range(0,1f)]
    public float volume;

    private void Awake()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
            if(source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    private void OnEnable()
    {
        repeatCount = repeat;
    }

    private void Update()
    {
        if (source.isPlaying)
            return;

        if (repeatCount > 0)
            Active();
        else
            gameObject.SetActive(false);
    }

    public void Active()
    {
        if (clip == null)
        {
            Debug.Log("Clip is NULL");
        }

        gameObject.SetActive(true);
        repeatCount--;
        source.volume = volume;
        source.PlayOneShot(clip);
    }

}
