using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Movement : MonoBehaviour
{
    Vector3 lastMousePos;
    Vector3 MouseDelta;

    public bool scrollable = true;
    public float CamSpeed = 10;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse2)){
            lastMousePos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
            ContextMenu.instance.Hide();
        }
        if(Input.GetKey(KeyCode.Mouse2)){
            float size = Camera.main.orthographicSize;
            Vector3 movement = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            MouseDelta = movement - lastMousePos;
            transform.position += -MouseDelta * (size/540);
            lastMousePos = movement;
        }
        if(scrollable && Input.mouseScrollDelta != Vector2.zero){
            ContextMenu.instance.Hide();
            float size = Camera.main.orthographicSize;
            if(size + Input.mouseScrollDelta.y * 4 < 30) return;
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 4;
            var mousePos = Input.mousePosition;
            mousePos. x -= Screen. width/2;
            mousePos. y -= Screen. height/2;
            transform.position += (mousePos/55) * Input.mouseScrollDelta.y;
        }
        
        if(Input.GetAxis("Horizontal") != 0){
            if(EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null){
                ContextMenu.instance.Hide();
                float size = Camera.main.orthographicSize;
                transform.position += Vector3.right * Input.GetAxis("Horizontal") * (size/540) * CamSpeed;
            }
        }
        if(Input.GetAxis("Vertical") != 0){
            if(EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null){
                ContextMenu.instance.Hide();
                float size = Camera.main.orthographicSize;
                transform.position += Vector3.up * Input.GetAxis("Vertical") * (size/540) * CamSpeed;
            }
        }
    }
}
