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


    //오른손 Transform
    public Transform trRight;
    //잡은 물체의 Transform
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
                //부딪힌 물체를 오른손의 자식으로 한다
                NowItem.transform.parent = trRight;
                NowItem.transform.position = trRight.position;
                NowItem.transform.rotation = trRight.rotation;
                //잡은 물체를 trcatched에 넣어둔다
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
                //이미 리스트에 있는 경우엔 획득 X
                if(Items.Contains(hitInfo.transform.gameObject))
                {
                    hitInfo.transform.parent = null;
                    Items.Remove(hitInfo.transform.gameObject);
                    hitInfo.transform.localScale = hitInfo.transform.GetComponent<InvenItem>().InitSize;
                    NowItem = hitInfo.transform.gameObject;
                    return;
                }
                //리스트에 없는 경우 획득
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

        //2개 이상일 때
        for (int i = 1; i < Items.Count; i++)
        {
            Items[i].GetComponent<Rigidbody>().isKinematic = true;
            Items[i].transform.localPosition = new Vector3(r * Mathf.Cos((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)), 
                0, r * Mathf.Sin((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)));
        }
    }
    void DropObj()
    {
        //1. 만약에 오른쪽 그랩버튼을 놓으면
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (trCatchedR != null)
            {
                Throwobj(trCatchedR, OVRInput.Controller.RTouch);
                //2. 잡은 물체를 놓는다.(잡은 물체의 부모를 없앤다)
                trCatchedR.parent = null;
                //3. trCatced의 값을 null로 한다
                trCatchedR = null;
            }
        }
    }
    public float throwPower = 5;
    void Throwobj(Transform obj, OVRInput.Controller controller)
    {
        //던지는 방향 (이동속력)
        Vector3 dir = OVRInput.GetLocalControllerVelocity(controller);
        // 던지는 회전방향 (회전속력)
        Vector3 angularDir = OVRInput.GetLocalControllerAngularVelocity(controller);
        //잡은 물체에 붙어있는 RIgidbody를 가져오자
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        //Velocity 값에 dir을 넣자
        rb.velocity = dir * throwPower;
        // angularVelocity에 angulardir를 넣자
        rb.angularVelocity = angularDir;
    }
    void SetKinematic(bool enable, Transform catchObj)
    {
        // 잡은 물체에서 Rigidbody 컴포넌트 가져온다
        Rigidbody rb = catchObj.GetComponent<Rigidbody>();
        // 가져온 컴포넌트의 isKinematic을 false한다
        rb.isKinematic = enable;
        //true 하면 물리X false하면 물리O
    }
}
