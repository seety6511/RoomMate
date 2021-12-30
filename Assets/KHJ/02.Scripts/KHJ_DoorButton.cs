using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class KHJ_DoorButton : MonoBehaviourPun
{
    KHJ_Door door;
    public int num;
    void Start()
    {
        door = KHJ_Door.instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        if (KHJ_Door.instance.able)
            photonView.RPC("ClickButton", RpcTarget.All);
    }
    [PunRPC]
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
            KHJ_SceneManager_1.instance.Scene1Clear();
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
