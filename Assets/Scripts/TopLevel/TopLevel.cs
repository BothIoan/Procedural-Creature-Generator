using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLevel : MonoBehaviour
{
    //test
    private List<IModule> iModules;
    //test


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
        Categ.runScript();
        topLevel = this;
        sHelper = SHelper.Inst();
        setup = Setup.Inst();
        modules = Modules.Inst();
        modules.InstModules();
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
        if (fun) sHelper.forFun();
        topLevelHelper.Cleanup();
        modules.Cleanup();
        setup.PlaneStage();

        modules.hSpine.Gen();
        modules.head.SetInJoint(modules.hSpine.getOutJointL()[0]);
        if (Random.Range(0, 2) == 1)
        {
            modules.tail.Gen();
        }
        if (Random.Range(0, 2) == 1)
            modules.legs.Gen();
        else
            modules.spider.Gen();
        if (Random.Range(0, 2) == 1)
        {
            modules.vSpine.Gen();
            modules.arms.Gen();
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    modules.wings.Gen();
                    //ArchwingStage();
                    break;
                case 1:
                    modules.batwings.Gen();
                    break;
            }
            modules.head.SetInJoint(modules.vSpine.getOutJoint());
        }
        modules.head.Gen();
        mHelper.DrawBonesStage();
    }

    public void pressButton()
    {
        generationStage();
        animationStage();
        eHelper.SaveCharacterStage();
    }
}
