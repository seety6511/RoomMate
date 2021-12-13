using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class KHJ_SceneManager_1 : MonoBehaviour
{
    public static KHJ_SceneManager_1 instance;
    public Volume volume;
    FilmGrain film;                     //Intensity 0 -> 1
    ChromaticAberration chromatic;      //Intensity 0 -> 1
    ColorAdjustments color;             //colorFilter white -> blalck
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        volume.profile.TryGet<FilmGrain>(out film);
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
        volume.profile.TryGet<ColorAdjustments>(out color);
        StartCoroutine(Load());
    }
    public void Scene1Clear()
    {
        Debug.Log("GotoFirstScene");
        StartCoroutine(Load());
    }
    public GameObject CurvedUI;
    public AudioClip clip;
    IEnumerator Load()
    {
        DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        DOTween.To(() => film.intensity.value, x => film.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(3);
        GetComponent<AudioSource>().PlayOneShot(clip);
        GameObject gameObject = Instantiate(CurvedUI);
        gameObject.transform.position = GameObject.Find("Player").transform.position;
        gameObject.transform.rotation = GameObject.Find("Player").transform.rotation;
        yield return new WaitForSeconds(1.4f);
        DOTween.To(() => color.colorFilter.value, x => color.colorFilter.value = x, new Color32(0,0,0,0), 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(4f);

        var ao = SceneManager.LoadSceneAsync(0);
        while (!ao.isDone)
        {
            yield return null;
        }
    }
}
