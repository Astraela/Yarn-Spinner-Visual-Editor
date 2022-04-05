using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChoiceBox : MonoBehaviour
{
    public float priority;

    float children;
    RectTransform rt;
    RectTransform rtParent;
    VerticalLayoutGroup group;

    private void Start(){
        rt = GetComponent<RectTransform>();
        group = GetComponent<VerticalLayoutGroup>();
        rtParent = transform.parent.GetComponent<RectTransform>();
        children = transform.childCount;
        priority = GetComponentsInParent<Box>().Length;
    }
    private void Update(){
        if(children != transform.childCount){
            UpdateSize();
        }

        children = transform.childCount;
    }

    public void UpdateSize(){
        float size = -group.spacing;
        foreach(RectTransform child in transform){
            size += child.rect.height;
            size += group.spacing;
        }

        rtParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,rtParent.rect.height + (size-rt.rect.height));
        rtParent.ForceUpdateRectTransforms();
        //rtParent.sizeDelta += new Vector2(0,size-rt.sizeDelta.y);
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x,size);
        foreach(Box box in GetComponentsInParent<Box>()){
            if(transform.IsChildOf(box.transform) && priority == box.priority){
                box.UpdateSize();
            }
        }
        foreach(ChoiceBox box in GetComponentsInParent<ChoiceBox>()){
            if(box != this && transform.IsChildOf(box.transform) && priority == box.priority){
                box.UpdateSize();
            }
        }
    }
}
