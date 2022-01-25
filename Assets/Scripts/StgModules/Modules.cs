using UnityEngine;
using UnityEngine.Animations.Rigging;

using System.Collections.Generic;
using UnityEditor;
using System.Threading.Tasks;
public class Modules : MonoBehaviour
{




    //temps
    public static List<IModule> iModules;

    public HSpineMod hSpine;
    public TailMod tail;
    public LegsMod legs;
    public SpiderLegsMod spider;
    public VSpineMod vSpine;
    public ArmsMod arms;
    public WingsMod wings;
    public BatwingsMod batwings;
    public HeadMod head;
    public GrammarMod grammar;
    //temps



    //ClassWiring
    private MHelper mHelper;
    private AHelper aHelper;
    private SHelper sHelper;
    //ClassWiring



    //SingletonConstructor

    private static Modules modules;
    public static Modules Inst(){
        return modules;
    }

    private void Awake()
    {
        modules = this;
        mHelper = MHelper.Inst();
        aHelper = AHelper.Inst();
        sHelper = SHelper.Inst();
        iModules = new List<IModule>();
    }

    //SingletonConstructor



    //Methods (B)
    public void InstModules()
    {
        grammar = new GrammarMod();
        hSpine = new HSpineMod(new List<GameObject>());
        tail = new TailMod(hSpine);
        legs = new LegsMod(hSpine);
        spider = new SpiderLegsMod(hSpine);
        vSpine = new VSpineMod(hSpine, new List<GameObject>());
        arms = new ArmsMod(vSpine);
        wings = new WingsMod(vSpine);
        batwings = new BatwingsMod(vSpine);
        head = new HeadMod(hSpine);

        iModules.Add(grammar);
        iModules.Add(hSpine);
        iModules.Add(tail);
        iModules.Add(legs);
        iModules.Add(spider);
        iModules.Add(vSpine);
        iModules.Add(arms);
        iModules.Add(wings);
        iModules.Add(batwings);
        iModules.Add(head);
    }

    public void Cleanup()
    {
        iModules.ForEach(x =>{
            x.Cleanup();
        });
    }
    //Methods (E)
}

public class VSpineMod : IModule
{

    public VSpineMod(IModule parentModule, List<GameObject> outJointL) : base(5,parentModule,outJointL) { }

    public override void Gen()
    {
        SetInJoint(parentModule.getOutJointL()[0]);

        int nrOfJoints = RememberPadding(1, 3);
        Vector3 jointP = sHelper.symPlane.transform.position;
        float distance = RandOvr(1f, 1.5f);
        float incline = RandOvr(0f, 2f);

        for (int i = 0; i < nrOfJoints; i++)
        {
            jointP.x = sHelper.symPlane.transform.position.x + incline;
            jointP.y = sHelper.symPlane.transform.position.y + distance;
            GameObject joint = Object.Instantiate(mHelper.joint, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = inJoint.transform;
            else joint.transform.parent = outJointL[i - 1].transform;
            mHelper.lstJoints.Add(joint);
            outJointL.Add(joint);
            float distanceOffset = RandOvr(2f, 3f);
            distance += distanceOffset;
            incline += incline;
        }
        AddPadding();
        outJoint = outJointL[nrOfJoints - 1];
        
    }
}

public class HSpineMod: IModule
{
    public HSpineMod(List<GameObject> outJointL) : base(7,outJointL) { }
    //inJoint is last joint (for tail)
    //inJointL[0] is first joint (for head/ Vspine)

