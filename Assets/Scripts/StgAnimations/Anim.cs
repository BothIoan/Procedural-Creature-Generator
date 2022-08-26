using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Anim:MonoBehaviour
{
    //ClassWiring

    SHelper sHelper;
    MHelper mHelper;
    AHelper aHelper;

    //ClassWiring

    //SingletonConstructor

    private static Anim anim;
    public static Anim Inst()
    {
        return anim;
    }

    private void Awake()
    {
        anim = this;
        //instantiate lists
        sHelper = SHelper.Inst();
        mHelper = MHelper.Inst();
        aHelper = AHelper.Inst();
    }

    //SingletonConstructor



    //Methods (B)

    public void AnimationStage()
    {
        HeadRiggingStage();
        ArmRiggingStage();
        LegRiggingStage();
    }


    //RiggingB

    private void HeadRiggingStage()
    {
        RigBuilder rigbuilder = sHelper.symPlane.GetComponent<RigBuilder>();
        rigbuilder.layers.Clear();
        GameObject rig = new GameObject();
        rig.name = "HeadRig";
        rig.transform.parent = sHelper.symPlane.transform;
        Rig headRig = rig.AddComponent<Rig>();
        aHelper.headTarget = rig;
        rigbuilder.layers.Add(new RigLayer(headRig, true));
        Vector3 position = mHelper.lstJoints[mHelper.lstJoints.Count - 1].transform.position;
        position.x += 2;
        rig.transform.position = position;
        MultiAimConstraint constraint = rig.AddComponent<MultiAimConstraint>();
        constraint.data.constrainedObject = aHelper.neck.transform;
        constraint.data.upAxis = MultiAimConstraintData.Axis.Y;
        constraint.data.aimAxis = MultiAimConstraintData.Axis.X;
        constraint.data.sourceObjects.Clear();
        var sourceObject = rig.GetComponent<MultiAimConstraint>().data.sourceObjects;
        sourceObject.Add(new WeightedTransform(rig.transform, 1));
        rigbuilder.layers[0].rig.GetComponent<MultiAimConstraint>().data.sourceObjects = sourceObject;
        constraint.data.constrainedXAxis = true;
        constraint.data.constrainedYAxis = true;
        constraint.data.constrainedZAxis = true;
        constraint.data.limits = new Vector2(-50, 50);
        constraint.data.worldUpAxis = MultiAimConstraintData.Axis.Y;
        rigbuilder.graph.Destroy();
        rigbuilder.Build();
    }

    private void ArmRiggingStage()
    {
        int i = 0;
        RigBuilder rigbuilder = sHelper.symPlane.GetComponent<RigBuilder>();
        if (aHelper.armReferences == null) return;
        aHelper.armReferences.ForEach(arm => {
            GameObject rig = new GameObject();
            Rig armRig = rig.AddComponent<Rig>();
            rigbuilder.layers.Add(new RigLayer(armRig, true));
            rig.name = "armsTarget" + i;
            rig.transform.parent = sHelper.symPlane.transform;
            Vector3 position = mHelper.lstJoints[mHelper.lstJoints.Count - 1].transform.position;
            position.x += 1.5f;
            position.y += -2;
            rig.transform.position = position;
            TwoBoneIKConstraint constraint = rig.AddComponent<TwoBoneIKConstraint>();
            aHelper.armTargets.Add(constraint);
            arm.name = "arm" + i;
            i++;
            constraint.data.tip = arm.transform;
            arm.transform.parent.name = "arm" + i;
            i++;
            constraint.data.mid = arm.transform.parent;
            arm.transform.parent.parent.name = "arm" + i;
            i++;
            constraint.data.root = arm.transform.parent.parent;
            constraint.data.target = rig.transform;
            constraint.data.targetPositionWeight = 1;
            constraint.data.targetRotationWeight = 1;
            constraint.weight = 0;
            aHelper.increase = true;
            rigbuilder.graph.Destroy();
            rigbuilder.Build();
        });
    }

    private void LegRiggingStage()
    {
        int i = 0;
        RigBuilder rigbuilder = sHelper.symPlane.GetComponent<RigBuilder>();
        if (aHelper.legReferences == null) return;
        float targetPosition = sHelper.symPlane.transform.position.y / 2;
        aHelper.legReferences.ForEach(x => {
            GameObject rig = new GameObject();
            Rig legRig = rig.AddComponent<Rig>();
            rigbuilder.layers.Add(new RigLayer(legRig, true));
            rig.name = "legsTarget" + i;
            rig.transform.parent = sHelper.symPlane.transform;
            Vector3 position = x.transform.position;
            position.y = targetPosition > x.transform.parent.parent.position.y ? x.transform.parent.parent.position.y : targetPosition;
            rig.transform.position = position;
            TwoBoneIKConstraint constraint = rig.AddComponent<TwoBoneIKConstraint>();
            aHelper.legTargets.Add(constraint);
            x.transform.parent.name = "leg" + i;
            i++;
            constraint.data.tip = x.transform.parent;
            x.transform.parent.parent.name = "leg" + i;
            i++;
            constraint.data.mid = x.transform.parent.parent;
            x.transform.parent.parent.parent.name = "leg" + i;
            i++;
            constraint.data.root = x.transform.parent.parent.parent;
            constraint.data.target = rig.transform;
            constraint.data.targetPositionWeight = 1;
            constraint.data.targetRotationWeight = 1;
            constraint.weight = 0;
            aHelper.increase = true;
            rigbuilder.graph.Destroy();
            rigbuilder.Build();
        });
    }

    //RiggingE



    //MovementB

    public void grabAnimation()
    {
        SHelper.CloseWarningMessage();
        Setup.DefocusEverything();
        aHelper.armsAnimated = true;
        aHelper.armTargets.ForEach(x => {
            if (!aHelper.increase)
            {
                x.weight -= 0.007f;
                if (x.weight == 0)
                {
                    aHelper.increase = true;
                    aHelper.armsAnimated = false;
                }
            }
            else
            {
                x.weight += 0.01f;
                if (x.weight == 1)
                {
                    aHelper.increase = false;
                }
            }
        });
    }

    public void walkDecay()
    {
        aHelper.legTargets.ForEach(x =>{
            if (x.weight > 0)
                x.weight -= 0.01F;

        });
    }
    public void WalkAnimation()
    {
        if (aHelper.weight >= 1 || aHelper.weight <= 0) aHelper.quantity *= -1;
        aHelper.weight += aHelper.quantity;

        //left alternates right + offset
        //aHelper.legTargets[0].weight = 1F - aHelper.weight;
        //aHelper.legTargets[aHelper.legTargets.Count-1].weight = aHelper.weight;
        NodeCList<int> node = aHelper.patternList.head;
        for (int index = 0; index < aHelper.legTargets.Count; index ++)
        {
            aHelper.legTargets[index].weight = (1 - aHelper.weight)*(node.value) + (aHelper.weight) * (1 - node.value);
            node = node.next;
        }

        //left alternates right
        /*
        for(int index = 0; index< aHelper.legTargets.Count; index += 2)
        {
            TwoBoneIKConstraint legRight = aHelper.legTargets[index];
            TwoBoneIKConstraint legLeft = aHelper.legTargets[index +1];
            legRight.weight = aHelper.weight;
            legLeft.weight = 1F- aHelper.weight;
        }
        */

        //back alternates front
        /*
        for (int index = 0; index < aHelper.legTargets.Count/2; index ++)
        {
            TwoBoneIKConstraint legRight = aHelper.legTargets[index];
            legRight.weight = aHelper.weight;
        }
        for (int index = aHelper.legTargets.Count / 2 +1; index < aHelper.legTargets.Count; index++)
        {
            TwoBoneIKConstraint legLeft = aHelper.legTargets[index];
            legLeft.weight = 1F - aHelper.weight;
        }
        */
    }
  

    // Update is called once per frame
    private void Update()
    {
        if (anim == null || aHelper.headTarget == null) return;
        Vector3 pivot = new Vector3(500, 0, 500);
        aHelper.headTarget.transform.RotateAround(pivot, Vector3.up, 10 * Time.deltaTime);
        if (aHelper.armsAnimated)
        {
           grabAnimation();
        }
        if(Input.GetAxis("Vertical") > 0)
        {
            WalkAnimation();
        }
        else
        {
            walkDecay();
        }
    }

    //MovementE

    //Methods (E)

    // do this https://forum.unity.com/threads/add-update-function-to-new-gameobject.217295/
}
