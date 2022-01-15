using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_FootSound : MonoBehaviour
{
    AudioSource audioSource;
    public List<AudioClip> clips;

    float currTime;
    public float interval;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        audioSource.PlayOneShot(clips[(int)Random.Range(0, clips.Count)]);
        currTime = 0;
    }

    private void OnEnable()
    {
        audioSource.PlayOneShot(clips[(int)Random.Range(0, clips.Count)]);
        currTime = 0;
    }
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime > interval)
        {
            audioSource.PlayOneShot(clips[(int)Random.Range(0, clips.Count)]);
            currTime = 0;
        }
    }
}
