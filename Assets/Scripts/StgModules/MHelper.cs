using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MHelper
{
    //ClassWiring
    private static SHelper sHelper;
    //ClassWiring

    //SingletonConstructor

    private static MHelper mHelper;
    public static MHelper Inst()
    {
        if (mHelper == null)
        {
            mHelper = new MHelper();
            sHelper = SHelper.Inst();
            //instantiate All that needs to be instantiated.
            mHelper.lstJoints = new List<GameObject>();
            mHelper.lstBones = new List<GameObject>();
            mHelper.lstSpikes = new List<GameObject>();
            mHelper.joint = GameObject.Find("Joint");
            mHelper.spidery = false;
            mHelper.bone = GameObject.Find("bone");
            mHelper.spike = GameObject.Find("spike");
            mHelper.skulls = new List<GameObject>();
            mHelper.skulls.Add(GameObject.Find("ravenSkull"));
            mHelper.skulls.Add(GameObject.Find("humanSkull"));
            mHelper.skulls.Add(GameObject.Find("horseSkull"));
            mHelper.skulls.Add(GameObject.Find("crocodileSkull"));
        }
        return mHelper;
    }

    //SingletonConstructir


    //Enums (B)

    //Bone textures
    enum textureType
    {
        Bone,
        Spike
    }

    //Enums (E)


    //Globals (B)

    //dataB
    public bool spidery;
    private int keyCounter = -1;
    //dataE

    //flagsB
    public bool ganGenerated = false;
    //flagsE

    //texturesB
    public GameObject joint;
    public GameObject spike;
    public GameObject bone;
    public List<GameObject> skulls;
    //texturesE

    //listsB
    public List<GameObject> lstJoints;
    public List<GameObject> lstBones;
    public List<GameObject> lstSpikes;
    //listsE

    //Globals (E)
    


    //Structures (B)

    //Used to store two objects (equidistant to another object)
    public struct symPair
    {
        public Transform transform1;
        public Transform transform2;

        public symPair(Transform transform1, Transform transform2) : this()
        {
            this.transform1 = transform1;
            this.transform2 = transform2;
        }
    }

    //Structures (E)



    //Methods (B)

    //Used to create 2 objects in mirror with respect to the simmetryPlane
    public symPair MirrorCreate(Transform symPlanT, Transform parent1T, Transform parent2T, float distanceX, float distanceY, float distanceZ, List<GameObject> textureList)
    {
        Vector3 initial = symPlanT.position;

        initial.y -= distanceY;
        initial.x -= distanceX;
        Vector3 pos1 = initial;
        pos1.z -= distanceZ;
        GameObject g1 = Object.Instantiate(mHelper.joint, pos1, Quaternion.identity);
        g1.transform.parent = parent1T;
        textureList.Add(g1);

        Vector3 pos2 = initial;
        pos2.z += distanceZ;
        GameObject g2 = Object.Instantiate(mHelper.joint, pos2, Quaternion.identity);
        g2.transform.parent = parent2T;
        textureList.Add(g2);
        return new symPair(g1.transform, g2.transform);
    }

    public void DrawBonesStage()
    {

        mHelper.lstJoints.ForEach(x => {
            if (x.transform.parent != null && x.transform.parent != sHelper.symPlane.transform)
            {
                Transform tempParent = x.transform.parent;
                x.transform.parent = null;
                GameObject newBone = GameObject.Instantiate(bone, x.transform.position, Quaternion.FromToRotation(Vector3.up, tempParent.transform.position - x.transform.position));
                newBone.transform.parent = null;
                newBone.transform.localScale = new Vector3(0, newBone.transform.localScale.y, 0);
                Vector3 minC = newBone.GetComponent<Renderer>().bounds.min;
                Vector3 maxC = newBone.GetComponent<Renderer>().bounds.max;
                float distC = Vector3.Distance(minC, maxC);
                float distN = Vector3.Distance(tempParent.transform.position, x.transform.position);
                float scaleN = distN / distC;
                newBone.transform.localScale = new Vector3(0.5f, scaleN * 1.1f, 0.5f);
                newBone.transform.parent = tempParent.transform;
                x.transform.parent = tempParent;
                mHelper.lstBones.Add(newBone);
            }
        });
        mHelper.lstSpikes.ForEach(x => {
            if (x.transform.parent != null && x.transform.parent != sHelper.symPlane.transform)
            {
                Transform tempParent = x.transform.parent;
                x.transform.parent = null;
                GameObject newBone = GameObject.Instantiate(spike, x.transform.position, Quaternion.FromToRotation(Vector3.up, tempParent.transform.position - x.transform.position));
                newBone.transform.parent = null;
                newBone.transform.localScale = new Vector3(0, newBone.transform.localScale.y, 0);
                Vector3 minC = newBone.GetComponent<Renderer>().bounds.min;
                Vector3 maxC = newBone.GetComponent<Renderer>().bounds.max;
                float distC = Vector3.Distance(minC, maxC);
                float distN = Vector3.Distance(tempParent.transform.position, x.transform.position);
                float scaleN = distN / distC;
                newBone.transform.localScale = new Vector3(0.5f, scaleN * 1.1f, 0.5f);
                newBone.transform.parent = tempParent.transform;
                x.transform.parent = tempParent;
                mHelper.lstBones.Add(newBone);
            }
        });
    }

    public int GetModuleKey()
    {
        keyCounter++;
        return keyCounter;
    }

    //Methods (E)
}
