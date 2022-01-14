using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class KHJ_Door : MonoBehaviour
{
    public static KHJ_Door instance;
    public bool isOpened;
    public Text[] Inputs;

    public bool able;

    public AudioClip clickSound;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    AudioSource source;

    public string Answer;
    public Text AnswerText;
    public List<int> AnswerList;
    public List<int> ButtonInputList;
    public Text under;
    public string InputString = "";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
        AnswerText.text = Answer;
        //힌트에 있는 답을 AnswerList에 추가
        for (int i = 0; i < Answer.ToString().Length; i++)
        {
            AnswerList.Add(int.Parse(Answer[i].ToString()));
        }
        able = true;
        ClearBtn();
    }
    public bool CheckAnswer()
    {
        bool check = true;
        for(int i = 0; i < AnswerList.Count; i++)
        {
            if(AnswerList[i] != ButtonInputList[i])
            {
                check = false;
                break;
            }
        }
        return check;
    }
    public void ClearBtn()
    {
        InputString = "";
        ButtonInputList.Clear();
        foreach(var text in Inputs)
        {
            text.text = "";
        }
    }
    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}