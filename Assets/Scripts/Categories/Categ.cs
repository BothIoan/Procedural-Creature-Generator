using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;

public class Categ
{
    private static string VENVPY = Directory.GetCurrentDirectory() + "\\Gan\\Scripts\\python.exe";
    private static string SCRIPT = Directory.GetCurrentDirectory() + "\\Gan\\gansManager.py";
    private static System.Diagnostics.Process process;
    private static StreamWriter scriptInput;
    private static AutoResetEvent evt = new AutoResetEvent(false);

    public static void StartScript()
    {
        process = new System.Diagnostics.Process();
        process.StartInfo.FileName = VENVPY;
        process.StartInfo.Arguments = SCRIPT;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardInput = true;
        process.EnableRaisingEvents = true;
        process.StartInfo.CreateNoWindow = true;
        process.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler(ReceiveData);
        process.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler((s, e) => {
            if (string.IsNullOrEmpty(e.Data)) return;
            Debug.LogError(e.Data);
        });
        process.Start();
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        scriptInput = process.StandardInput;
    }

    public static void EndScript()
    {
        SaveCateg(THelper.currentCategory);
        process.Kill();
    }

    public static void RequestData(string moduleKey)
    {
        //request info from gan
        scriptInput.WriteLine("g " + moduleKey);
        
    }

    public static void ReceiveData(object sender, System.Diagnostics.DataReceivedEventArgs args)
    {
        string sData = args.Data;
        if(sData[0].Equals('c'))
        {
            if(sData.Length == 1)
            {
                THelper.categs = new List<string>();
            }
            else
            {
                sData = sData.Substring(2);
                THelper.categs = sData.Split(new char[] { ',' }).ToList();
            }
            evt.Set();
            return;
        }
        if (sData[0].Equals('s'))
        {
            evt.Set();
            return;
        }
        if (sData[0].Equals('g'))
        {
            THelper.activeModules[int.Parse(sData.Substring(1))].ReceiveGanMade();
            return;
        }
        List<float> lData = Array.ConvertAll(sData.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries), float.Parse).ToList();
        THelper.activeModules[(int)lData[0]].ReceiveGen(lData.Skip(1).ToList());   
    }

    public static void SaveCateg(string categName)
    {
        scriptInput.WriteLine("s " + categName);
        evt.WaitOne();
    }

    public static void LoadCateg(string categName)
    {
        scriptInput.WriteLine("l " + categName);
        evt.WaitOne();
    }

    public static void GiveDataTrue(string moduleKey, List<float> data)
    {
        //send data to be trained with
        scriptInput.WriteLine("t " + moduleKey + " " + string.Join(",",data));
    }

    public static void GiveDataFalse(string genKey, List<float> data)
    {
    }

    public static void MakeGan(string moduleKey, string shape)
    {
        scriptInput.WriteLine("m " + moduleKey + " " + shape);
    }

    public static void ClearGans()
    {
        scriptInput.WriteLine("d");
        evt.WaitOne();
    }
    public static void RequestCategs()
    {
        scriptInput.WriteLine("c");
        evt.WaitOne();
    }
    public static void DeleteCateg(string categName)
    {
        scriptInput.WriteLine("x " + categName);
    }
}
