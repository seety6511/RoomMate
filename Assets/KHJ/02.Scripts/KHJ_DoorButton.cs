using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KHJ_DoorButton : MonoBehaviour
{
    KHJ_Door door;
    public int num;
    void Start()
    {
        door = KHJ_Door.instance;
    }
    public void ClickButton()
    {
        //�Է°� �ֱ�
        door.ButtonInputList.Add(num);
        door.Inputs[door.ButtonInputList.Count - 1].text = num.ToString();

        BtnInputEft();
        //��й�ȣ ���̸�ŭ �Է��� �����ٸ� Ȯ��
        if (door.ButtonInputList.Count == door.AnswerList.Count)
        {
            door.able = false;
            StartCoroutine(waitforAnswerCheck());
        }        
    }
    IEnumerator waitforAnswerCheck()
    {
        yield return new WaitForSeconds(1);
        if (door.CheckAnswer())
        {
            //������ ����
            door.PlaySound(door.correctSound);
            door.ClearBtn();
            door.under.text = "Clear";
            door.isOpened = true;
        }
        else
        {
            //Ʋ���� �ʱ�ȭ
            door.PlaySound(door.incorrectSound);
            door.ClearBtn();
            door.able = true;
        }
    }
    public void BtnInputEft()
    {
        //��ư Ŭ����
        door.PlaySound(door.clickSound);
    }
}
