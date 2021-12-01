using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GetItem();
        SetItem();
        int layer = 1 << LayerMask.NameToLayer("GainItem");
        //Ray ray = new Ray(trRight.position, trRight.forward);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
        {
            SelectedItem = hitInfo.transform.gameObject;
        }
        else
        {
            SelectedItem = null;
        }
        OnTouch();
        OnOffInven();
    }
    void OnOffInven()
    {
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        if (Input.GetButtonDown("Fire1"))
        {
            //Ray ray = new Ray(trRight.position, trRight.forward);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        //if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        if (Input.GetButton("Fire1"))
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

                //Vector3 mousePos = Input.mousePosition;
                //mousePos.z = Camera.main.nearClipPlane * offset;
                //NowItem.transform.localPosition = Camera.main.ScreenToWorldPoint(mousePos);
            }
        }
        else
        {
            if (NowItem != null)
            {
                NowItem.transform.parent = null;
                NowItem.GetComponent<Rigidbody>().isKinematic = false;
                NowItem = null;
            }
        }
    }
    void GetItem()
    {
        int layer = 1 << LayerMask.NameToLayer("GainItem");
        //if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        if (Input.GetButtonDown("Fire1"))
        {
            //Ray ray = new Ray(trRight.position, trRight.forward);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layer))
            {
                //�̹� ����Ʈ�� �ִ� ��쿣 ȹ�� X
                if(Items.Contains(hitInfo.transform.gameObject))
                {
                    hitInfo.transform.parent = null;
                    Items.Remove(hitInfo.transform.gameObject);
                    hitInfo.transform.localScale = hitInfo.transform.GetComponent<InvenItem>().InitSize;
                    NowItem = hitInfo.transform.gameObject;
                    return;
                }
                //����Ʈ�� ���� ��� ȹ��
                hitInfo.transform.parent = Pivot.transform;
                hitInfo.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                hitInfo.transform.localScale = Vector3.one * hitInfo.transform.GetComponent<InvenItem>().size_value;

                Items.Add(hitInfo.transform.gameObject);
            }
        }
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
    void DropObj()
    {
        //1. ���࿡ ������ �׷���ư�� ������
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
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
