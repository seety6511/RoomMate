using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
public class KHJ_ScreenFade : MonoBehaviour
{
    //포스트 프로세싱
    [Tooltip("Fade duration")]
    public Volume volume;
    DepthOfField depth;                 //end 0 -> 30
    //UI
    public Image Up;
    public Image Down;


    private void Start()
    {
        volume.profile.TryGet<DepthOfField>(out depth);
    }
    void OnEnable()
    {
        //초기설정
        Up.transform.localPosition = new Vector3(0, 5, 0);
        Down.transform.localPosition = new Vector3(0, -5, 0);
        FadeIn();
    }
    public void FadeIn()
    {
        StartCoroutine(Fade());
    }
	IEnumerator Fade()
	{
        yield return new WaitForSeconds(1f);
        Up.transform.DOLocalMoveY(6f, 1.8f).SetEase(Ease.OutQuart);
        Down.transform.DOLocalMoveY(-6f, 1.8f).SetEase(Ease.OutQuart);
        DOTween.To(() => depth.gaussianEnd.value, x => depth.gaussianEnd.value = x, 120f, 8f).SetEase(Ease.InExpo);
        yield return new WaitForSeconds(1.8f);
        Up.transform.DOLocalMoveY(5f, 0.5f).SetEase(Ease.OutQuart);
        Down.transform.DOLocalMoveY(-5f, 0.5f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.5f);
        DOTween.To(() => depth.gaussianStart.value, x => depth.gaussianStart.value = x, 2f, 4f).SetEase(Ease.InExpo);
        Up.transform.DOLocalMoveY(10f, 6f).SetEase(Ease.OutQuart);
        Down.transform.DOLocalMoveY(-10f, 6f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(6f);
        depth.active = false;
    }
}
