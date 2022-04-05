using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Deserializer
{
    enum state
    {
        NodeSearch = 1,
        Lines
    }

    private static Transform canvas;
    public static void Setup(Transform can){
        canvas = can;
    }

    static state currentState = state.NodeSearch;

    public static void DeSerialize(string file){
        var arr = file.Split("\n");

        for (int i = 0; i < arr.Length; i++)
        {
            switch(currentState){
                case state.NodeSearch:
                    NodeSearch(arr,i,arr[i]);
                    break;
                case state.Lines:
                    LineSearch(arr,i,arr[i]);
                    break;
            }
        }
    }

    //State Functions

    private static Lines.Node currentNode = null;
    private static void NodeSearch(string[] arr, int index, string line){
        if(line.StartsWith("---")){
            string title = FindTitle(arr,index);
            CreateNode(title);
            currentState = state.Lines;
        }
    }

    private static Lines.Line currentParent = null;
        private static int choiceIndex = 0;
    private static Lines.Dialogue lastDialogue = null;
    private static int CurrentIndentation = 0;
    private static void LineSearch(string[] arr, int index, string line){
        if(line.Replace("/t","").StartsWith("->")){
            //Do CHOICE THING IDFK
            int indentation = line.Length - line.Replace("/t","").Length;
            if(currentParent.type == Lines.LineType.Choice && indentation == CurrentIndentation){
                choiceIndex++;
                (currentParent as Lines.Choice).AddChoice(line.Replace("/t","").Replace("-> ","").Replace("->",""));
                
            }else{
                currentParent = CreateChoice(line.Replace("/t","").Replace("-> ","").Replace("->",""));
                CurrentIndentation = indentation;
            }
        }else if(line.Replace("/t","").StartsWith("===")){
            currentState = state.NodeSearch;
        }else{
            //Treat as DIALOGUE
            if(currentParent.type == Lines.LineType.Choice){
                int indentation = line.Length - line.Replace("/t","").Length;
                if(indentation == CurrentIndentation + 1){
                    if(lastDialogue == null){
                        lastDialogue = CreateDialogue();
                    }
                    lastDialogue.dialogueLines += (line.Replace("/t","") + "\n");
                }else{
                    while(CurrentIndentation != indentation){
                        currentParent = currentParent.lineParent;
                        CurrentIndentation -= 1;
                    }
                    lastDialogue = CreateDialogue();
                    lastDialogue.dialogueLines += (line.Replace("/t","") + "\n");
                }
            }else{
                if(lastDialogue == null){
                    lastDialogue = CreateDialogue();
                }
                lastDialogue.dialogueLines += (line.Replace("/t","") + "\n");
            }
            return;
        }
        lastDialogue = null;
    }

    //Create Functions
    private static void CreateNode(string title){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Node"));
        newNode.transform.SetParent(canvas,false);
        newNode.GetComponent<Lines.Node>().title = title;
        currentNode = newNode.GetComponent<Lines.Node>();
        currentParent = currentNode;
    }

    private static Lines.Dialogue CreateDialogue(){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Dialogue"));
        newNode.transform.SetParent(canvas);
        var dialog = newNode.GetComponent<Lines.Dialogue>();
        if(currentParent.type == Lines.LineType.Choice){
            var currentBox = (currentParent as Lines.Choice).choices[choiceIndex].option.GetComponentInChildren<Box>();
            dialog.transform.SetParent(currentBox.transform);
            dialog.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            (currentParent as Lines.Choice).AddLine(dialog);
            currentBox.UpdateSize();
        }else{
            var currentBox = (currentParent as Lines.Node).GetComponentInChildren<Box>();
            dialog.transform.SetParent(currentBox.transform);
            dialog.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            (currentParent as Lines.Node).AddLine(dialog);
            currentBox.UpdateSize();
        }
        
        return newNode.GetComponent<Lines.Dialogue>();
    }

    private static Lines.Choice CreateChoice(string choiceLine){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Option"));
        newNode.transform.SetParent(canvas);
        var choice = newNode.GetComponent<Lines.Choice>();
        if(currentParent.type == Lines.LineType.Choice){
            var currentBox = (currentParent as Lines.Choice).choices[choiceIndex].option.GetComponentInChildren<Box>();
            choice.transform.SetParent(currentBox.transform);
            choice.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            (currentParent as Lines.Choice).AddLine(choice);
            currentBox.UpdateSize();
        }else{
            var currentBox = (currentParent as Lines.Node).GetComponentInChildren<Box>();
            choice.transform.SetParent(currentBox.transform);
            choice.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            (currentParent as Lines.Node).AddLine(choice);
            currentBox.UpdateSize();
        }
        choice.choices[0].choiceString = choiceLine;
        return newNode.GetComponent<Lines.Choice>();
    }
    //Helper Functions
    private static string FindTitle(string[] arr, int index){
        for (int i = index; i >= 0; i--)
        {
            Debug.Log(arr[i]);
            if(arr[i].StartsWith("title: ")){
                return arr[i].Substring(("title: ").Length);
            }
        }
        throw new System.Exception("NoTitle");
    }
}
