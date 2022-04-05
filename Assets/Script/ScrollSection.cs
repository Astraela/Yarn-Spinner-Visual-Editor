using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollSection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Scrollbar scrollbar;
    bool scrollable = false;

    Movement _movement;

    Movement movement {get{
        if(_movement == null)
            _movement = FindObjectOfType<Movement>();
        return _movement;
    }}

    public void OnPointerEnter(PointerEventData eventData)
    {
        scrollable = true;
        movement.scrollable = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scrollable = false;
        movement.scrollable = true;
    }

    void Update()
    {
        if(scrollable && Input.mouseScrollDelta != Vector2.zero){
            scrollbar.value = Mathf.Clamp01(scrollbar.value - Input.mouseScrollDelta.y/5);
        }
    }
}
