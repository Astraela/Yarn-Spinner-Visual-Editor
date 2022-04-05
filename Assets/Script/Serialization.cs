using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using SFB;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Serialization : MonoBehaviour
{
    public Transform canvas;
    
    private string lastLocation = string.Empty;

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null){
            
            if(Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))){
                SaveAs();
            }else if(Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))){
                QuickSave();
            }else if(Input.GetKeyDown(KeyCode.O) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))){

            }
        }
    }

    public void QuickSave(){
        if(lastLocation == string.Empty){
            SaveAs();
            return;
        }else{
            File.WriteAllText(lastLocation,Serialize());
        }
    }

    public void SaveAs(){
        var extensionList = new [] {
            new ExtensionFilter("Yarn", "yarn"),
            new ExtensionFilter("All", "*"),
        };
        var path = StandaloneFileBrowser.SaveFilePanel("Save File", lastLocation, "Test.yarn", extensionList);
        if(path == string.Empty) return;
        lastLocation = path;
        File.WriteAllText(lastLocation,Serialize());
    }

    public void Open(){
        Deserializer.Setup(canvas);
        OpenFileDialog open = new OpenFileDialog();
        
        var extensionList = new [] {
            new ExtensionFilter("Yarn", "yarn"),
            new ExtensionFilter("All", "*"),
        };

        var path = StandaloneFileBrowser.OpenFilePanel("Open File",lastLocation,extensionList,false);
        if(path.Length == 0) return;

        foreach (Transform child in canvas) {
            Destroy(child.gameObject);
        }
        Deserializer.DeSerialize(File.ReadAllText(path[0]));
    }




    public string Serialize(){
        string file = "";
        foreach(Transform node in canvas){
            file += node.GetComponent<Lines.Node>().Serialize("");
            file += "\n\n";
        }
        
        return file;
    }
}
