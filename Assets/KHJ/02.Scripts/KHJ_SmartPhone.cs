using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(FadeIn(AppBG));
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(FadeOut(AppBG));
        }


        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        if (Input.GetButtonDown("Fire1"))
        {
            //Ray ray = new Ray(trRight.position, trRight.forward);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                //키패드 클릭 실행
                if (hitInfo.collider.name.Contains("App"))
                {
                    StartCoroutine(StartApp(hitInfo.collider.gameObject));
                }
                else if (hitInfo.collider.name.Contains("HomeButton"))
                {
                    if (IsRunningApp)
                    {
                        StartCoroutine(EndApp());
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

    IEnumerator StartApp(GameObject obj)
    {
        AppBG.SetActive(true);
        StartCoroutine(FadeIn(AppBG));
        IsRunningApp = true;
        RunningApp = obj.GetComponent<KHJ_App>();
        yield return new WaitForSeconds(0.1f);
        if (IsRunningApp)
            obj.GetComponent<KHJ_App>().App.SetActive(true);
    }
    IEnumerator EndApp()
    {
        IsRunningApp = false;
        RunningApp.App.SetActive(false);
        RunningApp = null;
        StartCoroutine(FadeOut(AppBG));
        yield return new WaitForSeconds(0.1f);
        AppBG.SetActive(false);
    }

    float time1 = 0f;
    float F_time1 = 0.2f;
    IEnumerator FadeIn(GameObject img)
    {
        Color alpha = img.GetComponent<Image>().color;
        alpha.a = 0;
        time1 = 0f;
        while (alpha.a < 1f)
        {
            print("FI "+alpha.a);
            time1 += Time.deltaTime / F_time1;
            alpha.a = Mathf.Lerp(0,1, time1);
            yield return null;
            img.GetComponent<Image>().color = alpha;
        }
    }
    IEnumerator FadeOut(GameObject img)
    {
        Color alpha = img.GetComponent<Image>().color;
        alpha.a = 1;
        time1 = 0f;
        while (alpha.a > 0f)
        {
            print("FO " + alpha.a);
            time1 += Time.deltaTime / F_time1;
            alpha.a = Mathf.Lerp(1,0, time1);
            yield return null;
            img.GetComponent<Image>().color = alpha;
        }
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
