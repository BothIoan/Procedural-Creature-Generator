using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour
{

    //ClassWiring
    SHelper sHelper;
    MHelper mHelper;
    //ClassWiring

    //UI
    static GameObject contents;
    static GameObject tf;
    private static GameObject createB;
    private static GameObject grabB;
    private static GameObject sendDataB;
    private static GameObject switchProceduralGeneratedB;
    private static GameObject switchGanGeneratedB;
    private static GameObject label;
    private static Sprite btnDefaultImage;
    private static Color btnDefaultColor;
    //UI


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
        contents = GameObject.Find("Content");
        tf = GameObject.Find("InputField");
        createB = GameObject.Find("CreateBone");
        grabB = GameObject.Find("Grab");
        sendDataB = GameObject.Find("SendData");
        switchGanGeneratedB = GameObject.Find("GanGenerated");
        switchProceduralGeneratedB = GameObject.Find("ProceduralGenerated");
        label = GameObject.Find("Label");
        btnDefaultImage = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        btnDefaultColor = new Color(208f / 255f, 195f / 255f, 210 / 255f);
        //UI

    }

    //SingletonConstructor



    //Methods (B)

    public GameObject InstSymPlane()
    {
        sHelper.newPlane();
        mHelper.lstJoints.Add(sHelper.symPlane);
        return sHelper.symPlane;
    }

    private static void CategOnClick(string categName)
    {
        DefocusEverything();
        Categ.SaveCateg(THelper.currentCategory);
        Categ.ClearGans();
        Categ.LoadCateg(categName);
        createB.SetActive(true);
        grabB.SetActive(true);
        ChangeSelected(categName);
        THelper.currentCategory = categName;
        THelper.categs.Add(categName);
        GanGeneratedCleanup();
        SetProceduralGenerated();
    }

    private static void SetProceduralGenerated()
    {
        DefocusEverything();
        MHelper.ganGenerated = false;
        switchGanGeneratedB.GetComponent<Button>().interactable = true;
        switchProceduralGeneratedB.GetComponent<Button>().interactable = false;
        SHelper.CloseWarningMessage();

    }

    public static string ValidateNewCategName()
    {
        string categName = tf.GetComponent<InputField>().text;
        if (categName.Equals(""))
        {
            return "Category names can't be ''";
        }
        if (categName.Contains(" ") || categName.Contains(","))
        {
            return "Category names can't contain ','s or ' 's";
        }
        if (THelper.categs.Contains(categName))
        {
            return "Duplicate category names not allowed";
        }
        return "";
    }


    public static void MakeButton(string categName)
    {
        GameObject container = new GameObject();
        container.transform.SetParent(contents.transform, false);
        HorizontalLayoutGroup hlg = container.AddComponent<HorizontalLayoutGroup>();
        hlg.childControlHeight = false;
        hlg.childForceExpandHeight = false;
        GameObject newButton = DefaultControls.CreateButton(new DefaultControls.Resources());
        newButton.name = categName + "_select";
        Text text = newButton.GetComponentInChildren<Text>();
        text.text = categName;
        text.fontSize = 7;
        Image btnImage = newButton.GetComponent<Image>();
        btnImage.sprite = btnDefaultImage;
        btnImage.color = btnDefaultColor;
        RectTransform nRectTransform = newButton.GetComponent<RectTransform>();
        //nRectTransform.sizeDelta = new Vector2(20, 10);
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => { CategOnClick(text.text); });
        newButton.transform.SetParent(container.transform, false);
        GameObject deleteButton = DefaultControls.CreateButton(new DefaultControls.Resources());
        deleteButton.name = categName + "_delete";
        Text textDelete = deleteButton.GetComponentInChildren<Text>();
        textDelete.text = "-";
        textDelete.fontSize = 7;
        RectTransform dRectTransform = newButton.GetComponent<RectTransform>();
        //dRectTransform.sizeDelta = new Vector2(20, 10);
        Button buttonDelete = deleteButton.GetComponent<Button>();
        buttonDelete.transform.SetParent(container.transform, false);
        buttonDelete.onClick.AddListener(() =>
        {
            Categ.DeleteCateg(categName);
            if (categName.Equals(THelper.currentCategory))
            {
                DefocusEverything();
                THelper.currentCategory = "";
            }
            Destroy(newButton);
            Destroy(deleteButton);
            Destroy(container);
            GanGeneratedCleanup();
            THelper.categs.Remove(categName);
            SHelper.CloseWarningMessage();
            SetProceduralGenerated();
            
        });
        RectTransform containerRT = container.GetComponent<RectTransform>();
        containerRT.sizeDelta = new Vector2(100, 33);
        btnImage = deleteButton.GetComponent<Image>();
        btnImage.sprite = btnDefaultImage;
        btnImage.color = btnDefaultColor;
        GanGeneratedCleanup();
    }

    private static void GanGeneratedCleanup()
    {
        THelper.Cleanup();
        Modules.Cleanup();
        sendDataB.SetActive(false);
        switchGanGeneratedB.SetActive(false);
        switchProceduralGeneratedB.SetActive(false);
        label.SetActive(false);
        MHelper.ganGenerated = false;
        switchGanGeneratedB.GetComponent<Button>().interactable = false;
        switchProceduralGeneratedB.GetComponent<Button>().interactable = true;
    }

    public string NewCateg()
    {
        string categName = tf.GetComponent<InputField>().text;
        MakeButton(categName);
        return categName;
    }

    public static void DefocusEverything()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
    public static void ChangeSelected(string newCategName)
    {
        if (!THelper.currentCategory.Equals(""))
        {
            GameObject oldSelect = GameObject.Find(THelper.currentCategory + "_select");
            oldSelect.GetComponent<Button>().interactable = true;
            GameObject oldDelete = GameObject.Find(THelper.currentCategory + "_delete");
            oldDelete.GetComponent<Button>().interactable = true;

        }
        GameObject newSelect = GameObject.Find(newCategName + "_select");
        newSelect.GetComponent<Button>().interactable = false;
    }

    //Methods (E)
}
