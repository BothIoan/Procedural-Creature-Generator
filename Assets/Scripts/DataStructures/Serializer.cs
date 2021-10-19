using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class Serializer
{
    private static List<float> serializationLst =new List<float>();
    public static int padding = 0;
    
    public static void setSerializationList(List<float> list)
    {
        serializationLst.Clear();
        serializationLst = list;
    }

    public static void SerializerCleanup()
    {
        serializationLst.Clear();
    }

    public static int RandOvr(int floor, int ceiling)
    {
        int value = Random.Range(floor, ceiling);
        serializationLst.Add(value);
        return value;
    }

    public static float RandOvr(float floor, float ceiling)
    {
        float value = Random.Range(floor, ceiling);
        serializationLst.Add(value);
        return value;
    }

    //used together. For first getting a random number, and then adding max - actual 0s to the serializar
    public static int RememberPadding(int floor, int ceiling)
    {
        int value = Random.Range(floor, ceiling);
        ceiling--;
        padding = ceiling - value;
        return value;
    }

    public static void AddPadding()
    {
        for (int i = 0; i < padding; i++)
        {
            serializationLst.Add(0);
        }
    }
    //used together.

    public static void AddPadding(int givenPadding)
    {
        for (int i = 0; i < givenPadding; i++)
        {
            serializationLst.Add(0);
        }
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

    public static void writeToFile()
    {
        StreamWriter writer = new StreamWriter("./out.txt", false);
        serializationLst.ForEach(x => {
            writer.Write(x.ToString() + " ");
        });
        writer.Flush();
        writer.Dispose();
    }
}
