using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Properties{
    
public class Dialogue : MonoBehaviour
{
    public TMP_InputField InputField;

    Lines.Dialogue currentLine;
    public void Show(Lines.Dialogue line){
        currentLine = line;
        InputField.onValueChanged.RemoveAllListeners();
        InputField.text = line.dialogueLines;
        InputField.onValueChanged.AddListener((string data) => {line.dialogueLines = data;});
    }

    public void DeleteDialogue(){
        if(currentLine.lineParent.type == Lines.LineType.Node){
            (currentLine.lineParent as Lines.Node).RemoveLine(currentLine);
        }else if(currentLine.lineParent.type == Lines.LineType.Choice){
            (currentLine.lineParent as Lines.Choice).RemoveLine(currentLine);
        }
        PropertyMiddleman.instance.Hide();
    }
}

}