using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    private bool dragging = false;

    private Vector3 offset;

    void Update(){
        if(dragging) 
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
    }


    public void OnDown(PointerEventData eventData){
        if(eventData.button != PointerEventData.InputButton.Left) return;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }

    public void OnUp(PointerEventData eventData){
        if(eventData.button != PointerEventData.InputButton.Left) return;
        dragging = false;
    }
}
