using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
public class KHJ_ScreenFade : MonoBehaviour
{
    [Tooltip("Fade duration")]
    public float fadeTime = 2.0f;
    public bool fadeOnStart = false;
    public Volume volume;
    Vignette vignette;                  //Intensity 1 -> 0
    DepthOfField depth;                 //end 0 -> 30
    private void Start()
    {
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<DepthOfField>(out depth);
    }
    void OnEnable()
    {
        FadeIn();
    }
    public void FadeIn()
    {
        StartCoroutine(Fade());
    }
	IEnumerator Fade()
	{
        yield return new WaitForSeconds(2f);
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.6f, 1.8f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(1.8f);
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 1.3f).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(1.3f);
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.4f, 2f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(2f);
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 1f, 1f).SetEase(Ease.InQuart);
        yield return new WaitForSeconds(1f);
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0f, 1.3f).SetEase(Ease.OutQuart);
    }
}
