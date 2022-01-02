using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSR_Button : MonoBehaviour
{
    public int num;
    NSR_DoorLock door;
    public NSR_Connect connect;

    public enum BTN 
    {
        num,
        del,
        enter
    }

    public BTN btn;

    private void Start()
    {
        door = gameObject.GetComponentInParent<NSR_DoorLock>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;

        switch (btn)
        {
            case BTN.num:
                NumBtn();
                break;

            case BTN.del:
                DelBtn();
                break;

            case BTN.enter:
                EnterBtn();
                break;
        }
    }

    void NumBtn()
    {
        if (door.i < door.inputNums.Length)
        {
            door.inputNums[door.i].text = num.ToString();
            door.i++;
        }
    }

    void DelBtn()
    {
        for(int i = 0; i < door.inputNums.Length; i++)
        {
            door.inputNums[i].text = "";
        }

        door.i = 0;
        connect.answer = "";
        connect.joinFailText.SetActive(false);
    }

    void EnterBtn()
    {
        if (door.i >= door.inputNums.Length)
        {
            for (int i = 0; i < door.inputNums.Length; i++)
            {
                connect.answer += door.inputNums[i].text;
            }
            connect.enterBtn = true;
        }
    }
}
