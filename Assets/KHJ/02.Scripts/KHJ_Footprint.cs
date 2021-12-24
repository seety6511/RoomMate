using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KHJ_Footprint : MonoBehaviour
{
    public KHJ_TriggerNameCheck[] names;
    public bool hand_R;
    public bool hand_L;
    public bool foot;

    public bool isSolved = false;
    public Material material;
    public Material material2;
    void Start()
    {
        names = GetComponentsInChildren<KHJ_TriggerNameCheck>();
        material = Hand_R.GetComponent<Material>();
    }
    public void AnswerCheck()
    {
        if (isSolved)
            return;
        if (InputCheck())
        {
            SolveEffect();
        }
    }
    bool InputCheck()
    {
        hand_R = names[0].OnTouch;
        hand_L = names[1].OnTouch;
        foot = names[2].OnTouch;
        if (hand_R && hand_L)
        {
            return true;
        }
        else
            return false;
    }

    public GameObject Nextfoorpuzzle;
    public GameObject Hand_R;
    public GameObject Hand_L;
    public GameObject Foot_R;
    public GameObject Foot_L;
    void SolveEffect()
    {
        if(Nextfoorpuzzle != null)
        {
            Nextfoorpuzzle.SetActive(true);
            Hand_R.transform.DOMove(new Vector3(0, -3, 0), 2).SetRelative();
            Hand_L.transform.DOMove(new Vector3(0, -3, 0), 2).SetRelative();
            Foot_R.transform.DOMove(new Vector3(0, -3, 0), 2).SetRelative();
            Foot_L.transform.DOMove(new Vector3(0, -3, 0), 2).SetRelative();
        }
        else
        {
            KHJ_SceneManager_1.instance.disappearWall();
        }
        //Destroy(gameObject);
    }
}
