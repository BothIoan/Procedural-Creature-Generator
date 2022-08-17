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

    //UI
    [SerializeField] private GameObject createB;
    [SerializeField] private GameObject grabB;
    [SerializeField] private GameObject sendDataB;
    [SerializeField] private GameObject genStrategyB;
    //UI

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
        RequestCategs();
        DisableButtons();
    }

    //SingletonConstructor

    [SerializeField] public bool fun;
    public void toggleGenStrategy()
    {
        Setup.DefocusEverything();
        MHelper.ganGenerated = !MHelper.ganGenerated;
    }

    public void  grammarStage()
    {
        setup.InstSymPlane();

        RequestValues();

        modules.hSpine.Gen();
        
        modules.head.SetInJoint(modules.hSpine.getOutJointL()[0]);
        if (modules.grammar.Rand(0, 1) == 0)
        {
            modules.tail.Gen();
        }
        if (modules.grammar.Rand(0, 1) == 0)
            modules.legs.Gen();
        else
            modules.spider.Gen();

        int random = modules.grammar.Rand(0, 2);
        if (modules.grammar.Rand(0, 1) == 0)
        {
            modules.vSpine.Gen();
            modules.arms.Gen();
            
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
    }

    public void PressButton()
    {
        Setup.DefocusEverything();
        if (fun) sHelper.forFun();
        topLevelHelper.Cleanup();
        modules.Cleanup();
        grammarStage();
        mHelper.DrawBonesStage();
        anim.AnimationStage(); 
        eHelper.SaveCharacterStage();
    }
    public void DataToGan()
    {
        Setup.DefocusEverything();
        foreach (IModule x in THelper.activeModules.Values) x.DataToGan();
    }

    public static void MakeGans()
    {
        foreach (IModule x in Modules.iModules)
        {
            x.MakeGan();
        }
    }

    public void RequestValues()
    {
        foreach (IModule x in THelper.activeModules.Values)
        {
            x.GetDataGan();
        }
    }

    private void OnApplicationQuit()
    {
        Categ.EndScript();
    }

    public void NewCateg()
    {
        Setup.DefocusEverything();
        if (THelper.activeModules.Count != 0)
        {
            Categ.SaveCateg(THelper.currentCategory);
            THelper.activeModules.Clear();
            Categ.ClearGans();
        }
        MakeGans();
        string categName = setup.NewCateg();
        Setup.ChangeSelected(categName);
        THelper.currentCategory = categName;
        EnableButtons();
    }
    public void DisableButtons()
    {
        MHelper.ganGenerated = false;
        genStrategyB.SetActive(false);
        sendDataB.SetActive(false);
    }
    public void EnableButtons()
    {
        genStrategyB.SetActive(true);
        sendDataB.SetActive(true);
    }
    public void RequestCategs()
    {
        Categ.RequestCategs();
        THelper.categs.ForEach(x =>
        {
            Setup.MakeButton(x);
        });
        if(THelper.categs.Count == 0)
        {
            return;
        }
        MakeGans();
        Categ.ClearGans();
        Setup.ChangeSelected(THelper.categs[0]);
        THelper.currentCategory = THelper.categs[0];
        EnableButtons();

    }
}
