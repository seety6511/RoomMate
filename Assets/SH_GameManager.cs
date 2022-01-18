using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Autohand;
public class SH_GameManager : MonoBehaviour
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
        //포스트프로세스 필드값들
        volume.profile.TryGet<FilmGrain>(out film);
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
        volume.profile.TryGet<ColorAdjustments>(out color);
        Player = FindObjectOfType<AutoHandPlayer>().gameObject;
        PlayerCam = FindObjectOfType<OVRCameraRig>().gameObject.transform;
    }

    public void Ending()
    {
        StartCoroutine(EndingEffect());
    }
    IEnumerator EndingEffect()
    {
        //엔딩 연출
        DOTween.To(() => chromatic.intensity.value, x => chromatic.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        DOTween.To(() => film.intensity.value, x => film.intensity.value = x, 1, 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(3);
        GetComponent<AudioSource>().PlayOneShot(clip);
        //챕터 1클리어 UI
        CurvedUI.SetActive(true);
        CurvedUI.transform.position = Player.transform.position;
        CurvedUI.transform.rotation = PlayerCam.rotation;
        yield return new WaitForSeconds(1.4f);
        DOTween.To(() => color.colorFilter.value, x => color.colorFilter.value = x, new Color32(0, 0, 0, 0), 3).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(5f);

        LoadScene(sceneName.ToString());
    }
    public void LoadScene(string name)
    {
        StartCoroutine("AsyncLoadScene", name);
    }
    IEnumerator AsyncLoadScene(string name)
    {
        var ao = SceneManager.LoadSceneAsync(name);
        while (!ao.isDone)
        {
            yield return null;
        }
    }
}
