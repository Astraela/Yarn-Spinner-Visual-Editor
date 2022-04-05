using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    static public ContextMenu instance;
    public GameObject menu;

    Vector3 center;

    Box currentBox;

    void Start()
    {
        instance = this;
        center = new Vector3(Screen.width/2,Screen.height/2,0);
    }

    public void ShowUp(bool down,Box box){
        if(down) GetComponent<RectTransform>().pivot = new Vector2(0,0);
        else     GetComponent<RectTransform>().pivot = new Vector2(0,1);

        menu.transform.position = new Vector3(Input.mousePosition.x + center.x*2,Input.mousePosition.y,transform.position.z) - center;

        currentBox = box;
        menu.SetActive(true);
    }

    public void Hide(){
        menu.SetActive(false);
    }

    public void ButtonPress(string type){
        GameObject prefab = null;
        switch (type)
        {
            case "Dialogue":
                prefab = Instantiate(ServiceDesk.instance.GetItem("Dialogue"));
                break;
            case "Function":
                prefab = Instantiate(ServiceDesk.instance.GetItem("Function"));
                break;
            case "Option":
                prefab = Instantiate(ServiceDesk.instance.GetItem("Option"));
                break;
        }
        prefab.transform.SetParent(currentBox.transform);
        prefab.transform.SetSiblingIndex(currentBox.transform.childCount-2);
        Hide();
        Lines.Line parentLine = currentBox.GetComponentInParent<Lines.Line>();
        if(parentLine.type == Lines.LineType.Node){
            (parentLine as Lines.Node).AddLine(prefab.GetComponent<Lines.Line>());
        }else if(parentLine.type == Lines.LineType.Choice){
            (parentLine as Lines.Choice).AddLine(prefab.GetComponent<Lines.Line>());
        }
        prefab.GetComponent<Lines.Line>().lineParent = parentLine;
    }

}
