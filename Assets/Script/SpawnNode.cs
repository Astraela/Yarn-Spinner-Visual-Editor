using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNode : MonoBehaviour
{
    public Transform canvas;

    public void CreateNode(){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Node"));
        newNode.transform.SetParent(canvas,false);
        newNode.GetComponent<Lines.Node>().title = "Node "+ canvas.childCount;
        newNode.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,0));
        newNode.transform.position = newNode.transform.position - new Vector3(0,0,newNode.transform.position.z);
    }
}
