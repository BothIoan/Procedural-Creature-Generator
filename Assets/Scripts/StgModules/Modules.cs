using UnityEngine;
using UnityEngine.Animations.Rigging;

using System.Collections.Generic;
using UnityEditor;
public class Modules : MonoBehaviour
{
    //temps
    public GameObject tempHJoint;
    public GameObject tempVJoint;
    public List<GameObject> hSpine;
    public List<GameObject> vSpine;
    public List<GameObject> tailStumps;
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
        //instantiate lists
        sHelper = SHelper.Inst();
    }

    //SingletonConstructor



    //Methods (B)
    public void VSpineStage()
    {
        int nrOfJoints = Serializer.RandOvr(1, 3);
        //temporary disable 
        // nrOfJoints = 1;
        vSpine = new List<GameObject>();
        Vector3 jointP = sHelper.symPlane.transform.position;
        float distance = Serializer.RandOvr(1f, 1.5f);

        float incline = Serializer.RandOvr(-1f, 1f);

        for (int i = 0; i < nrOfJoints; i++)
        {
            jointP.x = sHelper.symPlane.transform.position.x + incline;
            jointP.y = sHelper.symPlane.transform.position.y + distance;
            GameObject joint = Instantiate(mHelper.joint, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = tempHJoint.transform;
            else joint.transform.parent = vSpine[i - 1].transform;
            mHelper.lstJoints.Add(joint);
            vSpine.Add(joint);
            float distanceOffset = Serializer.RandOvr(2f, 3f);
            distance += distanceOffset;
            incline += incline;
        }
        tempVJoint = vSpine[nrOfJoints - 1];
    }

    public void HSpineStage()
    {

        int nrOfJoints = Serializer.RandOvr(1, 5);
        Vector3 jointP = sHelper.symPlane.transform.position;
        jointP.y += Serializer.RandOvr(1f, -1f);
        float distance = 0;
        float incline = Serializer.RandOvr(-0.5f, 0.5f);
        hSpine = new List<GameObject>();
        tailStumps = new List<GameObject>();
        for (int i = 0; i < nrOfJoints; i++)
        {
            jointP.y = sHelper.symPlane.transform.position.y + incline;
            jointP.x = sHelper.symPlane.transform.position.x + distance;
            GameObject joint = (GameObject)Instantiate(mHelper.joint, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = sHelper.symPlane.transform;
            else joint.transform.parent = hSpine[i - 1].transform;
            mHelper.lstJoints.Add(joint);
            hSpine.Add(joint);
            distance += Serializer.RandOvr(-2f, -3f);
            incline += incline;
        }

        tailStumps.Add(hSpine[nrOfJoints - 1]);
        tempHJoint = hSpine[0];
        //hack to be able to remove the arms stage
        tempVJoint = hSpine[0];
    }



    public void LegsStage()
    {
        mHelper.spidery = false;
        if (Serializer.RandOvr(0, 2) == 0)
        {
            mHelper.spidery = true;
        }
        //legs must happen between the current point and the ground.
        float yDown = 0.1f;
        //choose position of knee:
        float kneeY;
        float kneeY2 = 0;
        float kneeY3 = 0;
        float yUp = sHelper.symPlane.transform.position.y;
        if (mHelper.spidery)
        {

            kneeY = Serializer.RandOvr(-1f, -3f);
            kneeY2 = kneeY + Serializer.RandOvr(1f, 2f);
            kneeY3 = kneeY2 + Serializer.RandOvr(1f, 2f);
        }
        else
        {
            kneeY = Serializer.RandOvr(yUp, yDown);
        }

        aHelper.legReferences = new List<GameObject>();

        float distanceZ = Serializer.RandOvr(0f, 3f);
        hSpine.ForEach(x =>
        {
            if (mHelper.spidery)
            {

                float distanceX = Serializer.RandOvr(0f, -1.5f);
                for (int i = 0; i < 2; i++)
                {


                    if (i == 1) distanceX *= -1;
                    MHelper.symPair firstJoint = mHelper.MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ,mHelper.lstJoints);
                    MHelper.symPair secondJoint = mHelper.MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + Serializer.RandOvr(0f, 1f), mHelper.lstJoints);
                    MHelper.symPair thirdJoint = mHelper.MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX, kneeY2, distanceZ + Serializer.RandOvr(1f, 2f), mHelper.lstJoints);
                    MHelper.symPair fourthJoint = mHelper.MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, distanceX, kneeY3, distanceZ + Serializer.RandOvr(1f, 2f), mHelper.lstJoints);
                    mHelper.MirrorCreate(x.transform, fourthJoint.transform1, fourthJoint.transform2, sHelper.currentDSign * (distanceX + Serializer.RandOvr(0f, 1f)), x.transform.position.y, distanceZ, mHelper.lstSpikes);
                    aHelper.legReferences.Add(mHelper.lstSpikes[mHelper.lstSpikes.Count - 1]);
                    aHelper.legReferences.Add(mHelper.lstSpikes[mHelper.lstSpikes.Count - 2]);
                }
            }
            else for (int i = 0; i < Serializer.RandOvr(1, 3); i++)
                {

                    float distanceX = Serializer.RandOvr(0f, -1.5f);
                    MHelper.symPair firstJoint = mHelper.MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ, mHelper.lstJoints);
                    MHelper.symPair secondJoint = mHelper.MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + Serializer.RandOvr(0f, 1f), mHelper.lstJoints);
                    MHelper.symPair thirdJoint = mHelper.MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, sHelper.currentDSign * (distanceX + Serializer.RandOvr(0f, 1f)), x.transform.position.y, distanceZ, mHelper.lstJoints);
                    mHelper.MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, sHelper.currentDSign * (distanceX + 1) - 1, x.transform.position.y, distanceZ, mHelper.lstJoints);
                    aHelper.legReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 1]);
                    aHelper.legReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 2]);

                }
        });
    }

    public void ArmsStage()
    {
        vSpine.ForEach(x =>
        {
            for (int i = 0; i < Serializer.RandOvr(1, 3); i++)
            {
                float armKnee = Serializer.RandOvr(1f, 2f);
                float distanceZ = Serializer.RandOvr(0.1f, 0.5f);
                float distanceX = Serializer.RandOvr(0.5f, -2f);
                MHelper.symPair firstJoint = mHelper.MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ,mHelper.lstJoints);
                MHelper.symPair secondJoint = mHelper.MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, Serializer.RandOvr(0.5f, 1f), distanceZ + armKnee, mHelper.lstJoints);
                mHelper.MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX + Serializer.RandOvr(-0.5f, -1f), Serializer.RandOvr(0.5f, 1f), Serializer.RandOvr(0, distanceZ + armKnee), mHelper.lstJoints);
                aHelper.armReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 1]);
                aHelper.armReferences.Add(mHelper.lstJoints[mHelper.lstJoints.Count - 2]);

            }
        });
    }

    public void ArchwingStage()
    {
        float x = 0;
        float y = -0.5f;
        float z = 2;
        MHelper.symPair firstJoint = mHelper.MirrorCreate(tempVJoint.transform, tempVJoint.transform, tempVJoint.transform, x, y, z, mHelper.lstJoints);
        float xDFirst = x + 0;
        float yDFirst = y + 1;
        float zDFirst = z + 0.5f;
        MHelper.symPair firstDigit = mHelper.MirrorCreate(tempVJoint.transform, firstJoint.transform1, firstJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        float xDSecond = x + 0;
        float yDSecond = y + 2;
        float zDSecond = z + 0.8f;
        mHelper.MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);


        x += 0;
        y += -1;
        z += 1;
        MHelper.symPair secondJoint = mHelper.MirrorCreate(tempVJoint.transform, firstJoint.transform1, firstJoint.transform2, x, y, z, mHelper.lstJoints);
        xDFirst += 0;
        yDFirst += -1;
        zDFirst += 2;
        firstDigit = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        xDSecond += 0;
        yDSecond += -1;
        zDSecond += 2.4f;
        MHelper.symPair SecondDigit = mHelper.MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);
        mHelper.MirrorCreate(tempVJoint.transform, SecondDigit.transform1, SecondDigit.transform2, x, y + 3, z + 2.5f, mHelper.lstJoints);

        x += 0;
        y += -1;
        z += 1;
        MHelper.symPair thirdJoint = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, x, y, z, mHelper.lstJoints);
        //f
        xDFirst += 0;
        yDFirst += -2;
        zDFirst += 1;
        firstDigit = mHelper.MirrorCreate(tempVJoint.transform, thirdJoint.transform1, thirdJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        xDSecond += 0;
        yDSecond += -2.5f;
        zDSecond += 1.8f;
        SecondDigit = mHelper.MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);
        mHelper.MirrorCreate(tempVJoint.transform, SecondDigit.transform1, SecondDigit.transform2, x, y + 1, z + 4, mHelper.lstJoints);

        x += 0;
        y += -1;
        z += 1;
        MHelper.symPair fourthJoint = mHelper.MirrorCreate(tempVJoint.transform, thirdJoint.transform1, thirdJoint.transform2, x, y, z, mHelper.lstJoints);
        //f
        xDFirst += 0;
        yDFirst += -1;
        zDFirst += -0.5f;
        firstDigit = mHelper.MirrorCreate(tempVJoint.transform, fourthJoint.transform1, fourthJoint.transform2, xDFirst, yDFirst, zDFirst, mHelper.lstJoints);
        xDSecond += 0;
        yDSecond += -1.6f;
        zDSecond += -1;
        mHelper.MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond, mHelper.lstJoints);
    }

    public void BatwingsStage()
    {
        float x = 1;
        float y = -1.5f;
        float z = 2;
        MHelper.symPair firstJoint = mHelper.MirrorCreate(tempVJoint.transform, tempVJoint.transform, tempVJoint.transform, x, y, z, mHelper.lstJoints);
        x += 0;
        y += -3;
        z += +2;
        MHelper.symPair secondJoint = mHelper.MirrorCreate(tempVJoint.transform, firstJoint.transform1, firstJoint.transform2, x, y, z, mHelper.lstJoints);

        //first digit
        float xD = x;
        float yD = y - 1;
        float zD = z + 1;
        MHelper.symPair digitFirst = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += -0.5f;
        zD += 0;
        mHelper.MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstSpikes);

        //second digit
        xD = x;
        yD = y + 0.4f;
        zD = z + 2.8f;
        digitFirst = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 0.6f;
        zD += 0.6f;
        mHelper.MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstSpikes);


        //third digit
        xD = x;
        yD = y + 3;
        zD = z + 2.6f;
        digitFirst = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 2.5f;
        zD += 0.2f;
        MHelper.symPair digitSecond = mHelper.MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 2;
        zD += -0.8f;
        mHelper.MirrorCreate(tempVJoint.transform, digitSecond.transform1, digitSecond.transform2, xD, yD, zD, mHelper.lstSpikes);

        //fourth digit
        xD = x;
        yD = y + 4;
        zD = z + 1.5f;
        digitFirst = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD,mHelper.lstJoints);
        xD += 0;
        yD += 1.5f;
        zD += -0.5f;
        digitSecond = mHelper.MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD, mHelper.lstJoints);
        xD += 0;
        yD += 0.5f;
        zD += -1;
        mHelper.MirrorCreate(tempVJoint.transform, digitSecond.transform1, digitSecond.transform2, xD, yD, zD, mHelper.lstSpikes);

        //fifth digit
        digitFirst = mHelper.MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, x, y + 4.2f, z, mHelper.lstJoints);
        mHelper.MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, x, y + 5, z - 0.5f, mHelper.lstSpikes);
    }

    public void GenerativeWingsStage()
    {
        float distance = Serializer.RandOvr(1.5f, 1.75f);
        float logIncline = Serializer.RandOvr(-0f, -0.1f);
        float linearDifference = Serializer.RandOvr(-0.25f, -0.5f);
        float linearIncline = linearDifference;
        float offset = Serializer.RandOvr(-0.5f, -1f);
        int nrJoints = Serializer.RandOvr(5, 8);
        List<MHelper.symPair> joints = new List<MHelper.symPair>();
        MHelper.symPair parent = new MHelper.symPair(tempVJoint.transform, tempVJoint.transform);

        //bota
        for (int i = 0; i < nrJoints; i++)
        {
            parent = mHelper. MirrorCreate(tempVJoint.transform, parent.transform1, parent.transform2, 0, logIncline + linearIncline + offset, distance, mHelper.lstJoints);
            joints.Add(parent);
            logIncline += logIncline;
            linearIncline += linearDifference;
            distance += Serializer.RandOvr(1.5f, 2f);
        }

        //pene
        float yLinearDistance = Serializer.RandOvr(0.5f, 1f);
        float zLinearDistance = Serializer.RandOvr(0.5f, 0f);
        int maximaPoint = Serializer.RandOvr((nrJoints / 2) + 1, nrJoints - 1) - 1;
        float yLogIncline = Serializer.RandOvr(+0.2f, +0.5f);
        float firstCoef = 2;
        float secondCoef = 2;
        float zLogIncline = Serializer.RandOvr(0.01f, 0.012f);
        for (int i = 0; i < nrJoints; i++)
        {
            zLogIncline += zLogIncline;
            if (i == maximaPoint)
            {
                yLogIncline += yLinearDistance;
            }
            float startY = tempVJoint.transform.position.y - joints[i].transform1.position.y;
            float endZ = tempVJoint.transform.position.z - joints[i].transform1.position.z + zLinearDistance + zLogIncline;
            if (i >= maximaPoint)
            {

                float endY = yLogIncline + tempVJoint.transform.position.y - joints[i].transform1.position.y;
                MHelper.symPair middlePhalanx = mHelper.MirrorCreate(tempVJoint.transform, joints[i].transform1, joints[i].transform2, 0, (endY - startY) * 3 / 4 + startY, endZ + 0.5f, mHelper.lstJoints);
                MHelper.symPair lastPhlanx = mHelper.MirrorCreate(tempVJoint.transform, middlePhalanx.transform1, middlePhalanx.transform2, 0, endY, endZ, mHelper.lstSpikes);

                yLogIncline /= secondCoef;
            }
            else
            {
                float endY = yLogIncline + yLinearDistance + tempVJoint.transform.position.y - joints[i].transform1.position.y;
                MHelper.symPair middlePhalanx = mHelper.MirrorCreate(tempVJoint.transform, joints[i].transform1, joints[i].transform2, 0, (endY - startY) * 3 / 4 + startY, endZ + 0.5f, mHelper.lstJoints);
                mHelper.MirrorCreate(tempVJoint.transform, middlePhalanx.transform1, middlePhalanx.transform2, 0, endY, endZ, mHelper.lstSpikes);
                yLogIncline *= firstCoef;
            }
        }
    }

    public void HeadStage()
    {
        Vector3 headP = tempVJoint.transform.position;
        headP.y += Serializer.RandOvr(0f, 1.5f);
        headP.x += Serializer.RandOvr(0f, 1.5f);
        GameObject head = (GameObject)Instantiate(mHelper.joint, headP, Quaternion.identity);
        head.transform.parent = tempVJoint.transform;
        head.name = "j0";
        aHelper.neck = head;
        mHelper.lstJoints.Add(head);
        GameObject crocSkull = (GameObject)Instantiate(mHelper.skulls[Serializer.RandOvr(0, mHelper.skulls.Count)], headP, Quaternion.Euler(new Vector3(0, 180, 0)));
        crocSkull.transform.parent = head.transform;
        mHelper.lstJoints.Add(crocSkull);
        //  newBone.transform.rotation = Quaternion.FromToRotation(Vector3.up, tempVJoint.transform.position  - head.transform.position);

        //MeshRenderer meshRenderer = newBone.GetComponent<MeshRenderer>();
        //meshRenderer.bounds.SetMinMax(head.transform.position, head.transform.position);
    }


    
    public void TailStage()
    {

        float xDistance = Serializer.RandOvr(-1f, -1.5f);
        int nrSegments = Serializer.RandOvr(4, 11);
        Transform parent = tailStumps[0].transform;
        Vector3 position = parent.transform.position;
        position.x += xDistance;
        for (int i = 0; i < nrSegments; i++)
        {
            GameObject tailSegment = (GameObject)Instantiate(mHelper.joint, position, Quaternion.identity);
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

    //Methods (E)
}