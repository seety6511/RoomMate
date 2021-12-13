using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KHJ_DiaryButton : MonoBehaviour
{
    public Image bgImage;
    KHJ_Diary diary;
    public string num;
    void Start()
    {
        diary = KHJ_Diary.instance;
        bgImage = GetComponentInChildren<Image>();
    }
    /*not use
    public void ClickButton()
    {
        if(num == 0)
        {
            //초기화 버튼이면 초기화
            diary.ClearBtn();
            return;
        }
        //아니면 입력값 넣기
        if (!diary.ButtonInputList.Contains(num))
        {
            diary.ButtonInputList.Add(num);
            BtnInputEft();
            //비밀번호 길이만큼 입력이 끝났다면 확인
            if (diary.ButtonInputList.Count == diary.AnswerList.Count)
            {
                diary.able = false;
                StartCoroutine(waitforsecond());
            }
        }
    }*/

    /*not use
    IEnumerator waitforsecond()
    {
        yield return new WaitForSeconds(1);
        if (diary.CheckAnswer())
        {
            //맞으면 열림
            diary.PlaySound(diary.correctSound);
            diary.isOpened = true;
            diary.TurnPrePage();
        }
        else
        {
            //틀리면 초기화
            diary.PlaySound(diary.incorrectSound);
            diary.ClearBtn();
            diary.able = true;
        }
    }*/

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer != LayerMask.NameToLayer("Hand"))
    //        return;
    //    diary.Input(this);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        Debug.Log("A");
        diary.Input(this);
    }
    public void BtnInputEft()
    {
        //색 바뀌기
        bgImage.color = Color.gray;
        diary.PlaySound(diary.clickSound);
    }
    public void ClearButton()
    {
        bgImage.color = Color.white;
    }
}
