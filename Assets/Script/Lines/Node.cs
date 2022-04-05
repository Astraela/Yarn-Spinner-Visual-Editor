using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lines{

public class Node : Line
{    
    [SerializeField]
    private string _title;
    public string title { get => _title; set{
        _title = value;
        GetComponentInChildren<TextMeshProUGUI>().text = value;
        } 
    }


    List<Line> lines = new List<Line>();

    public void AddLine(Line line){
        lines.Add(line);
        line.lineParent = this;
    }
    
    public void RemoveLine(Line line){
        lines.Remove(line);
        Destroy(line.gameObject);
    }

    public override string Serialize(string indentation)
    {
        string str = "title: " + title + "\n" + "tags:\n" + "---\n";
        foreach(Transform line in GetComponentInChildren<Box>().transform){
            if(line.GetComponent<Lines.Line>())
                str += line.GetComponent<Lines.Line>().Serialize(indentation);
        }
        str += "===\n";
        return str;
    }
}

}
