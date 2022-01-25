using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class IModule
{
    public List<float> normFeatures;
    private int index;
    protected int padding = 0;
    protected AutoResetEvent evt;

    protected MHelper mHelper;
    protected AHelper aHelper;
    protected SHelper sHelper;

    protected GameObject inJoint;
    protected GameObject outJoint;
    protected List<GameObject> inJointL;
    protected List<GameObject> outJointL;
    

    protected IModule parentModule;

    protected int modKey = -1;
    protected int featureCount;


    //There are two constructors. When changing something pay attention to both of them
    public IModule(int featureCount, IModule parent, List<GameObject> outL = null)
    {
        mHelper = MHelper.Inst();
        aHelper = AHelper.Inst();
        sHelper = SHelper.Inst();

        evt = new AutoResetEvent(false);
        outJointL = outL;
        modKey = mHelper.GetModuleKey();
        this.featureCount = featureCount;

        parentModule = parent;
    }
    public IModule(int featureCount,List<GameObject> outL = null)
    {
        mHelper = MHelper.Inst();
        aHelper = AHelper.Inst();
        sHelper = SHelper.Inst();

        evt = new AutoResetEvent(false);
        outJointL = outL;
        modKey = mHelper.GetModuleKey();
        this.featureCount = featureCount;
    }

    
    public void Cleanup()
    {
        inJoint = null;
        outJoint = null;
        if(inJointL != null)
        inJointL.Clear();
        if(outJointL != null)
        outJointL.Clear();
    }
    public abstract void Gen();
 

    public GameObject getOutJoint()
    {
        return outJoint;
    }
    public List<GameObject> getOutJointL()
    {
        return outJointL;
    }
    public void SetInJoint(GameObject iJ)
    {
        inJoint = iJ;
    }
    public void SetInJointL(List<GameObject> iJL)
    {
        inJointL = iJL;
    }
    public void SetParent(IModule parentModule)
    {
        this.parentModule = parentModule;
    }

    public void ReceiveGen(List<float> normFeatures)
    {
        this.normFeatures = normFeatures;
        index = 0;
        evt.Set();
    }
    public void DataToGan()
    {
        Categ.GiveDataTrue(modKey.ToString(), normFeatures);
    }
    public void GetDataGan()
    {   
        if (mHelper.ganGenerated)
        {
            Categ.RequestData(modKey.ToString());
            evt.WaitOne();
        }
        else
        {
            normFeatures = new List<float>(featureCount);
        }
    }

    public void MakeGan(){
        if (featureCount == 0) return;
        THelper.activeModules.Add(modKey, this);
        Categ.MakeGan(modKey.ToString(), featureCount.ToString());
        evt.WaitOne();
    }

    public void ReceiveGanMade()
    {
        evt.Set();
    }

    protected int RandOvr(int floor, int ceiling)
    {
        int value;
        if (mHelper.ganGenerated)
        {
            value = UnityEngine.Random.Range(floor, ceiling + 1);
            normFeatures[index] = value;
        }
        else 
        {
            value = (int)Math.Round(normFeatures[index]);
        }
        index++;
        return value;
    }
    protected float RandOvr(float floor, float ceiling)
    {
        if (mHelper.ganGenerated)
        {
            return UnityEngine.Random.Range(floor, ceiling);
        }
        float value = normFeatures[index];
        index++;
        return value;
    }
}
