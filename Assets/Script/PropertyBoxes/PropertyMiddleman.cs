using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyMiddleman : MonoBehaviour
{
    public GameObject node;
    public GameObject dialogue;
    public GameObject choice;
    public GameObject function;
    public GameObject reference;
    public GameObject background;
    public static PropertyMiddleman instance;
    public Lines.Line current;
    private void Start(){
        instance = this;
    }

    public void Hide(){
        node.SetActive(false);
        dialogue.SetActive(false);
        choice.SetActive(false);
        function.SetActive(false);
        reference.SetActive(false);
        background.SetActive(false);
    }

    public void TurnOn(Lines.Line line){
        current = line;
        node.SetActive(false);
        dialogue.SetActive(false);
        choice.SetActive(false);
        function.SetActive(false);
        reference.SetActive(false);

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
            case Lines.LineType.Function:
                function.SetActive(true);
                function.GetComponent<Properties.Function>().Show(line as Lines.Function);
                break;
            case Lines.LineType.Reference:
                reference.SetActive(true);
                reference.GetComponent<Properties.Reference>().Show(line as Lines.Reference);
                break;
        }
        background.SetActive(true);
    }
}
