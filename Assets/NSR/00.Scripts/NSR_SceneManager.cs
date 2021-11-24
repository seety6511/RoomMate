using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NSR_SceneManager : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(960, 640, false);
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("NSR_BodyPlayer", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("NSR_HandPlayer", new Vector3(15f, 1.6f, -2.56f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
