using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }
        else
        {
            OutlineObj.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "InvenButton")
            return;
        OnTrigger();
    }
    private void OnTrigger()
    {
        InvenManager.instance.GetItem(gameObject);
    }

}
