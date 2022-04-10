using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Properties{
    
public class Reference : MonoBehaviour
{
    public TMP_InputField InputField;

    Lines.Reference currentLine;
    public void Show(Lines.Reference line){
        currentLine = line;
        InputField.onValueChanged.RemoveAllListeners();
        InputField.text = line.nodeName;
        InputField.onValueChanged.AddListener((string data) => {line.nodeName = data;});
    }

    public void DeleteSelf(){
        if(currentLine.lineParent.type == Lines.LineType.Node){
            (currentLine.lineParent as Lines.Node).RemoveLine(currentLine);
        }else if(currentLine.lineParent.type == Lines.LineType.Choice){
            (currentLine.lineParent as Lines.Choice).RemoveLine(currentLine);
        }
        PropertyMiddleman.instance.Hide();
    }
}

}