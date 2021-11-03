using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Categ
{
    public static void runScript()
    {
        var p = new System.Diagnostics.Process();
        p.StartInfo.FileName = "python3";
        p.StartInfo.Arguments = "mlScripts/script.py";
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        Debug.Log(p.StandardOutput.ReadToEnd());
    }
    
    
}
