using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NSR_GameManager : MonoBehaviourPunCallbacks
{
    static public NSR_GameManager instance;

    public int countPlayer = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