    public override void Gen()
    {
        
        int nrOfJoints = RememberPadding(1, 5); 
        Vector3 jointP = sHelper.symPlane.transform.position;
        jointP.y += RandOvr(1f, -1f);
        
        float distance = 0;
        float incline = RandOvr(-0.5f, 0.5f);
        for (int i = 0; i < nrOfJoints; i++)
        {
            jointP.y = sHelper.symPlane.transform.position.y + incline;
            jointP.x = sHelper.symPlane.transform.position.x + distance;
            GameObject joint = Object.Instantiate(mHelper.joint, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = sHelper.symPlane.transform;
            else joint.transform.parent = outJointL[i - 1].transform;
            mHelper.lstJoints.Add(joint);
            outJointL.Add(joint);
            distance += RandOvr(-2f, -3f);
            incline += incline;
        }
        AddPadding();
        outJoint = outJointL[outJointL.Count-1];
    }

}

public class LegsMod: IModule
{
    public LegsMod(IModule parentModule) : base(30,parentModule) { }

    public override void Gen()
    {
        SetInJointL(parentModule.getOutJointL());

        //legs must happen between the current point and the ground.
        float yDown = 0.2f;
        //choose position of knee:
        float yUp = sHelper.symPlane.transform.position.y;
        float kneeY = RandOvr(yUp/2, yDown);

        aHelper.legReferences = new List<GameObject>();

        float distanceZ = RandOvr(0f, 3f);
        inJointL.ForEach(x =>
        {
            int nrLegsCurrentJoint = RememberPadding(1, 3);
            for (int i = 0; i < nrLegsCurrentJoint; i++)
            {
                float distanceX = RandOvr(0f, -1.5f);
                MHelper.symPair firstJoint = mHelper.MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ, mHelper.lstJoints);
                MHelper.symPair secondJoint = mHelper.MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + RandOvr(0f, 1f), mHelper.lstJoints);
                MHelper.symPair thirdJoint = mHelper.MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, sHelper.currentDSign * (distanceX + RandOvr(0f, 1f)), x.transform.position.y, distanceZ, mHelper.lstJoints);
                mHelper.MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, sHelper.currentDSign * (distanceX + 1) - 1, x.transform.position.y, distanceZ, mHelper.lstJoints);
                aHelper.legReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 1]);
                aHelper.legReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 2]);
            }
            AddPadding(padding * 3);
        });
        AddPadding((4 - inJointL.Count) * 7);
    }
}

public class SpiderLegsMod : IModule
{
    public SpiderLegsMod(IModule parentModule) : base(40,parentModule) { }

    public override void Gen()
    {
        SetInJointL(parentModule.getOutJointL());

        //legs must happen between the current point and the ground.
        //choose position of knee:
        float kneeY = RandOvr(-1f, -3f);
        float kneeY2 = kneeY + RandOvr(1f, 2f);
        float kneeY3 = kneeY2 + RandOvr(1f, 2f);
        float yUp = sHelper.symPlane.transform.position.y;
        aHelper.legReferences = new List<GameObject>();
        float distanceZ = RandOvr(0f, 3f);
        inJointL.ForEach(x =>
        {
            float distanceX = RandOvr(0f, -1.5f);
            for (int i = 0; i < 2; i++)
            {
                if (i == 1) distanceX *= -1;
                MHelper.symPair firstJoint = mHelper.MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ, mHelper.lstJoints);
                MHelper.symPair secondJoint = mHelper.MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + RandOvr(0f, 1f), mHelper.lstJoints);
                MHelper.symPair thirdJoint = mHelper.MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX, kneeY2, distanceZ + RandOvr(1f, 2f), mHelper.lstJoints);
                MHelper.symPair fourthJoint = mHelper.MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, distanceX, kneeY3, distanceZ + RandOvr(1f, 2f), mHelper.lstJoints);
                mHelper.MirrorCreate(x.transform, fourthJoint.transform1, fourthJoint.transform2, sHelper.currentDSign * (distanceX + RandOvr(0f, 1f)), x.transform.position.y, distanceZ, mHelper.lstSpikes);
                aHelper.legReferences.Add(mHelper.lstSpikes[mHelper.lstSpikes.Count - 1]);
                aHelper.legReferences.Add(mHelper.lstSpikes[mHelper.lstSpikes.Count - 2]);
            }
        });
        AddPadding((4 - inJointL.Count) * 9);
        //5;
    }
}

