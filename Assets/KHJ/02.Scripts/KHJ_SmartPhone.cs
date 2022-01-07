using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[RequireComponent(typeof(AudioSource))]
public class KHJ_SmartPhone : MonoBehaviour
{
    public static KHJ_SmartPhone instance;
    public GameObject AppBG;
    public GameObject[] Apps;
    public bool IsRunningApp;
    public bool IsSolved;
    public bool IsAlarmEnd = false;
    public bool IsTouching;
    public KHJ_App RunningApp;
    AudioSource source;
    public AudioClip openSound;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
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
    //오른손 Transform
    public Transform trRight;
    //잡은 물체의 Transform
    public Transform trCatchedR;

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
    //화면 터치시 터치된 좌표
    public Transform TouchPos;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        IsTouching = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        IsTouching = true;
        TouchPos = other.transform;
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        IsTouching = false;
        TouchPos = null;
    }
    public GameObject tmp;
    void Update()
    {
        if (!Pattern1.gameObject.activeSelf)
        {
            IsSolved = true;
        }
        Set_smartphone();
        if (IsTouching)
        {
            if (!IsAlarmEnd)
                return;
            tmp.SetActive(true);
            tmp.transform.position = TouchPos.position;
        }
        else
        {
            tmp.SetActive(false);
        }
    }

    public KHJ_Pattern Pattern1;
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
    public void StartApp(GameObject obj)
    {
        source.PlayOneShot(openSound);
        AppBG.GetComponent<Image>().DOFade(1, duration).SetAutoKill(false).SetEase(Ease.InOutQuad).Pause();
        AppBG.transform.DOScale(1, duration).SetAutoKill(false).SetEase(Ease.InOutCirc).Pause();
        DOTween.Play(AppBG.GetComponent<Image>());
        DOTween.Play(AppBG.transform);
        IsRunningApp = true;
        obj.GetComponent<KHJ_App>().App.SetActive(true);
        RunningApp = obj.GetComponent<KHJ_App>();
    }
    public void EndApp()
    {
        source.PlayOneShot(openSound);
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
    public GameObject AlarmObj;
    public void EndAlarm()
    {
        StartCoroutine(EndAlarm_());
        AlarmObj.GetComponentInChildren<Image>().DOFade(0, duration).SetAutoKill(false).SetEase(Ease.InOutQuad).Pause();
        AlarmObj.transform.DOScale(0.2f, duration).SetAutoKill(false).SetEase(Ease.InOutCirc).Pause();
        DOTween.Play(AlarmObj.GetComponentInChildren<Image>());
        DOTween.Play(AlarmObj.transform);
    }
    IEnumerator EndAlarm_()
    {
        yield return new WaitForSeconds(0.4f);
        AlarmObj.SetActive(false);
        IsAlarmEnd = true;
    }
}
