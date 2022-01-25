using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class IModule
{
    protected List<float> unnormFeatures;
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
        unnormFeatures = new List<float>();
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
        unnormFeatures = new List<float>();
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
        unnormFeatures.Clear();
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
            normFeatures = new List<float>();
            for (int i = 0; i < featureCount; i++)
            {
                normFeatures.Add(UnityEngine.Random.Range(0f, 1f));
            }
            index = 0;
            return;
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
        int value = (int)Math.Round(normFeatures[index] * (ceiling - floor) + floor);
        index++;
        unnormFeatures.Add(value);
        return value;
    }
    protected float RandOvr(float floor, float ceiling)
    {
        float value = normFeatures[index] * (ceiling - floor) + floor;
        index++;
        unnormFeatures.Add(value);
        return value;
    }
    //used together. For first getting a random number, and then adding max - actual 0s to the serializar
    protected int RememberPadding(int floor, int ceiling)
    {
        ceiling--;
        int value = (int)Math.Round(normFeatures[index] * (ceiling - floor) + floor);
        index++;
        unnormFeatures.Add(value);
        padding = ceiling - value;
        return value;
    }
    protected void AddPadding()
    {
        for (int i = 0; i < padding; i++)
        {
            unnormFeatures.Add(0);
            normFeatures[index] = 0;
            index++;
        }
    }
    protected void AddPadding(int givenPadding)
    {
        for (int i = 0; i < givenPadding; i++)
        {
            unnormFeatures.Add(0);
            normFeatures[index] = 0;
            
            index++;
        }
    }
    //used together.
}
