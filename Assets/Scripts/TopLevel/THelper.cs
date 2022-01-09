using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class THelper
{
    //SingletonConstructor

    private static THelper tHelper;
    private static MHelper mHelper;
    private static AHelper aHelper;
    public static THelper Inst()
    {
        if (tHelper == null)
        {
            tHelper = new THelper();
            mHelper = MHelper.Inst();
            aHelper = AHelper.Inst();
            activeModules = new Dictionary<int, IModule>();
            
        }
        return tHelper;
    }


    //SingletonConstructir



    //ListsB
    public static Dictionary<int,IModule> activeModules;
    //ListsE



    //Methods (B)
    public void Cleanup()
    {
        //temporary, to see different results;
        mHelper.lstJoints.ForEach((x) =>
        {
            UnityEngine.Object.Destroy(x);
        });
        mHelper.lstJoints.Clear();
        mHelper.lstBones.ForEach((x) =>
        {
            UnityEngine.Object.Destroy(x);
        });
        mHelper.lstBones.Clear();
        mHelper.lstSpikes.ForEach(x =>
        {
            UnityEngine.Object.Destroy(x);
        });
        mHelper.lstSpikes.Clear();
        aHelper.refrencesCleanup();
        aHelper.armsAnimated = false;
        aHelper.increase = true;
        activeModules.Clear();
    }
    //Methods (E)
}
