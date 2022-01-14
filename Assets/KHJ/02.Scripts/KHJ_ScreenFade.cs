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


    private void Awake()
    {
        volume.profile.TryGet<DepthOfField>(out depth);
    }
    void OnEnable()
    {
        EyeOpen_();
    }
    public void EyeOpen_()
    {
        StartCoroutine(EyeOpen());
    }
	IEnumerator EyeOpen()
	{
        //초기설정
        depth.active = true;
        depth.gaussianEnd.value = 0;
        depth.gaussianStart.value = 0;
        Up.transform.localPosition = new Vector3(0, 5, 0);
        Down.transform.localPosition = new Vector3(0, -5, 0);
        //1초 뒤 눈뜨기
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
    public void EyeClose_()
    {
        //초기설정
        depth.active = true;
        depth.gaussianEnd.value = 0;
        depth.gaussianStart.value = 0;
        Up.transform.localPosition = new Vector3(0, 5, 0);
        Down.transform.localPosition = new Vector3(0, -5, 0);

        Up.transform.DOLocalMoveY(5f, 0.2f).SetEase(Ease.OutQuart);
        Down.transform.DOLocalMoveY(-5f, 0.2f).SetEase(Ease.OutQuart);
    }
}
