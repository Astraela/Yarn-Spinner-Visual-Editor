using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButton : MonoBehaviour
{
    public void buttonPress(){
        if(Input.mousePosition.y < Screen.width/2){
            ContextMenu.instance.ShowUp(false,GetComponentInParent<Box>());
        }else{
            ContextMenu.instance.ShowUp(true,GetComponentInParent<Box>());
        }
    }
}
