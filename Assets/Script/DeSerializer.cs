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
        Random.InitState((int)Time.time);
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

    private static Lines.Line LastLine = null;
    private static Lines.Line currentParent = null;
    private static Lines.Dialogue lastDialogue = null;
    private static int CurrentIndentation = 0;
    private static void LineSearch(string[] arr, int index, string line){
        if(line.TrimStart('\t').StartsWith("->")){
            //Do CHOICE THING IDFK
            int indentation = line.Length - line.TrimStart('\t').Length;
            if(currentParent.type == Lines.LineType.Choice && indentation == CurrentIndentation-1){
                (currentParent as Lines.Choice).AddChoice(line.TrimStart('\t').Replace("-> ","").Replace("->",""));
            }else if(indentation == CurrentIndentation){
                currentParent = CreateChoice(line.TrimStart('\t').Replace("-> ","").Replace("->",""));
                CurrentIndentation = indentation+1;
            }else if(indentation < CurrentIndentation){
                while(indentation != CurrentIndentation-1){
                    currentParent = currentParent.lineParent;
                    CurrentIndentation -= 1;
                }
                (currentParent as Lines.Choice).AddChoice(line.TrimStart('\t').Replace("-> ","").Replace("->",""));
            }
        }else if(line.TrimStart('\t').StartsWith("===")){
            currentState = state.NodeSearch;
        }else if(line.TrimStart('\t').StartsWith("<<call ")){
            lastDialogue = null;
            CreateFunction(line.TrimStart('\t').Substring(7).TrimEnd('>'));            
        }else if(line.TrimEnd('\t').StartsWith("[[")){
            lastDialogue = null;
            CreateReference(line.TrimStart('\t','[').TrimEnd(']'));
        }else{
            //Treat as DIALOGUE
            if(currentParent.type == Lines.LineType.Choice){
                int indentation = line.Length - line.TrimStart('\t').Length;
                if(indentation == CurrentIndentation){
                    if(lastDialogue == null){
                        lastDialogue = CreateDialogue();
                    }
                    lastDialogue.dialogueLines += (line.TrimStart('\t') + "\n");
                }else{
                    while(CurrentIndentation != indentation){
                        currentParent = currentParent.lineParent;
                        CurrentIndentation -= 1;
                    }
                    lastDialogue = CreateDialogue();
                    lastDialogue.dialogueLines += (line.TrimStart('\t') + "\n");
                }
            }else{
                if(lastDialogue == null){
                    lastDialogue = CreateDialogue();
                }
                lastDialogue.dialogueLines += (line.TrimStart('\t') + "\n");
            }
            return;
        }
        lastDialogue = null;
    }

    //Create Functions
    private static void CreateNode(string title){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Node"));
        newNode.transform.SetParent(canvas,false);
        Vector3 center = new Vector3(Random.Range(Screen.width*.25f,Screen.width*.75f),Random.Range(Screen.width*.2f,Screen.width*.9f),0);
        newNode.transform.position = Camera.main.ScreenToWorldPoint(center);
        newNode.transform.position = newNode.transform.position - new Vector3(0,0,newNode.transform.position.z);
        newNode.GetComponent<Lines.Node>().title = title;
        currentNode = newNode.GetComponent<Lines.Node>();
        currentParent = currentNode;
    }

    private static void CreateFunction(string func){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Function"));
        var function = newNode.GetComponent<Lines.Function>();
        function.func = func;
        newNode.transform.SetParent(canvas);
        if(currentParent.type == Lines.LineType.Choice){
            var choice = currentParent as Lines.Choice;
            var currentBox = choice.choices[choice.choices.Count-1].option.GetComponentInChildren<Box>();
            newNode.transform.SetParent(currentBox.transform);
            newNode.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            choice.AddLine(function);
            currentBox.UpdateSize();
        }else{
            var currentBox = (currentParent as Lines.Node).GetComponentInChildren<Box>();
            newNode.transform.SetParent(currentBox.transform);
            newNode.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            (currentParent as Lines.Node).AddLine(function);
            currentBox.UpdateSize();
        }
    }

    private static Lines.Dialogue CreateDialogue(){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Dialogue"));
        newNode.transform.SetParent(canvas);
        var dialog = newNode.GetComponent<Lines.Dialogue>();
        if(currentParent.type == Lines.LineType.Choice){
            var choice = currentParent as Lines.Choice;
            var currentBox = choice.choices[choice.choices.Count-1].option.GetComponentInChildren<Box>();
            dialog.transform.SetParent(currentBox.transform);
            dialog.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            choice.AddLine(dialog);
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

    
    private static Lines.Reference CreateReference(string nodeName){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Reference"));
        newNode.transform.SetParent(canvas);
        var reference = newNode.GetComponent<Lines.Reference>();
        if(currentParent.type == Lines.LineType.Choice){
            var choice = currentParent as Lines.Choice;
            var currentBox = choice.choices[choice.choices.Count-1].option.GetComponentInChildren<Box>();
            reference.transform.SetParent(currentBox.transform);
            reference.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            choice.AddLine(reference);
            currentBox.UpdateSize();
        }else{
            var currentBox = (currentParent as Lines.Node).GetComponentInChildren<Box>();
            reference.transform.SetParent(currentBox.transform);
            reference.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            (currentParent as Lines.Node).AddLine(reference);
            currentBox.UpdateSize();
        }
        reference.nodeName = nodeName;
        
        return newNode.GetComponent<Lines.Reference>();
    }

    private static Lines.Choice CreateChoice(string choiceLine){
        var newNode = MonoBehaviour.Instantiate(ServiceDesk.instance.GetItem("Option"));
        newNode.transform.SetParent(canvas);
        var choice = newNode.GetComponent<Lines.Choice>();
        if(currentParent.type == Lines.LineType.Choice){
            var currentChoice = currentParent as Lines.Choice;
            var currentBox = currentChoice.choices[currentChoice.choices.Count-1].option.GetComponentInChildren<Box>();
            choice.transform.SetParent(currentBox.transform);
            choice.transform.SetSiblingIndex(currentBox.transform.childCount-2);
            currentChoice.AddLine(choice);
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
