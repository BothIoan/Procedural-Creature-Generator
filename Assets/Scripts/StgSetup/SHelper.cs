using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SHelper
{
     
    //SingletonConstructor

    private static SHelper sHelper;
    public static SHelper Inst()
    {
        if (sHelper == null)
        {
            sHelper = new SHelper();
            //instantiate All that needs to be instantiated.
            sHelper.nume1 = new List<string>();
            sHelper.nume2 = new List<string>();
            sHelper.text = GameObject.Find("Title").GetComponent<Text>();
        }
        return sHelper;
    }

    //SingletonConstructor

    //Globals (B)

    //texturesB
    public GameObject symPlane;
    
    //texturesE

    //dataB
    public int currentDSign;
    public Text text;
    //dataE

    //listsB
    private List<string> nume1;
    private List<string> nume2;
    //listsE
   
    //Globals (E)



    //Methods (B)

    //gives quirky names to the creatures.
    public void forFun()
    {
        nume1.Add("omu");
        nume1.Add("taganu'");
        nume1.Add("pericolu'");
        nume1.Add("cainele");
        nume1.Add("animalu'");
        nume1.Add("scandalu'");
        nume1.Add("copilu");
        nume1.Add("nacazu'");
        nume1.Add("faraonu'");
        nume1.Add("bocteru'");
        nume1.Add("basculantu'");
        nume1.Add("junghiu'");
        nume1.Add("milmoiu'");
        nume1.Add("mazgoiu");
        nume1.Add("opelu'");
        nume1.Add(" talharu'");
        nume1.Add(" belferu'");
        nume2.Add(" rau");
        nume2.Add(" spurcat");
        nume2.Add(" periculos");
        nume2.Add(" salbatic");
        nume2.Add(" scandal");
        nume2.Add(" infiorator");
        nume2.Add(" dusmanos");
        nume2.Add(" durerii");
        nume2.Add(" nacaz");
        nume2.Add(" nacajit");
        nume2.Add(" din animeuri");
        nume2.Add(" had");
        nume2.Add(" hait");
        nume2.Add(" chinezesc");
        nume2.Add(" tradarii");
        nume2.Add(" accident");
        nume2.Add(" blastamat");
        nume2.Add(" distrugator");
        nume2.Add(" puterii");
        nume2.Add(" mortii");
        nume2.Add(" lorbant");
        nume2.Add(" terminatoru");
        nume2.Add(" nebun");
        nume2.Add(" ciumei");
        nume2.Add(" intunecat mandru");
        nume2.Add(" intunecat");
        nume2.Add(" tumultos");
        nume2.Add(" taganesc");
        nume2.Add(" din filmele matrix");
        nume2.Add(" buclucas");
        nume2.Add(" sarac");
        nume2.Add(" derbedeu");

        text.text = (nume1[Random.Range(0, nume1.Count)] + nume2[Random.Range(0, nume2.Count)]);
    }

    public void newPlane()
    {
        Vector3 initial = new Vector3(501, 5, 510);
        sHelper.symPlane = Object.Instantiate(GameObject.Find("Plane"), initial, Quaternion.identity);
        sHelper.getDirection();
    }

    private void getDirection()
    {
        currentDSign = -1;
        float maxX = sHelper.symPlane.GetComponent<MeshRenderer>().bounds.size.x;
        float currentDirection = maxX / 2 + sHelper.symPlane.transform.position.x;
        if (currentDirection > sHelper.symPlane.transform.position.x)
        {
            currentDSign = 1;
        }
    }

    //Methods (B)
}
