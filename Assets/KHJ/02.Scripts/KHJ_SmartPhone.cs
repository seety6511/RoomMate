using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KHJ_SmartPhone : MonoBehaviour
{
    public GameObject AppBG;
    public GameObject[] Apps;
    public bool IsRunningApp;
    public KHJ_App RunningApp;
    public enum AppName
    {
        Caculator,
        Camera,
        Clock,
        Calendar,
        Music,
        Record,
        Web,
        Memo,
        Contact,
        Map,
        Wifi,
        Light,
        Settings,
        Mail,
        Phone,
    }
    void Update()
    {
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        if (Input.GetButtonDown("Fire1"))
        {
            //Ray ray = new Ray(trRight.position, trRight.forward);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("O"+ray.origin);
            Debug.Log("M"+Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                Debug.Log("H" + hitInfo.point);
                //키패드 클릭 실행
                if (hitInfo.collider.name.Contains("App"))
                {
                    StartApp(hitInfo.collider.gameObject);
                }
                else if (hitInfo.collider.name.Contains("HomeButton"))
                {
                    if (IsRunningApp)
                    {
                        EndApp();
                    }
                }
                else if (hitInfo.collider.name.Contains("Toggle"))
                {
                    hitInfo.collider.GetComponent<KHJ_Toggle>().OnTrigger();
                }
                Set_smartphone();
            }
        }        
    }

    void Set_smartphone()
    {
        if (IsRunningApp)
        {
            foreach(GameObject obj in Apps)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject obj in Apps)
            {
                obj.SetActive(true);
            }
        }
    }


    float duration = 0.4f;
    void StartApp(GameObject obj)
    {
        AppBG.GetComponent<Image>().DOFade(1, duration).SetAutoKill(false).SetEase(Ease.InOutQuad).Pause();
        AppBG.transform.DOScale(1, duration).SetAutoKill(false).SetEase(Ease.InOutCirc).Pause();
        DOTween.Play(AppBG.GetComponent<Image>());
        DOTween.Play(AppBG.transform);
        IsRunningApp = true;
        obj.GetComponent<KHJ_App>().App.SetActive(true);
        RunningApp = obj.GetComponent<KHJ_App>();
    }
    void EndApp()
    {
        AppBG.GetComponent<Image>().DOFade(0, duration).SetAutoKill(false).SetEase(Ease.InOutQuad).Pause();
        AppBG.transform.DOScale(0.2f, duration).SetAutoKill(false).SetEase(Ease.InOutCirc).Pause();
        DOTween.Play(AppBG.GetComponent<Image>());
        DOTween.Play(AppBG.transform);
        IsRunningApp = false;
        RunningApp.App.SetActive(false);
        RunningApp = null;
    }
    void Switch_App(AppName name)
    {
        switch (name)
        {
            case AppName.Caculator:
                break;
            case AppName.Camera:
                break;
            case AppName.Clock:
                break;
            case AppName.Calendar:
                break;
            case AppName.Music:
                break;
            case AppName.Record:
                break;
            case AppName.Web:
                break;
            case AppName.Memo:
                break;
            case AppName.Contact:
                break;
            case AppName.Map:
                break;
            case AppName.Wifi:
                break;
            case AppName.Light:
                break;
            case AppName.Settings:
                break;
            case AppName.Mail:
                break;
            case AppName.Phone:
                break;
        }
    }

}
