using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Lines{

public class Choice : Line
{
    [SerializeField]
    public List<ChoiceList> choices;

    public Transform container;
    public Transform choicePrefab;

    public void MoveLine(int index, Line line, int siblingIndex){
        var lines = choices[index].lines;
        lines.Remove(line);
        lines.Insert(siblingIndex,line);
    }

    public void RemoveChoice(int index){
        var choice = choices[index];
        Destroy(choice.option.gameObject);
        choices.RemoveAt(index);
    }

    public void AddLine(Line line){
        int index = line.GetComponentInParent<Box>().transform.parent.GetSiblingIndex();
        choices[index].lines.Add(line);
    }

    public void RemoveLine(Line line){
        int index = line.GetComponentInParent<Box>().transform.parent.GetSiblingIndex();
        choices[index].lines.Remove(line);
        Destroy(line.gameObject);
    }

    public void AddChoiceButton(){
        AddChoice();
        if(PropertyMiddleman.instance.current == this){
            PropertyMiddleman.instance.TurnOn(this);
        }
    }

    public ChoiceList AddChoice(){
        var newChoicePrefab = Instantiate(choicePrefab);
        var newChoice = new ChoiceList(newChoicePrefab,"Option "+ (container.childCount+1));
        newChoicePrefab.SetParent(container);
        newChoicePrefab.SetSiblingIndex(container.childCount);
        choices.Add(newChoice);
        return newChoice;
    }
    public ChoiceList AddChoice(string content){
        var newChoicePrefab = Instantiate(choicePrefab);
        var newChoice = new ChoiceList(newChoicePrefab,content);
        newChoicePrefab.SetParent(container);
        newChoicePrefab.SetSiblingIndex(container.childCount);
        choices.Add(newChoice);
        return newChoice;
    }
    
    public override string Serialize(string indentation)
    {
        string str = "";

        foreach(ChoiceList choiceList in choices){
            str += indentation + "-> " + choiceList.choiceString + "\n";
            foreach(Lines.Line line in choiceList.lines){
                str += line.Serialize(indentation + "\t");
            }
        }

        return str;
    }
}

[System.Serializable]
public class ChoiceList{
    [SerializeField]
    private string _choiceString;
    public string choiceString {get => _choiceString; set{
        _choiceString = value;
        option.GetComponentInChildren<TMP_Text>().text = value;
    }}
    public Transform option;
    public List<Line> lines = new List<Line>();
    public ChoiceList(Transform option, string name){
        this.option = option;
        this.choiceString = name;
    }
}

}