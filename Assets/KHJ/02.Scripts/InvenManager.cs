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
    void Update()
    {
        Pivot.transform.Rotate(new Vector3(0, PivotRotateSpeed, 0) * Time.deltaTime);
        SetItem();
        int layer = 1 << LayerMask.NameToLayer("GainItem");
        Ray ray = new Ray(trRight.position, trRight.forward);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            SelectedItem = hitInfo.transform.gameObject;
        }
        else
        {
            SelectedItem = null;
        }
        //OnTouch();
        //OnOffInven();
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

    public float offset;
    void OnTouch()
    {
        bool input;
        if (NSR_AutoHandManager.instance.isMaster)
            input = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        else
            input = NSR_AutoHandPlayer.instance.receive_input_R[0];
        if (input)
        {
            if (NowItem != null)
            {
                //    VR
                //�ε��� ��ü�� �������� �ڽ����� �Ѵ�
                NowItem.transform.parent = trRight;
                NowItem.transform.position = trRight.position;
                NowItem.transform.rotation = trRight.rotation;
                //���� ��ü�� trcatched�� �־�д�
                trCatchedR = NowItem.transform;
            }
        }
        else
        {
            if (NowItem != null)
            {
                NowItem.transform.parent = null;
                NowItem.GetComponent<Rigidbody>().isKinematic = false;
                if (NowItem.GetComponent<Grabbable>())
                    NowItem.GetComponent<Grabbable>().enabled = true;
                NowItem = null;
            }
        }
    }
    public void GetItem(GameObject getItem)
    {
        if (Items.Contains(getItem))
        {
            getItem.transform.parent = null;
            Items.Remove(getItem);
            getItem.transform.localScale = getItem.GetComponent<InvenItem>().InitSize;
            NowItem = getItem;
            return;
        }
        //����Ʈ�� ���� ��� ȹ��
        getItem.transform.parent = Pivot.transform;
        getItem.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        getItem.transform.localScale = Vector3.one * getItem.GetComponent<InvenItem>().size_value;

        Items.Add(getItem);
    }

    public float r = 2;
    void SetItem()
    {
        if (Items.Count == 0) return;       
        
        Items[0].transform.localPosition = new Vector3(-r, 0, 0);
        Items[0].GetComponent<Rigidbody>().isKinematic = true;
        if (Items[0].GetComponent<Grabbable>())
            Items[0].GetComponent<Grabbable>().enabled = false;
        if (Items.Count == 1) return;

        //2�� �̻��� ��
        for (int i = 1; i < Items.Count; i++)
        {
            if(Items[i].GetComponent<Grabbable>())
                Items[i].GetComponent<Grabbable>().enabled = false;
            Items[i].GetComponent<Rigidbody>().isKinematic = true;
            Items[i].transform.localPosition = new Vector3(r * Mathf.Cos((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)), 
                0, r * Mathf.Sin((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)));
        }
    }
    void DropObj()
    {
        //1. ���࿡ ������ �׷���ư�� ������
        bool input;
        if (NSR_AutoHandManager.instance.isMaster)
            input = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        else
            input = NSR_AutoHandPlayer.instance.receive_input_R[2];
        if (input)
        {
            if (trCatchedR != null)
            {
                Throwobj(trCatchedR, OVRInput.Controller.RTouch);
                //2. ���� ��ü�� ���´�.(���� ��ü�� �θ� ���ش�)
                trCatchedR.parent = null;
                //3. trCatced�� ���� null�� �Ѵ�
                trCatchedR = null;
            }
        }
    }
    public float throwPower = 5;
    void Throwobj(Transform obj, OVRInput.Controller controller)
    {
        if (NSR_AutoHandManager.instance.isMaster == false) return;

        //������ ���� (�̵��ӷ�)
        Vector3 dir = OVRInput.GetLocalControllerVelocity(controller);
        // ������ ȸ������ (ȸ���ӷ�)
        Vector3 angularDir = OVRInput.GetLocalControllerAngularVelocity(controller);
        //���� ��ü�� �پ��ִ� RIgidbody�� ��������
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        //Velocity ���� dir�� ����
        rb.velocity = dir * throwPower;
        // angularVelocity�� angulardir�� ����
        rb.angularVelocity = angularDir;
    }
    void SetKinematic(bool enable, Transform catchObj)
    {
        // ���� ��ü���� Rigidbody ������Ʈ �����´�
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        // ������ ������Ʈ�� isKinematic�� false�Ѵ�
        rb.isKinematic = enable;
        //true �ϸ� ����X false�ϸ� ����O
    }
}
