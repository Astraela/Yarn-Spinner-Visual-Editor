using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyMiddleman : MonoBehaviour
{
    public GameObject node;
    public GameObject dialogue;
    public GameObject choice;

    public static PropertyMiddleman instance;
    public Lines.Line current;
    private void Start(){
        instance = this;
    }

    public void Hide(){
        node.SetActive(false);
        dialogue.SetActive(false);
        choice.SetActive(false);
    }

    public void TurnOn(Lines.Line line){
        current = line;
        node.SetActive(false);
        dialogue.SetActive(false);
        choice.SetActive(false);

        switch (line.type)
        {
            case Lines.LineType.Node:
                node.SetActive(true);
                node.GetComponent<Properties.Node>().Show(line as Lines.Node);
                break;
            case Lines.LineType.Dialogue:
                dialogue.SetActive(true);
                dialogue.GetComponent<Properties.Dialogue>().Show(line as Lines.Dialogue);
                break;
            case Lines.LineType.Choice:
                choice.SetActive(true);
                choice.GetComponent<Properties.Choice>().Show(line as Lines.Choice);
                break;
        }
    }
}
