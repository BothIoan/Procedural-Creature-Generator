using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

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
    [SerializeField] private GameObject switchGanGeneratedB;
    [SerializeField] private GameObject switchProceduralGeneratedB;
    [SerializeField] private GameObject label;
    //UI

    //SingletonConstructor

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
        eHelper = EHelper.Inst();
        mHelper = MHelper.Inst();
        THelper.Inst();
        new Serializer();
        RequestCategs();
        DisableButtons();
        SetProceduralGenerated();
    }

    //SingletonConstructor
    public void SetGanGenerated()
    {
        Setup.DefocusEverything();
        MHelper.ganGenerated = true;
        switchGanGeneratedB.GetComponent<Button>().interactable = false;
        switchProceduralGeneratedB.GetComponent<Button>().interactable = true;
        SHelper.CloseWarningMessage();
    }
    public void SetProceduralGenerated()
    {
        Setup.DefocusEverything();
        MHelper.ganGenerated = false;
        switchGanGeneratedB.GetComponent<Button>().interactable = true;
        switchProceduralGeneratedB.GetComponent<Button>().interactable = false;
        SHelper.CloseWarningMessage();

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
        SHelper.CloseWarningMessage();
        Setup.DefocusEverything();
        THelper.Cleanup();
        Modules.Cleanup();
        DisableButtons();
        grammarStage();
        mHelper.DrawBonesStage();
        anim.AnimationStage(); 
        eHelper.SaveCharacterStage();
        if (!THelper.currentCategory.Equals(""))
        {
            EnableButtons();
        }
    }
    public void DataToGan()
    {
        SHelper.CloseWarningMessage();
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
        SHelper.CloseWarningMessage();
        string validateString = Setup.ValidateNewCategName();
        if (!validateString.Equals(""))
        {
            SHelper.categNameWarningMessage.GetComponent<Text>().text = validateString;
            SHelper.categNameWarningMessage.SetActive(true);
            return;
        }

        Setup.DefocusEverything();
        if (THelper.activeModules.Count != 0)
        {
            Categ.SaveCateg(THelper.currentCategory);
            THelper.activeModules.Clear();
            Categ.ClearGans();
        }
        Modules.Cleanup();
        THelper.Cleanup();
        DisableButtons();
        SetProceduralGenerated();
        MakeGans();
        string categName = setup.NewCateg();
        Setup.ChangeSelected(categName);
        THelper.currentCategory = categName;
    }
    public void DisableButtons()
    {
        switchGanGeneratedB.SetActive(false);
        switchProceduralGeneratedB.SetActive(false);
        label.SetActive(false);
        sendDataB.SetActive(false);
    }
    public void EnableButtons()
    {
        switchGanGeneratedB.SetActive(true);
        switchProceduralGeneratedB.SetActive(true);
        label.SetActive(true);
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

    }
}
