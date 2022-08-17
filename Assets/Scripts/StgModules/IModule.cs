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
        if(normFeatures.Count!= 0)
        Categ.GiveDataTrue(modKey.ToString(), normFeatures);
    }
    public void GetDataGan()
    {   
        if (MHelper.ganGenerated)
        {
            Categ.RequestData(modKey.ToString());
            evt.WaitOne();
        }
        else
        {
            normFeatures = new List<float>(featureCount);
            index = 0;
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

    int Mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    protected int RandOvr(int floor, int ceiling)
    {
        
        int value;
        if (MHelper.ganGenerated)
        {
            //if (normFeatures[index] < floor || normFeatures[index] > ceiling) normFeatures[index] = normFeatures[index] % (ceiling - floor) + floor;
            normFeatures[index] = normFeatures[index] < floor ? floor : normFeatures[index];
            normFeatures[index] = normFeatures[index] > ceiling ? ceiling : normFeatures[index];

            value = (int)Math.Round(normFeatures[index]);
        }
        else 
        {
            value = UnityEngine.Random.Range(floor, ceiling + 1);
            normFeatures.Add(value);
        }
        index++;
        return value;
    }
    protected float RandOvr(float floor, float ceiling)
    {
        float value;
        if (MHelper.ganGenerated)
        {
            
            normFeatures[index] = normFeatures[index] < floor ? floor : normFeatures[index];
            normFeatures[index] = normFeatures[index] > ceiling ? ceiling : normFeatures[index];
            value = normFeatures[index];

        }
        else
        {
            value = UnityEngine.Random.Range(floor, ceiling);
            normFeatures.Add(value);
        }
        index++;
        return value;
    }
    protected int RememberPadding(int floor, int ceiling)
    {
        int value;
        ceiling--;
        if (MHelper.ganGenerated)
        {
            

            normFeatures[index] = normFeatures[index] < floor ? floor : normFeatures[index];
            normFeatures[index] = normFeatures[index] > ceiling ? ceiling : normFeatures[index];
            value = (int)Math.Round(normFeatures[index]);
        }
        else
        {
            value = UnityEngine.Random.Range(floor, ceiling + 1);
            normFeatures.Add(value);
        }
        padding = ceiling - value;
        index++;
        return value;
    }

    protected void AddPadding()
    {
        if(MHelper.ganGenerated)
            for (int i = 0; i < padding; i++)
            {
                normFeatures[index] = 0;
                index++;
            }
        else
            for (int i = 0; i < padding; i++)
            {
                normFeatures.Add(0);
                index++;
            }
    }
    protected void AddPadding(int givenPadding)
    {
        if(MHelper.ganGenerated)
            for (int i = 0; i < givenPadding; i++)
            {
                normFeatures[index] = 0;
                index++;
            }
        else
            for (int i = 0; i < givenPadding; i++)
            {
                normFeatures.Add(0);
                index++;
            }

    }
}
