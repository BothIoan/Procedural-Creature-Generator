using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Serializer
{
    private static List<float> serializationLst =new List<float>();
    
    
    public static void setSerializationList(List<float> list)
    {
        serializationLst = list;
        serializationLst.Clear();
    }

    public static void SerializerCleanup()
    {
        serializationLst.Clear();
    }

    public static void ForDebug()
    {
        serializationLst.ForEach(x=> {
            Debug.Log(x);
        });
    }
    
    public static void ForDebug2()
    {
        Debug.Log(serializationLst.Count);
    }

    public static void WriteToFile()
    {
        StreamWriter writer = new StreamWriter("./out.txt", false);
        serializationLst.ForEach(x => {
            writer.Write(x.ToString() + " ");
        });
        writer.Flush();
        writer.Dispose();
    }
}
