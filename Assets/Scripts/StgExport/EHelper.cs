using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EHelper
{
    //ClassWiring
    private static SHelper setupHelper;
    //ClassWiring

    //SingletonConstructor

    private static EHelper eHelper;
    public static EHelper Inst()
    {
        if (eHelper == null)
        {
            eHelper = new EHelper();
            setupHelper = SHelper.Inst();
        }
        return eHelper;
    }

    //SingletonConstructor

    //Methods (B)
    public void SaveCharacterStage()
    {
        PrefabUtility.SaveAsPrefabAsset(setupHelper.symPlane, "Assets/Dihanii/dihanie.prefab");
    }
    //Methods (E)
}
