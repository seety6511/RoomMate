using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
[RequireComponent(typeof(AudioSource))]
public class KHJ_Footprint : MonoBehaviourPun
{
    public KHJ_TriggerNameCheck[] names;
    public BoxCollider[] boxes;
    //��, �߿� ��Ҵ��� Ȯ���ϴ� bool ����
    public bool hand_R;
    public bool hand_L;
    public bool foot;
    //Ŭ���� �ߴ��� Ȯ���ϴ� bool ����
    public bool isSolved = false;
    AudioSource source;
    public AudioClip clearSound;
    public SH_Hint HandPhone;
    void Start()
    {
        names = GetComponentsInChildren<KHJ_TriggerNameCheck>();
        boxes = GetComponentsInChildren<BoxCollider>();
        source = GetComponent<AudioSource>();
    }
    public void AnswerCheck()
    {
        if (isSolved)
            return;
        if (InputCheck())
        {
            if (PhotonNetwork.IsConnected)
                photonView.RPC("SolveEffect", RpcTarget.All);
            else
                SolveEffect();
            isSolved = true;
        }
    }
    bool InputCheck()
    {
        hand_R = names[0].OnTouch;
        hand_L = names[1].OnTouch;
        foot = names[2].OnTouch;
        if (hand_R && hand_L && foot)
        {
            return true;
        }
        else
            return false;
    }

    public GameObject Nextfoorpuzzle;
    public SkinnedMeshRenderer[] HandmeshRenderer;
    public MeshRenderer[] FootmeshRenderer;
    public Material TransparentMaterial_1;
    public Material TransparentMaterial_2;

    [PunRPC]
    void SolveEffect()
    {
        source.PlayOneShot(clearSound);
        if (Nextfoorpuzzle != null)
        {
            Nextfoorpuzzle.SetActive(true);
        }
        else
        {
            HandPhone.hasOn = false;
            KHJ_SceneManager_1.instance.disappearWall();
        }
        //�ݶ��̴� ���ֱ�
        foreach (BoxCollider box in boxes) box.enabled = false;

        //�չ� �������� ȿ��
        foreach (SkinnedMeshRenderer renderer in HandmeshRenderer)
        {
            renderer.material = TransparentMaterial_1;
            renderer.material.SetFloat("_FillPercent", 1.5f);
            renderer.material.DOFloat(0, "_FillPercent", 2);
        }
        foreach (MeshRenderer renderer in FootmeshRenderer)
        {
            renderer.material = TransparentMaterial_2;
            renderer.material.SetFloat("_FillPercent", 1.5f);
            renderer.material.DOFloat(-0.5f, "_FillPercent", 2);
        }
    }
}
