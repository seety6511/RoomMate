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
        }
        else
        {
            OutlineObj.GetComponent<Renderer>().material.SetColor("_OutlineColor", Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "InvenButton")
        {
            OnTrigger();
        }

        if (other.gameObject.layer != LayerMask.NameToLayer("Hand") || other.gameObject.name != "Tip")
            return;
        else
        {
            if (InvenManager.instance.Items.Contains(gameObject) && GetComponent<Grabbable>().beingGrabbed)
            {                
                print("Grab");
                GetComponent<Rigidbody>().isKinematic = false;
                transform.parent = null;
                InvenManager.instance.Items.Remove(gameObject);
                transform.localScale = InitSize;
                return;
            }
        }
    }
    private void OnTrigger()
    {
        GetComponent<Grabbable>().HandsRelease();
        InvenManager.instance.GetItem(gameObject);
    }
}
