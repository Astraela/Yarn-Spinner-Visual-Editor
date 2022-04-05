using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lines{

public class Dialogue : Line
{
    public string dialogueLines;


    public override string Serialize(string indentation)
    {
        string str = "";
        using (StringReader reader = new StringReader(dialogueLines))
        {
            string line = string.Empty;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    str += indentation + line + "\n";
                }

            } while (line != null);
        }

        return str;
    }
}

}