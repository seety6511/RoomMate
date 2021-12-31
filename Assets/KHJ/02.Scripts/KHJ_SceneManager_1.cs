using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using UnityEditor;

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
        //포스트프로세스 필드값들
        volume.profile.TryGet<FilmGrain>(out film);
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
        volume.profile.TryGet<ColorAdjustments>(out color);
    }
    public GameObject CurvedUIMenu;
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch) || OVRInput.GetDown(OVRInput.Button.Four, OVRInput.Controller.LTouch))
        {
            print("Menu");
            CurvedUIMenu.SetActive(!CurvedUIMenu.activeSelf);
            CurvedUIMenu.transform.position = Player.transform.position;
            CurvedUIMenu.transform.rotation = Player.transform.rotation;
        }
    }

    public GameObject IllusionWall;
    public void disappearWall()
    {
        IllusionWall.SetActive(false);
    }
    public void Scene1Clear()
    {
        Debug.Log("GotoFirstScene");
        StartCoroutine(Load());
    }
    public GameObject CurvedUI;
    public AudioClip clip;
    public GameObject Player;
    IEnumerator Load()
    {
        //엔딩 연출
        DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        DOTween.To(() => film.intensity.value, x => film.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(3);
        GetComponent<AudioSource>().PlayOneShot(clip);
        //챕터 1클리어 UI
        GameObject gameObject = Instantiate(CurvedUI);
        gameObject.transform.position = Player.transform.position;
        gameObject.transform.rotation = Player.transform.rotation;
        yield return new WaitForSeconds(1.4f);
        DOTween.To(() => color.colorFilter.value, x => color.colorFilter.value = x, new Color32(0,0,0,0), 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(4f);
    }
    public void GoToScene(string name)
    {
        //40324B
        //SH_Test 1
        //KHJ_Test
        if (name == "Quit")
        {
#if UNITY_EDITOR
            EditorApplication.Exit(0);
#elif UNITY_STANDALONE
            Application.Quit();
#endif
            return;
        }
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
}
