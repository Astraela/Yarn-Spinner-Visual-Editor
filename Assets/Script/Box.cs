using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Box : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float priority;

    float children;
    RectTransform rt;
    RectTransform rtParent;
    VerticalLayoutGroup group;

    private void Awake(){
        rt = GetComponent<RectTransform>();
        group = GetComponent<VerticalLayoutGroup>();
        rtParent = transform.parent.GetComponent<RectTransform>();
        children = transform.childCount;
        priority = GetComponentsInParent<Transform>().Length;
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
        
        if(transform.parent.GetComponentInParent<Box>())
            transform.parent.GetComponentInParent<Box>().UpdateSize();
        if(transform.parent.GetComponentInParent<ChoiceBox>())
            transform.parent.GetComponentInParent<ChoiceBox>().UpdateSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject currentLine = ServiceDesk.instance.GetItem("CurrentLine");
        if(currentLine != null){
            Line line = currentLine.GetComponent<Line>();
                currentLine.GetComponent<Line>().boxes.Remove(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject currentLine = ServiceDesk.instance.GetItem("CurrentLine");
        if(currentLine != null && !transform.IsChildOf(currentLine.transform))
            currentLine.GetComponent<Line>().boxes.Add(this);
    }

    private void OnTransformParentChanged(){
        priority = GetComponentsInParent<Transform>().Length;
    }
}
