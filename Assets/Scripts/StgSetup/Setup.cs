using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup: MonoBehaviour
{

    //ClassWiring
    SHelper sHelper;
    MHelper mHelper;
    //ClassWiring



    //SingletonConstructor

    private static Setup setup;
    public static Setup Inst()
    {
        return setup;
    }

    private void Awake()
    {
        setup = this;
        sHelper = SHelper.Inst();
        mHelper = MHelper.Inst();
    }

    //SingletonConstructor



    //Methods (B)

    public GameObject PlaneStage()
    {
        sHelper.newPlane();
        mHelper.lstJoints.Add(sHelper.symPlane);
        return sHelper.symPlane;
    }

    //Methods (E)
}
