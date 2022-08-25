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
    //dataE

    //listsB
    private List<string> nume1;
    private List<string> nume2;
    //listsE
   
    //Globals (E)

    //Methods (B)

    public void newPlane()
    {
        Vector3 initial = new Vector3(501, 5, 510);
        sHelper.symPlane = Object.Instantiate(GameObject.Find("Plane"), initial, Quaternion.identity);
        Debug.Log("Totusi exista");
        Debug.Log(sHelper.symPlane);
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
