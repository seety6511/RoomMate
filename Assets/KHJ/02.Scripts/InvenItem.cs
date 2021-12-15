using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand;
public class InvenItem : MonoBehaviour
{
    public float size_value;
    public GameObject OutlineObj;
    public Vector3 InitSize;

    private void Start()
    {
        InitSize = transform.localScale;
    }
    void Update()
    {
        if(InvenManager.instance.SelectedItem == this.gameObject)
        {
            OutlineObj.SetActive(true);
        }
        else
        {
            OutlineObj.SetActive(false);
        }

        if (InvenManager.instance.Items.Contains(gameObject))
        {
            OutlineObj.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.blue);
            transform.localScale = Vector3.one * size_value;
        }
        else
        {
            OutlineObj.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
            transform.localScale = InitSize;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "InvenButton")
        {
            OnTrigger();
        }
    }
    private void OnTrigger()
    {
        GetComponent<Grabbable>().HandsRelease();
        InvenManager.instance.GetItem(gameObject);
    }
}
