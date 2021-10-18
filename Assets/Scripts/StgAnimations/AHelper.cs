using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AHelper
{


    //SingletonConstructor

    private static AHelper aHelper;
    public static AHelper Inst()
    {
        if (aHelper == null)
        {
            aHelper = new AHelper();
            //instantiate lists
            aHelper.armReferences = new List<GameObject>();
            aHelper.legReferences = new List<GameObject>();
            aHelper.legTargets = new List<TwoBoneIKConstraint>();
            aHelper.armTargets= new List<TwoBoneIKConstraint>();
            aHelper.armsAnimated = false;
            aHelper.increase = true;
            aHelper.sLegsAnimated = false;
            aHelper.weight = 1F;
            aHelper.quantity = 0.05F;
            aHelper.patternList = NodeCList<int>.CreateCList(new int[]{0,1,1,0},4);
        }
        return aHelper;
    }

    //SingletonConstructor



    //UsedJoints (B)  Here I keep joints needed for animations, that are created by module

    //HeadAnim
    public GameObject neck;
    public GameObject headTarget;

    //ArmsAnim 
    public List<GameObject> armReferences; //(REFERENCES ALREADY SAVED IN ANOTHER LIST. AT CLEANUP, JUST CLEAN LIST)
    public List<TwoBoneIKConstraint> armTargets;

    //LegsAnim
    public List<GameObject> legReferences; //(REFERENCES ALREADY SAVED IN ANOTHER LIST. AT CLEANUP, JUST CLEAN LIST)
    public List<TwoBoneIKConstraint> legTargets;

    //UsedJoints (E)


    //Data (B)
    public bool increase;
    public bool armsAnimated;
    public bool sLegsAnimated;
    public float weight;
    public float quantity;
    public CircularList<int> patternList;
    //Data (E)


    //Methods (B)

    public void refrencesCleanup()
    {
        legTargets.ForEach(x => {
            Object.Destroy(x);
        });
        legTargets.Clear();
        armTargets.ForEach(x => {
            Object.Destroy(x);
        });
        armTargets.Clear();
        armReferences.Clear();
        legReferences.Clear();
    }

    //Methods (E)
}
