using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
public class InvenManager : MonoBehaviour
{
    public static InvenManager instance;
    float PivotRotateSpeed = 10;
    public GameObject Pivot;
    public List<GameObject> Items;
    public GameObject SelectedItem;
    public GameObject NowItem;


    //������ Transform
    public Transform trRight;
    //���� ��ü�� Transform
    public Transform trCatchedR;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

   
    private void Update()
    {
        Pivot.transform.Rotate(new Vector3(0, PivotRotateSpeed, 0) * Time.deltaTime);
        SetItem();
        int layer = 1 << LayerMask.NameToLayer("GainItem");
        Ray ray = new Ray(trRight.position, trRight.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            SelectedItem = hitInfo.transform.gameObject;
        }
        else
        {
            SelectedItem = null;
        }
    }
    void OnOffInven()
    {
        //�κ��丮 ���� �״��ϱ�
        bool input;
        if (NSR_AutoHandManager.instance.isMaster)
            input = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        else
            input = NSR_AutoHandPlayer.instance.receive_input_R[1];
        if (input)
        {
            Ray ray = new Ray(trRight.position, trRight.forward);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue))
            {
                if (hitInfo.transform.name == "InvenButton")
                {
                    Pivot.SetActive(!Pivot.activeSelf);
                }
            }
        }
    }
    public void GetItem(GameObject getItem)
    {
        //����Ʈ�� ���� ��� ȹ��
        getItem.transform.parent = Pivot.transform;
        getItem.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        Items.Add(getItem);
    }

    public float r = 2;
    void SetItem()
    {
        if (Items.Count == 0) return;       
        
        Items[0].transform.localPosition = new Vector3(-r, 0, 0);
        Items[0].GetComponent<Rigidbody>().isKinematic = true;

        if (Items.Count == 1) return;

        //2�� �̻��� ��
        for (int i = 1; i < Items.Count; i++)
        {
            Items[i].GetComponent<Rigidbody>().isKinematic = true;
            Items[i].transform.localPosition = new Vector3(r * Mathf.Cos((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)), 
                0, r * Mathf.Sin((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)));
        }
    }
}
