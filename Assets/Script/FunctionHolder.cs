using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using UnityEngine;
using SFB;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

public class FunctionHolder : MonoBehaviour
{
    private string lastLocation;
    public static FunctionItem custom = new FunctionItem("Custom","Custom Function. Allows you to type a non-added function, format: FunctionName(arg,arg,arg...)");
    public List<FunctionItem> functions = new List<FunctionItem>(){custom};

    public static FunctionHolder instance;
    
    private void Start(){
        instance = this;
        lastLocation = PlayerPrefs.GetString("LastFunctionLocation",string.Empty);
        if(lastLocation != string.Empty){
            var list = JsonConvert.DeserializeObject<List<List<string>>>(File.ReadAllText(lastLocation));
            functions.Clear();
            functions.Add(custom);
            foreach(List<string> functionItem in list){
                FunctionItem newFunctionItem = new FunctionItem(functionItem[0],functionItem[1]);
                for (int i = 2; i < functionItem.Count; i++)
                {
                    newFunctionItem.args.Add(functionItem[i]);
                }
                functions.Add(newFunctionItem);
            }
        }
    }

    public void Open(){
        OpenFileDialog open = new OpenFileDialog();
        
        var extensionList = new [] {
            new ExtensionFilter("Yarnf", "yarnf"),
            new ExtensionFilter("All", "*"),
        };

        string location = PlayerPrefs.GetString("LastFunctionLocation",string.Empty);
        if(location.Contains(".yarn")){
            location = location.Substring(0,location.LastIndexOf('\\'));
        }
        var path = StandaloneFileBrowser.OpenFilePanel("Open File",location,extensionList,false);
        if(path.Length == 0) return;

        PlayerPrefs.SetString("LastFunctionLocation",path[0]);
        var list = JsonConvert.DeserializeObject<List<List<string>>>(File.ReadAllText(path[0]));
        functions.Clear();
        functions.Add(custom);
        foreach(List<string> functionItem in list){
            FunctionItem newFunctionItem = new FunctionItem(functionItem[0],functionItem[1]);
            for (int i = 2; i < functionItem.Count; i++)
            {
                newFunctionItem.args.Add(functionItem[i]);
            }
            functions.Add(newFunctionItem);
        }
        FindObjectOfType<Properties.Function>().UpdateDropdown();
        
    }
}

public class FunctionItem{
    public string name;
    public string description;
    public List<string> args = new List<string>();
    public FunctionItem(string _name, string _description){name = _name; description = _description;}
}
