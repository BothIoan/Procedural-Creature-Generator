using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IModule
{
    protected List<float> serializationList;

    protected MHelper mHelper;
    protected AHelper aHelper;
    protected SHelper sHelper;

    protected GameObject inJoint;
    protected GameObject outJoint;
    protected List<GameObject> inJointL;
    protected List<GameObject> outJointL;

    protected IModule parentModule;

    public IModule(IModule parent, List<GameObject> outL = null)
    {
        mHelper = MHelper.Inst();
        aHelper = AHelper.Inst();
        sHelper = SHelper.Inst();

        serializationList = new List<float>();
        outJointL = outL;
        parentModule = parent;
    }

    public IModule(List<GameObject> outL = null)
    {
        mHelper = MHelper.Inst();
        aHelper = AHelper.Inst();
        sHelper = SHelper.Inst();

        serializationList = new List<float>();
        outJointL = outL;
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
}
