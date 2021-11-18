using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
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
        Categ.StartScript();
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
        MakeGans();
    }

    //SingletonConstructor

    [SerializeField] public bool fun;

    public void animationStage()
    {
        anim.HeadRiggingStage();
        anim.ArmRiggingStage();
        anim.LegRiggingStage();
    }

    public void  generationStage()
    {

        
        if (fun) sHelper.forFun();
        topLevelHelper.Cleanup();
        modules.Cleanup();
        setup.PlaneStage();

        RequestGanAll();

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

    public void PressButton()
    {
        generationStage();
        animationStage();
        eHelper.SaveCharacterStage();
    }
    public void DataToGan()
    {
        foreach (IModule x in THelper.activeModules.Values) x.DataToGan();
    }

    private void MakeGans()
    {
        foreach (IModule x in Modules.iModules)
        {
            x.MakeGan();
        }
    }

    public void RequestGanAll()
    {
        foreach (IModule x in Modules.iModules)
        {
            x.GetDataGan();
        }
    }

    private void OnApplicationQuit()
    {
        Categ.EndScript();
    }
}
