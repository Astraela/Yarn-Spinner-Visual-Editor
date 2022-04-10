using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace Properties{

public class Function : MonoBehaviour
{
    public Transform verticalList;
    public GameObject PropertyPrefab;
    public TMP_Dropdown dropDown;
    public TMP_Text description;

    public List<TMP_InputField> arguments = new List<TMP_InputField>();
    public GameObject valid;
    public GameObject invalid;
    public TMP_InputField invalidTextBox;

    List<FunctionItem> functions;
    Lines.Function selectedLine;
    
    private void Start(){
        functions = FunctionHolder.instance.functions;
        UpdateDropdown();
    }

    public void Show(Lines.Function line){
        selectedLine = line;
        string[] args = GetArgs();
        int index = dropDown.options.FindIndex(x => x.text == args[0]);
        if(index == -1){
            ValueChanged(0);
            dropDown.value = 0;
            invalidTextBox.text = line.func;
            invalidTextBox.onValueChanged.RemoveAllListeners();
            invalidTextBox.onValueChanged.AddListener((string data) => {line.func = data;});
        }else{
            ValueChanged(index);
            dropDown.value = index;
            for (int i = 1; i < args.Length; i++)
            {
                int inter = i;
                arguments[i-1].onValueChanged.AddListener((string data) => {args[inter] = data; line.func = format(args);});
                arguments[i-1].text = args[i];
            }
        }
    }

    public void UpdateDropdown(){
        dropDown.ClearOptions();
        functions = FunctionHolder.instance.functions;
        foreach(FunctionItem functionItem in functions){
            dropDown.AddOptions(new List<string>(){functionItem.name});
        }
    }

    public void ValueChanged(int value){
        foreach(Transform child in verticalList){
            Destroy(child.gameObject);
        }
        if(value == 0){
            invalid.SetActive(true);
            valid.SetActive(false);
            invalidTextBox.text = selectedLine.func;
            invalidTextBox.onValueChanged.RemoveAllListeners();
            invalidTextBox.onValueChanged.AddListener((string data) => {selectedLine.func = data;});
            FunctionItem functionItem = functions.Find(x => x.name == "Custom");
            description.text = FunctionHolder.custom.description;
        }else{
            valid.SetActive(true);
            invalid.SetActive(false);
            string functionName = dropDown.options[value].text;
            arguments.Clear();
            var args = GetArgs();
            FunctionItem functionItem = functions.Find(x => x.name == functionName);
            description.text = functionItem.description;
            selectedLine.func = functionName + "(";
            
            for (int i = 0; i < functionItem.args.Count; i++)
            {
                var newPropertyPrefab = Instantiate(PropertyPrefab);
                var textField = newPropertyPrefab.GetComponentInChildren<TextMeshProUGUI>();
                var inputField = newPropertyPrefab.GetComponentInChildren<TMP_InputField>();
                newPropertyPrefab.transform.SetParent(verticalList);
                textField.text = functionItem.args[i];
                int index = i;
                if(args.Length > i+1){
                    inputField.text = args[i+1];
                    selectedLine.func += args[i+1];
                }else
                    inputField.text = "";
                inputField.onValueChanged.AddListener((string data) => {string[] args = GetArgs(); args[index+1] = data; selectedLine.func = format(args); });
                arguments.Add(inputField);
                if(i != functionItem.args.Count-1)
                    selectedLine.func += ",";
            }
                selectedLine.func += ")";
        }
    }
    
    public string[] GetArgs(){
        string title = selectedLine.func.Substring(0,selectedLine.func.IndexOf('(')+1);
        string rest = selectedLine.func.Substring(selectedLine.func.IndexOf('(')+1);
        string[] restArgs = rest.Split(',');
        string[] arr = new string[1+restArgs.Length];
        arr[0] = title;
        restArgs.CopyTo(arr,1);
        if(arr[0].Length > 1)
            arr[0] = arr[0].Substring(0,arr[0].Length-1);
        if(arr.Length > 1 && arr[arr.Length-1].Length > 0)
            arr[arr.Length-1] = arr[arr.Length-1].Substring(0,arr[arr.Length-1].Length-1);
        return arr;
    }

    public string format(string[] args){
        string str = "";
        string join = string.Join(',',args);
        join = join.Substring(join.IndexOf(',')+1);
        str =  args[0] + "(" + join + ")";
        return str;
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
