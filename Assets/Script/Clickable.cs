using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Clickable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent<PointerEventData> down;
    public UnityEvent<PointerEventData> up;

    public void OnPointerDown(PointerEventData eventData)
    {
        down.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        up.Invoke(eventData);
    }
}
