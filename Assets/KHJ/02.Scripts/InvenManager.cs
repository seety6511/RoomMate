using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenManager : MonoBehaviour
{
    public static InvenManager instance;
    public GameObject Pivot;
    public List<GameObject> Items;
    public GameObject SelectedItem;
    float PivotRotateSpeed = 10;

    public GameObject NowItem;
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
    }


    public float offset;
    void OnTouch()
    {
        if (Input.GetButton("Fire1"))
        {
            if (NowItem != null)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane * offset;

                NowItem.transform.localPosition = Camera.main.ScreenToWorldPoint(mousePos);
            }
        }
        else
        {
            NowItem = null;
        }
    }
    void GetItem()
    {
        int layer = 1 << LayerMask.NameToLayer("GainItem");
        if (Input.GetButtonDown("Fire1"))
        {
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
        if (Items.Count == 1) return;
      
        //2개 이상일 때
        for (int i = 1; i < Items.Count; i++)
        {
            Items[i].transform.localPosition = new Vector3(r * Mathf.Cos((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)), 
                0, r * Mathf.Sin((180 - (360.0f / Items.Count) * i) * (Mathf.PI / 180)));
        }
    }
}