public class ArmsMod : IModule
{
    public ArmsMod(IModule parentModule) : base(30,parentModule) { }

    public override void Gen()
    {
        SetInJointL(parentModule.getOutJointL());
        inJointL.ForEach(x =>
        {
            int nrArmsCurrentJoint = RememberPadding(1, 3);
            for (int i = 0; i < nrArmsCurrentJoint; i++)
            {
                float armKnee = RandOvr(1f, 2f);
                float distanceZ = RandOvr(0.1f, 0.5f);
                float distanceX = RandOvr(0.5f, -2f);
                MHelper.symPair firstJoint = mHelper.MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ, mHelper.lstJoints);
                MHelper.symPair secondJoint = mHelper.MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, RandOvr(0.5f, 1f), distanceZ + armKnee, mHelper.lstJoints);
                mHelper.MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX + RandOvr(-0.5f, -1f), RandOvr(0.5f, 1f), RandOvr(0, 2.5f), mHelper.lstJoints);
                aHelper.armReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 1]);
                aHelper.armReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 2]);
            }
            AddPadding(padding * 7);
        });
        AddPadding((2 - inJointL.Count)* 15 );
    }
}

public class ArchwingMod : IModule
{
    public ArchwingMod(IModule parentModule) : base(0,parentModule) { }

