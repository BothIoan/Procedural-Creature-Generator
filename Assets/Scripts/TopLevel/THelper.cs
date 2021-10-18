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
        }
        return tHelper;
    }

    //SingletonConstructir



    //Methods (B)
    public void Cleanup()
    {
        //temporary, to see different results;
        mHelper.lstJoints.ForEach((x) =>
        {
            Object.Destroy(x);
        });
        mHelper.lstJoints.Clear();
        mHelper.lstBones.ForEach((x) =>
        {
            Object.Destroy(x);
        });
        mHelper.lstBones.Clear();
        mHelper.lstSpikes.ForEach(x =>
        {
            Object.Destroy(x);
        });
        mHelper.lstSpikes.Clear();
        aHelper.refrencesCleanup();
        aHelper.armsAnimated = false;
        aHelper.increase = true;
    }
    //Methods (E)
}
