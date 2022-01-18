using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;
using Autohand;
public class SH_ChapterDoor : MonoBehaviour
{
    public Scene scene;
    public Modular3DText roomNumber;
    public Modular3DText roomName;
    public Grabbable handle;

    private void Awake()
    {
        roomName.UpdateText(scene.ToString());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateRoom();
    }

    public void CreateRoom()
    {
        print("CreateRoom : " + roomNumber.Text);
        FindObjectOfType<SH_ConnectManager>().CreateRoom(roomNumber.Text);
    }
}
