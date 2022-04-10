using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Lines{

public enum LineType
{
    Node = 0,
    Dialogue = 1,
    Choice = 2,
    Function = 3,
    Reference = 4,
}


public abstract class Line : MonoBehaviour, IPointerDownHandler
{       
        public Line lineParent;
        public LineType type;

        public void OnPointerDown(PointerEventData eventData)
        {
            PropertyMiddleman.instance.TurnOn(this);
        }

        public abstract string Serialize(string indentation);
}

}
