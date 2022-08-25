using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup: MonoBehaviour
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
    private static Color btnSelectedColor;
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
        btnSelectedColor = new Color(195f / 255f, 200f / 255f, 210f / 255f);
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
        sendDataB.SetActive(true);
        switchGanGeneratedB.SetActive(true);
        switchProceduralGeneratedB.SetActive(true);
        label.SetActive(true);
        ChangeSelected(categName);
        THelper.currentCategory = categName;
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
        btnImage.overrideSprite = btnDefaultImage;
        btnImage.color = btnDefaultColor;
        RectTransform nRectTransform = newButton.GetComponent<RectTransform>();
        //nRectTransform.sizeDelta = new Vector2(20, 10);
        Button button = newButton.GetComponent<Button>();
        button.onClick.AddListener(() => { CategOnClick(text.text); });
        newButton.transform.SetParent(container.transform,false);
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
                sendDataB.SetActive(false);
                switchGanGeneratedB.SetActive(false);
                switchProceduralGeneratedB.SetActive(false);
                label.SetActive(false);
                MHelper.ganGenerated = false;
                DisableSelected();
                THelper.currentCategory = "";
            }
            Destroy(newButton);
            Destroy(deleteButton);
            Destroy(container);
        });
        RectTransform containerRT = container.GetComponent<RectTransform>();
        containerRT.sizeDelta = new Vector2(100, 33);
        btnImage = deleteButton.GetComponent<Image>();
        btnImage.overrideSprite = btnDefaultImage;
        btnImage.color = btnDefaultColor;
    }

    public string NewCateg()
    {
        string categName= tf.GetComponentInChildren<Text>().text;
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
            oldSelect.GetComponent<Image>().color = btnDefaultColor;
            GameObject oldDelete = GameObject.Find(THelper.currentCategory + "_delete");
            oldDelete.GetComponent<Image>().color = btnDefaultColor;

        }
        GameObject newSelect = GameObject.Find(newCategName + "_select");
        newSelect.GetComponent<Image>().color = btnSelectedColor;
        GameObject newDelete = GameObject.Find(newCategName + "_delete");
        newDelete.GetComponent<Image>().color = btnSelectedColor;
    }

    public static void DisableSelected()
    {
        GameObject oldSelect = GameObject.Find(THelper.currentCategory + "_select");
        oldSelect.GetComponent<Image>().color = btnDefaultColor;
        GameObject oldDelete = GameObject.Find(THelper.currentCategory + "_delete");
        oldDelete.GetComponent<Image>().color = btnDefaultColor;
    }

    //Methods (E)
}
