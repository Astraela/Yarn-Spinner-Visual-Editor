using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Properties{

public class Choice : MonoBehaviour
{
    public Transform verticalList;
    public GameObject optionPrefab;


    public List<TMP_InputField> options = new List<TMP_InputField>();
    
    Lines.Choice selectedLine;
    public void Show(Lines.Choice line){
        selectedLine = line;
        for (int i = options.Count-1; i >= 0; i--)
        {
            Destroy(options[i].transform.parent.gameObject);
        }
        options = new List<TMP_InputField>();
        int siblingIndex = 0;
        foreach(Lines.ChoiceList option in line.choices){
            var newOption = Instantiate(optionPrefab);
            var inputField = newOption.GetComponentInChildren<TMP_InputField>();
            inputField.text = option.choiceString;
            inputField.onValueChanged.AddListener((string data) => {option.choiceString = data;});
            newOption.GetComponentInChildren<Button>().onClick.AddListener(() => DeletePress(siblingIndex-1));
            newOption.GetComponentInChildren<TextMeshProUGUI>().text = "Option " + (siblingIndex+1);
            newOption.transform.SetParent(verticalList);
            newOption.transform.SetSiblingIndex(siblingIndex);
            options.Add(inputField);
            siblingIndex++;
        }
    }

    public void DeletePress(int siblingIndex){
        print(siblingIndex);
        print(options.Count);
        Destroy(options[siblingIndex].transform.parent.gameObject);
        options.RemoveAt(siblingIndex);
        selectedLine.RemoveChoice(siblingIndex);
        int sibling = 1;
        foreach(var option in options){
            option.transform.parent.GetComponentInChildren<TextMeshProUGUI>().text = "Option " + sibling;
            sibling++;
        }
    }

    public void AddOption(){
        var newPrefab = Instantiate(optionPrefab).transform;
        newPrefab.SetParent(verticalList);
        newPrefab.SetSiblingIndex(verticalList.childCount-2);
        var newChoice = selectedLine.AddChoice();
        var inputField = newPrefab.GetComponentInChildren<TMP_InputField>();
        inputField.text = "Option " + (verticalList.childCount-1);
        newPrefab.GetComponentInChildren<TextMeshProUGUI>().text = "Option " + (verticalList.childCount-1);
        inputField.onValueChanged.AddListener((string data) => {newChoice.choiceString = data;});
        newPrefab.GetComponentInChildren<Button>().onClick.AddListener(() => DeletePress(newPrefab.GetSiblingIndex()));
        options.Add(inputField);
    }

    
    public void DeleteOptions(){
        if(selectedLine.lineParent.type == Lines.LineType.Node){
            (selectedLine.lineParent as Lines.Node).RemoveLine(selectedLine);
        }else if(selectedLine.lineParent.type == Lines.LineType.Choice){
            (selectedLine.lineParent as Lines.Choice).RemoveLine(selectedLine);
        }
        PropertyMiddleman.instance.Hide();
    }
}

}