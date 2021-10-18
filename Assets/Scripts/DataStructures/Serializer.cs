using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer
{
    private static List<float> floatSerializationLst;
    private static List<int> intSerializationLst;

    public Serializer()
    {
        Serializer.floatSerializationLst = new List<float>();
        Serializer.intSerializationLst = new List<int>();
    }
    public static void SerializerCleanup()
    {
        floatSerializationLst.Clear();
        intSerializationLst.Clear();
    }

    public static int RandOvr(int floor, int ceiling)
    {
        int value = Random.Range(floor, ceiling);
        intSerializationLst.Add(value);
        return value;
    }

    public static float RandOvr(float floor, float ceiling)
    {
        float value = Random.Range(floor, ceiling);
        floatSerializationLst.Add(value);
        return value;
    }

    public static void PaddingInt(int maximum, int actual)
    {
        int nrZeros = maximum - actual;
        for(int i = 0; i < nrZeros; i++)
        {
            intSerializationLst.Add(0);
        }
    }

    public static void PaddingFloat(int maximum, int actual)
    {
        int nrZeros = maximum - actual;
        for (int i = 0; i < nrZeros; i++)
        {
            intSerializationLst.Add(0);
        }
    }

    public static void ForDebug()
    {
        Debug.Log("Int List:\n");
        intSerializationLst.ForEach(x=> {
            Debug.Log(x);
        });
        Debug.Log("\n");
        Debug.Log("\n");
        Debug.Log("Float List:\n");
        intSerializationLst.ForEach(x => {
            Debug.Log(x);
        });
    }
}
