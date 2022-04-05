using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private bool dragging = false;

    private Vector3 offset;

    bool downEnough = false;
    float downEnoughCount = 0;
    float downEnoughTreshold = .5f;

    void Update(){
        if(dragging && !downEnough) downEnoughCount += Time.deltaTime;
        if(dragging && !downEnough && downEnoughCount >= downEnoughTreshold) downEnough = true;
        if(!dragging && !downEnough) return;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
    }


    public void OnDown(PointerEventData eventData){
        if(eventData.button != PointerEventData.InputButton.Left) return;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    public void OnUp(PointerEventData eventData){
        if(eventData.button != PointerEventData.InputButton.Left) return;
        if(downEnough == false)
            GetComponentInParent<Lines.Line>().OnPointerDown(null);
        downEnoughCount = 0;
        downEnough = false;
        dragging = false;
    }
}