    public override void Gen()
    {
        SetInJoint(parentModule.getOutJoint());
        SetInJointL(parentModule.getOutJointL());

        float x = 0;
        float y = -0.5f;
        float z = 2;
        MHelper.symPair firstJoint = mHelper.MirrorCreate(inJoint.transform, inJoint.transform, inJoint.transform, x, y, z, mHelper.lstJoints);
        float xDFirst = x + 0;
        float yDFirst = y + 1;
        float zDFirst = z + 0.5f;
        MHelper.symPair firstDigit = mHelper.MirrorCreate(inJoint.transform, firstJoint.transform1, firstJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        float xDSecond = x + 0;
        float yDSecond = y + 2;
        float zDSecond = z + 0.8f;
        mHelper.MirrorCreate(inJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);


        x += 0;
        y += -1;
        z += 1;
        MHelper.symPair secondJoint = mHelper.MirrorCreate(inJoint.transform, firstJoint.transform1, firstJoint.transform2, x, y, z, mHelper.lstJoints);
        xDFirst += 0;
        yDFirst += -1;
        zDFirst += 2;
        firstDigit = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        xDSecond += 0;
        yDSecond += -1;
        zDSecond += 2.4f;
        MHelper.symPair SecondDigit = mHelper.MirrorCreate(inJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);
        mHelper.MirrorCreate(inJoint.transform, SecondDigit.transform1, SecondDigit.transform2, x, y + 3, z + 2.5f, mHelper.lstJoints);

        x += 0;
        y += -1;
        z += 1;
        MHelper.symPair thirdJoint = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, x, y, z, mHelper.lstJoints);
        //f
        xDFirst += 0;
        yDFirst += -2;
        zDFirst += 1;
        firstDigit = mHelper.MirrorCreate(inJoint.transform, thirdJoint.transform1, thirdJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        xDSecond += 0;
        yDSecond += -2.5f;
        zDSecond += 1.8f;
        SecondDigit = mHelper.MirrorCreate(inJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);
        mHelper.MirrorCreate(inJoint.transform, SecondDigit.transform1, SecondDigit.transform2, x, y + 1, z + 4, mHelper.lstJoints);

        x += 0;
        y += -1;
        z += 1;
        MHelper.symPair fourthJoint = mHelper.MirrorCreate(inJoint.transform, thirdJoint.transform1, thirdJoint.transform2, x, y, z, mHelper.lstJoints);
        //f
        xDFirst += 0;
        yDFirst += -1;
        zDFirst += -0.5f;
        firstDigit = mHelper.MirrorCreate(inJoint.transform, fourthJoint.transform1, fourthJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        xDSecond += 0;
        yDSecond += -1.6f;
        zDSecond += -1;
        mHelper.MirrorCreate(inJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);
    }
}

public class BatwingsMod: IModule
{
    public BatwingsMod(IModule parentModule) : base(0,parentModule) { }

    public override void Gen()
    {
        SetInJoint(parentModule.getOutJoint());

        float x = 1;
        float y = -1.5f;
        float z = 2;
        MHelper.symPair firstJoint = mHelper.MirrorCreate(inJoint.transform, inJoint.transform, inJoint.transform, x, y, z, mHelper.lstJoints);
        x += 0;
        y += -3;
        z += +2;
        MHelper.symPair secondJoint = mHelper.MirrorCreate(inJoint.transform, firstJoint.transform1, firstJoint.transform2, x, y, z, mHelper.lstJoints);

        //first digit
        float xD = x;
        float yD = y - 1;
        float zD = z + 1;
        MHelper.symPair digitFirst = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += -0.5f;
        zD += 0;
        mHelper.MirrorCreate(inJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstSpikes);

        //second digit
        xD = x;
        yD = y + 0.4f;
        zD = z + 2.8f;
        digitFirst = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 0.6f;
        zD += 0.6f;
        mHelper.MirrorCreate(inJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstSpikes);


        //third digit
        xD = x;
        yD = y + 3;
        zD = z + 2.6f;
        digitFirst = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 2.5f;
        zD += 0.2f;
        MHelper.symPair digitSecond = mHelper.MirrorCreate(inJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 2;
        zD += -0.8f;
        mHelper.MirrorCreate(inJoint.transform, digitSecond.transform1, digitSecond.transform2, xD, yD, zD, mHelper.lstSpikes);

        //fourth digit
        xD = x;
        yD = y + 4;
        zD = z + 1.5f;
        digitFirst = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 1.5f;
        zD += -0.5f;
        digitSecond = mHelper.MirrorCreate(inJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 0.5f;
        zD += -1;
        mHelper.MirrorCreate(inJoint.transform, digitSecond.transform1, digitSecond.transform2, xD, yD, zD, mHelper.lstSpikes);

        //fifth digit
        digitFirst = mHelper.MirrorCreate(inJoint.transform, secondJoint.transform1, secondJoint.transform2, x, y + 4.2f, z, mHelper.lstJoints);
        mHelper.MirrorCreate(inJoint.transform, digitFirst.transform1, digitFirst.transform2, x, y + 5, z - 0.5f, mHelper.lstSpikes);
    }
}

public class WingsMod: IModule
{
    public WingsMod(IModule parentModule) : base(17,parentModule) { }

    public override void Gen()
    {
        SetInJoint(parentModule.getOutJoint());

        float distance = RandOvr(1.5f, 1.75f);
        float logIncline = RandOvr(-0f, -0.1f);
        float linearDifference = RandOvr(-0.25f, -0.5f);
        float linearIncline = linearDifference;
        float offset = RandOvr(-0.5f, -1f);
        int nrJoints = RememberPadding(5, 8);
        List<MHelper.symPair> joints = new List<MHelper.symPair>();
        MHelper.symPair parent = new MHelper.symPair(inJoint.transform, inJoint.transform);
        //bota
        for (int i = 0; i < nrJoints; i++)
        {
            parent = mHelper.MirrorCreate(inJoint.transform, parent.transform1, parent.transform2, 0, logIncline + linearIncline + offset, distance, mHelper.lstJoints);
            joints.Add(parent);
            logIncline += logIncline;
            linearIncline += linearDifference;
            distance += RandOvr(1.5f, 2f);
        }
        AddPadding();

        //pene
        float yLinearDistance = RandOvr(0.5f, 1f);
        float zLinearDistance = RandOvr(0.5f, 0f);
        int maximaPoint = nrJoints / 2 + RandOvr(0, 2);
        float yLogIncline = RandOvr(+0.2f, +0.5f);
        float firstCoef = 2;
        float secondCoef = 2;
        float zLogIncline = RandOvr(0.01f, 0.012f);
        for (int i = 0; i < nrJoints; i++)
        {
            zLogIncline += zLogIncline;
            if (i == maximaPoint)
            {
                yLogIncline += yLinearDistance;
            }
            float startY = inJoint.transform.position.y - joints[i].transform1.position.y;
            float endZ = inJoint.transform.position.z - joints[i].transform1.position.z + zLinearDistance + zLogIncline;
            if (i >= maximaPoint)
            {

                float endY = yLogIncline + inJoint.transform.position.y - joints[i].transform1.position.y;
                MHelper.symPair middlePhalanx = mHelper.MirrorCreate(inJoint.transform, joints[i].transform1, joints[i].transform2, 0, (endY - startY) * 3 / 4 + startY, endZ + 0.5f, mHelper.lstJoints);
                MHelper.symPair lastPhlanx = mHelper.MirrorCreate(inJoint.transform, middlePhalanx.transform1, middlePhalanx.transform2, 0, endY, endZ, mHelper.lstSpikes);

                yLogIncline /= secondCoef;
            }
            else
            {
                float endY = yLogIncline + yLinearDistance + inJoint.transform.position.y - joints[i].transform1.position.y;
                MHelper.symPair middlePhalanx = mHelper.MirrorCreate(inJoint.transform, joints[i].transform1, joints[i].transform2, 0, (endY - startY) * 3 / 4 + startY, endZ + 0.5f, mHelper.lstJoints);
                mHelper.MirrorCreate(inJoint.transform, middlePhalanx.transform1, middlePhalanx.transform2, 0, endY, endZ, mHelper.lstSpikes);
                yLogIncline *= firstCoef;
            }
        }

    }
}

public class HeadMod: IModule
{
    public HeadMod(IModule parentModule) : base(3,parentModule) { }

    public override void Gen()
    {
        //SetInJoint(parentModule.getOutJoint());

        //Stack<float> rands = Categ.GetInfo();
        Vector3 headP = inJoint.transform.position;
        headP.y += RandOvr(0f, 2f);
        headP.x += RandOvr(0f, 2f);
        GameObject head = Object.Instantiate(mHelper.joint, headP, Quaternion.identity);
        head.transform.parent = inJoint.transform;
        head.name = "j0";
        aHelper.neck = head;
        mHelper.lstJoints.Add(head);
        GameObject skull = Object.Instantiate(mHelper.skulls[(RandOvr(0, mHelper.skulls.Count-1)) ], headP, Quaternion.Euler(new Vector3(0, 180, 0)));
        skull.transform.parent = head.transform;
        mHelper.lstJoints.Add(skull);
    }
}

public class TailMod : IModule
{
    public TailMod(IModule parentModule) : base(2,parentModule) { }

    public override void Gen()
    {
        SetInJoint(parentModule.getOutJoint());

        float xDistance = RandOvr(-1f, -1.5f);
        int nrSegments = RandOvr(4, 11);
        // aici tre pus in-u
        Transform parent = inJoint.transform;
        Vector3 position = parent.transform.position;
        position.x += xDistance;
        for (int i = 0; i < nrSegments; i++)
        {
            GameObject tailSegment = Object.Instantiate(mHelper.joint, position, Quaternion.identity);
            tailSegment.transform.parent = parent;
            mHelper.lstJoints.Add(tailSegment);
            position.x += xDistance;
            parent = tailSegment.transform;

        }
        GameObject spike = mHelper.lstJoints[mHelper.lstJoints.Count - 1];
        mHelper.lstSpikes = new List<GameObject>();
        mHelper.lstSpikes.Add(spike);
        mHelper.lstJoints.RemoveAt(mHelper.lstJoints.Count - 1);
    }

}
//mai tre restu' modulelor.

