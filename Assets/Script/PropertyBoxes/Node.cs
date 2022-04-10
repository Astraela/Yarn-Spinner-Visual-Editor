using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Properties{

public class Node : MonoBehaviour
{
    public TMP_InputField InputField;

    Lines.Node currentLine;
    public void Show(Lines.Node line){
        currentLine = line;
        InputField.onValueChanged.RemoveAllListeners();
        InputField.text = line.title;
        InputField.onValueChanged.AddListener((string data) => {line.title = data;});
    }

    public void DeleteSelf(){
        Destroy(currentLine.gameObject);
        PropertyMiddleman.instance.Hide();
    }
}

}