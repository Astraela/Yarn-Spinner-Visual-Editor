using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lines{

public class Reference : Line
{
    public string nodeName;

    public override string Serialize(string indentation)
    {
        return indentation + "[[" + nodeName +"]]\n";
    }
}

}