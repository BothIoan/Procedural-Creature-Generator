using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLevel : MonoBehaviour
{

    //ClassWiring
    private static SHelper sHelper;
    private static EHelper eHelper;
    private static MHelper mHelper;

    private Modules modules;
    private Setup setup;
    private Anim anim;
    //ClassWiring

    //SingletonConstructor

    private static THelper topLevelHelper;
    private static TopLevel topLevel;
    public static TopLevel Inst()
    {
        return topLevel;
    }

    private void Awake()
    {
        topLevel = this;
        sHelper = SHelper.Inst();
        setup = Setup.Inst();
        modules = Modules.Inst();
        anim = Anim.Inst();
        topLevelHelper = THelper.Inst();
        eHelper = EHelper.Inst();
        mHelper = MHelper.Inst();
        new Serializer();
    }

    //SingletonConstructor

    [SerializeField] public bool fun;

    public void animationStage()
    {
        anim.HeadRiggingStage();
        anim.ArmRiggingStage();
        anim.LegRiggingStage();
    }
    public void generationStage()
    {
        if (fun)
            sHelper.forFun();
        topLevelHelper.Cleanup();
        setup.PlaneStage();
        modules.HSpineStage();
        if (Random.Range(0, 2) == 1)
        {
            modules.TailStage();
        }
        modules.LegsStage();

        if (Random.Range(0, 2) == 1)
        {

            modules.VSpineStage();
            modules.ArmsStage();
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    modules.GenerativeWingsStage();
                    //ArchwingStage();
                    break;
                case 1:
                    modules.BatwingsStage();
                    break;
            }

        }

        modules.HeadStage();
        mHelper.DrawBonesStage();
        Serializer.ForDebug();
    }

    public void pressButton()
    {
        generationStage();
        animationStage();
        eHelper.SaveCharacterStage();

    }
}
