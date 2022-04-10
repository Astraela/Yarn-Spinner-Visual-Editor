using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lines{

public class Function : Line
{
    [System.NonSerialized]
    public string func = "FunctionName(test)";


    public override string Serialize(string indentation)
    {
        string str = indentation+"<<call ";
        
        str += func;

        str += ">>\n";

        return str;
    }
}

}
