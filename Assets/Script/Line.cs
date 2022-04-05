using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Line : MonoBehaviour
{
    public List<Box> boxes = new List<Box>();


    private bool dragging = false;
    private Vector3 offset;
    private RectTransform rt;
    private Transform canvas;

    Transform lastParent;
    int lastIndex;

    void Start(){
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>().transform;
    }

    bool downEnough = false;
    float downEnoughCount = 0;
    float downEnoughTreshold = .5f;
    void Update()
    {
        if(dragging && !downEnough) downEnoughCount += Time.deltaTime;
        if(dragging && !downEnough && downEnoughCount >= downEnoughTreshold) downEnough = true;
        if(!dragging && !downEnough) return;
        if(boxes.Count == 0 ){
            if(transform.parent != canvas)
                transform.SetParent(canvas,true);
            Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else{
            Box selectedBox = null;
            foreach(Box box in boxes){
                if(selectedBox == null || selectedBox.priority < box.priority)
                    selectedBox = box;
            }
            transform.SetParent(selectedBox.transform,false);
            int siblingIndex = 0;
            foreach (RectTransform sibling in selectedBox.transform)
            {
                if(sibling == rt) continue;
                if(sibling.name == "Button") continue;
                if(sibling.transform.position.y>Camera.main.ScreenToWorldPoint(Input.mousePosition).y){
                    siblingIndex++;
                }else break;
            }
            transform.SetSiblingIndex(siblingIndex);
            return;
        }
    }


    void PlaceLine(){
        if(boxes.Count == 0){
            transform.SetParent(lastParent,true);
            transform.SetSiblingIndex(lastIndex);
        }else{
            lastParent = transform.parent;
            lastIndex = transform.GetSiblingIndex();
        }
    }

    public void OnDown(PointerEventData eventData){
        if(eventData.button != PointerEventData.InputButton.Left) return;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ServiceDesk.instance.SetItem("CurrentLine",gameObject);
            lastParent = transform.parent;
            lastIndex = transform.GetSiblingIndex();
        boxes.Add(transform.parent.GetComponent<Box>());
            transform.SetParent(canvas,true);
        dragging = true;
        foreach(Image img in GetComponentsInChildren<Image>()){
            img.raycastTarget = false;
        }
    }

    public void OnUp(PointerEventData eventData){
        if(eventData.button != PointerEventData.InputButton.Left) return;
        downEnoughCount = 0;
        downEnough = false;
        ServiceDesk.instance.SetItem("CurrentLine",null);
        dragging = false;
            PlaceLine();
        foreach(Image img in GetComponentsInChildren<Image>()){
            img.raycastTarget = true;
        }
    }
}
