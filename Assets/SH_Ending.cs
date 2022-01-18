using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Autohand;

public class SH_Ending : MonoBehaviour
{
    [SerializeField]
    SH_SceneName sceneName;
    public Volume volume;
    FilmGrain film;                     //Intensity 0 -> 1
    ChromaticAberration chromatic;      //Intensity 0 -> 1
    ColorAdjustments color;             //colorFilter white -> blalck

    public GameObject CurvedUI;
    public AudioClip clip;
    public GameObject Player;
    public Transform PlayerCam;
    private void Awake()
    {
        //����Ʈ���μ��� �ʵ尪��
        volume.profile.TryGet<FilmGrain>(out film);
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
        volume.profile.TryGet<ColorAdjustments>(out color);
        Player = FindObjectOfType<AutoHandPlayer>().gameObject;
        PlayerCam = FindObjectOfType<OVRCameraRig>().gameObject.transform;
    }


    IEnumerator Ending()
    {
        //���� ����
        DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        DOTween.To(() => film.intensity.value, x => film.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(3);
        GetComponent<AudioSource>().PlayOneShot(clip);
        //é�� 1Ŭ���� UI
        CurvedUI.SetActive(true);
        CurvedUI.transform.position = Player.transform.position;
        CurvedUI.transform.rotation = PlayerCam.rotation;
        yield return new WaitForSeconds(1.4f);
        DOTween.To(() => color.colorFilter.value, x => color.colorFilter.value = x, new Color32(0, 0, 0, 0), 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(5f);

        GoToScene(sceneName.ToString());
    }
    public void GoToScene(string name)
    {
        StartCoroutine(LoadScene(name));
    }
    IEnumerator LoadScene(string name)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Key"))
            return;

        StartCoroutine(Ending());
    }
}
