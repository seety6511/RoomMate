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
        //입력값 넣기
        door.ButtonInputList.Add(num);
        door.Inputs[door.ButtonInputList.Count - 1].text = num.ToString();

        BtnInputEft();
        //비밀번호 길이만큼 입력이 끝났다면 확인
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
            //맞으면 열림
            door.PlaySound(door.correctSound);
            door.ClearBtn();
            door.under.text = "Clear";
            door.isOpened = true;
            KHJ_SceneManager_1.instance.Scene1Clear();
        }
        else
        {
            //틀리면 초기화
            door.PlaySound(door.incorrectSound);
            door.ClearBtn();
            door.able = true;
        }
    }
    public void BtnInputEft()
    {
        //버튼 클릭음
        door.PlaySound(door.clickSound);
    }
}
