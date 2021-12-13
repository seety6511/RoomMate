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
            //�ʱ�ȭ ��ư�̸� �ʱ�ȭ
            diary.ClearBtn();
            return;
        }
        //�ƴϸ� �Է°� �ֱ�
        if (!diary.ButtonInputList.Contains(num))
        {
            diary.ButtonInputList.Add(num);
            BtnInputEft();
            //��й�ȣ ���̸�ŭ �Է��� �����ٸ� Ȯ��
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
            //������ ����
            diary.PlaySound(diary.correctSound);
            diary.isOpened = true;
            diary.TurnPrePage();
        }
        else
        {
            //Ʋ���� �ʱ�ȭ
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
        //�� �ٲ��
        bgImage.color = Color.gray;
        diary.PlaySound(diary.clickSound);
    }
    public void ClearButton()
    {
        bgImage.color = Color.white;
    }
}
