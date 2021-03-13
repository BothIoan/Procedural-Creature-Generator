using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class NewObject : MonoBehaviour
{
    //for fun
    private List<string> nume1 = new List<string>();
    private List<string> nume2 = new List<string>();
    [SerializeField] Text text;
    [SerializeField] bool fun;
    [SerializeField] public GameObject bone;
    [SerializeField] public GameObject spike;
    [SerializeField] List<GameObject> skulls;
    //for fun


    // the Equip prefab - required for instantiation
    public GameObject equipPrefab;
    public GameObject parent;
    public List<GameObject> createdObjects;
    public List<GameObject> drawnBones;
    public GameObject currentSymPlane;
    int currentDSign;
    public GameObject tempHJoint;
    public GameObject tempVJoint;
    public List<GameObject> hSpine;
    public List<GameObject> vSpine;
    public List<GameObject> tailStumps;
    public List<GameObject> spikes;



    private void forFun()
    {
        nume1.Add("omu");
        nume1.Add("taganu'");
        nume1.Add("pericolu'");
        nume1.Add("cainele");
        nume1.Add("animalu'");
        nume1.Add("scandalu'");
        nume1.Add("copilu");
        nume1.Add("nacazu'");
        nume1.Add("faraonu'");
        nume1.Add("bocteru'");
        nume1.Add("basculantu'");
        nume1.Add("junghiu'");
        nume1.Add("milmoiu'");
        nume1.Add("mazgoiu");
        nume1.Add("sarumanu'");
        nume1.Add("opelu'");
        nume1.Add(" talharu'");
        nume1.Add(" belferu'");
        nume2.Add(" rau");
        nume2.Add(" spurcat");
        nume2.Add(" periculos");
        nume2.Add(" salbatic");
        nume2.Add(" scandal");
        nume2.Add(" infiorator");
        nume2.Add(" dusmanos");
        nume2.Add(" durerii");
        nume2.Add(" nacaz");
        nume2.Add(" nacajit");
        nume2.Add(" din animeuri");
        nume2.Add(" had");
        nume2.Add(" hait");
        nume2.Add(" caraiala");
        nume2.Add(" chinezesc");
        nume2.Add(" tradarii");
        nume2.Add(" bastarca");
        nume2.Add(" glorios");
        nume2.Add(" liliac");
        nume2.Add(" accident");
        nume2.Add(" blastamat");
        nume2.Add(" distrugator");
        nume2.Add(" puterii");
        nume2.Add(" mortii");
        nume2.Add(" lorbant");
        nume2.Add(" terminatoru");
        nume2.Add(" nebun");
        nume2.Add(" ciumei");
        nume2.Add(" intunecat mandru");
        nume2.Add(" intunecat");
        nume2.Add(" tumultos");
        nume2.Add(" taganesc");
        nume2.Add(" din filmele matrix");
        nume2.Add(" buclucas");
        nume2.Add(" sarac");
        
        text.text =  (nume1[Random.Range(0, nume1.Count)] + nume2[Random.Range(0,nume2.Count)]);
    }

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

    public void Cleanup()
    {
        //temporary, to see different results;
        createdObjects.ForEach((x) =>
        {
            Destroy(x);
        });
        createdObjects.Clear();
        drawnBones.ForEach((x) =>
        {
            Destroy(x);
        });
        drawnBones.Clear();
        spikes.ForEach(x =>
        {
            Destroy(x);
        });
        spikes.Clear();
        //temp
    }
    
    public GameObject PlaneStage()
    {
        Vector3 initial = new Vector3(501, 5, 510);
        GameObject symPlane = (GameObject)Instantiate(parent, initial, Quaternion.identity);
        createdObjects.Add(symPlane);
        currentSymPlane = symPlane;
        return symPlane;
    }

    public void getDirection()
    {
        currentDSign = -1;
        float maxX = currentSymPlane.GetComponent<MeshRenderer>().bounds.size.x;
        float currentDirection = maxX / 2 + currentSymPlane.transform.position.x;
        if (currentDirection > currentSymPlane.transform.position.x)
        {
            currentDSign = 1;
        }
    }

    public void VSpineStage()
    {
        int nrOfJoints = Random.Range(1, 3);
        //temporary disable 
      // nrOfJoints = 1;
       vSpine = new List<GameObject>();
        Vector3 jointP = currentSymPlane.transform.position;
        float distance = Random.Range(1f,1.5f);

        float incline = Random.Range(-1f, 1f);
        
        for (int i = 0; i < nrOfJoints; i++)
        {
            jointP.x = currentSymPlane.transform.position.x + incline;
            jointP.y = currentSymPlane.transform.position.y + distance;
            GameObject joint = (GameObject)Instantiate(equipPrefab, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = tempHJoint.transform;
            else joint.transform.parent = vSpine[i - 1].transform;
            createdObjects.Add(joint);
            vSpine.Add(joint);
            distance += Random.Range(2f, 3f);
            incline += incline;
        }
        tempVJoint = vSpine[nrOfJoints-1];
    }

    public void DrawBonesStage()
    {
        
        createdObjects.ForEach(x => {
            if (x.transform.parent != null && x.transform.parent !=currentSymPlane.transform)
            {
                Transform tempParent = x.transform.parent;
                x.transform.parent = null;
                GameObject newBone = (GameObject)Instantiate(bone, x.transform.position, Quaternion.FromToRotation(Vector3.up, tempParent.transform.position - x.transform.position));
                newBone.transform.parent = null;
                newBone.transform.localScale = new Vector3(0, newBone.transform.localScale.y, 0);
                Vector3 minC = newBone.GetComponent<Renderer>().bounds.min;
                Vector3 maxC = newBone.GetComponent<Renderer>().bounds.max;
                float distC = Vector3.Distance(minC,maxC);
                float distN = Vector3.Distance(tempParent.transform.position, x.transform.position);
                float scaleN = distN / distC;
                newBone.transform.localScale = new Vector3(0.5f, scaleN* 1.1f, 0.5f);
                newBone.transform.parent = tempParent.transform;
                x.transform.parent = tempParent;
                drawnBones.Add(newBone);
            }
        });
        spikes.ForEach(x => {
            if (x.transform.parent != null && x.transform.parent != currentSymPlane.transform)
            {
                Transform tempParent = x.transform.parent;
                x.transform.parent = null;
                GameObject newBone = (GameObject)Instantiate(spike, x.transform.position , Quaternion.FromToRotation(Vector3.up, tempParent.transform.position - x.transform.position));
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
                drawnBones.Add(newBone);
            }
        });
    }

    public void HSpineStage()
    {

        int nrOfJoints = Random.Range(1, 5);
        Vector3 jointP = currentSymPlane.transform.position;
        jointP.y += Random.Range(1f,-1f);
        float distance = 0;
        float incline = Random.Range(-0.5f, 0.5f);
        hSpine = new List<GameObject>();
        tailStumps = new List<GameObject>();
        for (int i = 0; i < nrOfJoints;i++)
        {
            jointP.y = currentSymPlane.transform.position.y + incline;
            jointP.x = currentSymPlane.transform.position.x + distance;
            GameObject joint = (GameObject)Instantiate(equipPrefab, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = currentSymPlane.transform;
            else joint.transform.parent = hSpine[i - 1].transform;
            createdObjects.Add(joint);
            hSpine.Add(joint);
            distance += Random.Range(-2f, -3f);
            incline += incline;
        }
        
        tailStumps.Add(hSpine[nrOfJoints-1]);
        tempHJoint = hSpine[0];
        //hack to be able to remove the arms stage
        tempVJoint = hSpine[0];
    }

    

    public void LegsStage()
    {
        bool spidery = false;
        if (Random.Range(0, 2) == 0)
        {
            spidery = true;
        }
        //legs must happen between the current point and the ground.
        float yDown = 0.1f;
        //choose position of knee:
        float kneeY;
        float kneeY2 = 0;
        float kneeY3 = 0;
        float yUp = currentSymPlane.transform.position.y;
        if (spidery)
        {
            
            kneeY = Random.Range(-1f, -3f);
            kneeY2 = kneeY + Random.Range(1f, 2f);
            kneeY3 = kneeY2 + Random.Range(1f, 2f);
        }
        else
        {
            kneeY = Random.Range(yUp, yDown);
        }

       
      
        float distanceZ = Random.Range(0f, 3f);
        hSpine.ForEach(x =>
        {
            if(spidery)
            {

                float distanceX = Random.Range(0f, -1.5f);
                for(int i = 0; i< 2; i++)
                {
                  
                    
                    if (i == 1) distanceX *= -1;
                    symPair firstJoint = MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ);
                    symPair secondJoint = MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + Random.Range(0f, 1f));
                    symPair thirdJoint = MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX, kneeY2, distanceZ + Random.Range(1f, 2f));
                    symPair fourthJoint = MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, distanceX, kneeY3, distanceZ + Random.Range(1f, 2f));
                    MirrorCreateSpikes(x.transform, fourthJoint.transform1, fourthJoint.transform2, currentDSign * (distanceX + Random.Range(0f, 1f)), x.transform.position.y, distanceZ);
                }
            }
            else for (int i = 0; i < Random.Range(1, 3); i++)
            {
                
                float distanceX = Random.Range(0f, -1.5f);
                symPair firstJoint = MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ);
                symPair secondJoint = MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + Random.Range(0f, 1f));
                symPair thirdJoint = MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, currentDSign * (distanceX + Random.Range(0f, 1f)), x.transform.position.y, distanceZ);
                MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, currentDSign * (distanceX + 1) - 1, x.transform.position.y, distanceZ);
                
            }
        });
    }

    public void ArmsStage() 
    {
        vSpine.ForEach(x =>
        {
            for (int i = 0; i < Random.Range(1,3); i++)
            {
                float armKnee = Random.Range(1f, 2f);
                float distanceZ = Random.Range(0.1f, 0.5f);
                float distanceX = Random.Range(0.5f, -2f);
                symPair firstJoint = MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ);
                symPair secondJoint = MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, Random.Range(0.5f, 1f), distanceZ + armKnee);
                MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX + Random.Range(-0.5f, -1f), Random.Range(0.5f, 1f), Random.Range(0, distanceZ + armKnee));
            }
        });
    }

    public void ArchwingStage()
    {
        float x = 0;
        float y = -0.5f;
        float z = 2;
        symPair firstJoint = MirrorCreate(tempVJoint.transform, tempVJoint.transform, tempVJoint.transform, x, y, z);
        float xDFirst = x + 0;
        float yDFirst = y + 1;
        float zDFirst = z + 0.5f;
        symPair firstDigit = MirrorCreate(tempVJoint.transform, firstJoint.transform1, firstJoint.transform2, xDFirst, yDFirst, zDFirst );
        float xDSecond = x + 0;
        float yDSecond = y + 2;
        float zDSecond = z + 0.8f;
        MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond);
 

        x += 0;
        y += -1;
        z += 1;
        symPair secondJoint = MirrorCreate(tempVJoint.transform, firstJoint.transform1, firstJoint.transform2, x , y, z);
        xDFirst += 0;
        yDFirst += -1;
        zDFirst += 2;
        firstDigit = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xDFirst, yDFirst, zDFirst);
        xDSecond += 0;
        yDSecond += -1;
        zDSecond += 2.4f;
        symPair SecondDigit = MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond);
        MirrorCreate(tempVJoint.transform, SecondDigit.transform1, SecondDigit.transform2, x, y + 3, z + 2.5f);

        x += 0;
        y += -1;
        z += 1;
        symPair thirdJoint = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, x , y, z);
        //f
        xDFirst += 0;
        yDFirst += -2;
        zDFirst += 1;
        firstDigit = MirrorCreate(tempVJoint.transform, thirdJoint.transform1, thirdJoint.transform2, xDFirst, yDFirst, zDFirst);
        xDSecond += 0;
        yDSecond += -2.5f;
        zDSecond += 1.8f;
        SecondDigit = MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond);
        MirrorCreate(tempVJoint.transform, SecondDigit.transform1, SecondDigit.transform2, x , y + 1, z + 4);

        x += 0;
        y += -1;
        z += 1;
        symPair fourthJoint = MirrorCreate(tempVJoint.transform, thirdJoint.transform1, thirdJoint.transform2, x , y, z);
        //f
        xDFirst += 0;
        yDFirst += -1;
        zDFirst += -0.5f;
        firstDigit = MirrorCreate(tempVJoint.transform, fourthJoint.transform1, fourthJoint.transform2, xDFirst, yDFirst, zDFirst);
        xDSecond += 0;
        yDSecond += -1.6f;
        zDSecond += -1;
        MirrorCreate(tempVJoint.transform, firstDigit.transform1, firstDigit.transform2, xDSecond, yDSecond, zDSecond);
    }

    public void BatwingsStage()
    {
        float x = 1;
        float y = -1.5f;
        float z = 2;
        symPair firstJoint = MirrorCreate(tempVJoint.transform, tempVJoint.transform, tempVJoint.transform, x , y , z);
        x += 0;
        y += -3;
        z += +2;
        symPair secondJoint = MirrorCreate(tempVJoint.transform, firstJoint.transform1, firstJoint.transform2, x , y, z);

        //first digit
        float xD = x;
        float yD = y - 1;
        float zD = z + 1;
        symPair digitFirst = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD);
        xD += 0;
        yD += - 0.5f;
        zD += 0;
        MirrorCreateSpikes(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD);

        //second digit
        xD = x;
        yD = y + 0.4f;
        zD = z + 2.8f;
        digitFirst = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD);
        xD += 0;
        yD += 0.6f;
        zD += 0.6f;
        MirrorCreateSpikes(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD);


        //third digit
        xD = x;
        yD = y + 3;
        zD = z + 2.6f;
        digitFirst = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD , zD);
        xD += 0;
        yD += 2.5f;
        zD += 0.2f;
        symPair digitSecond = MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD);
        xD += 0;
        yD += 2;
        zD += - 0.8f;
        MirrorCreateSpikes(tempVJoint.transform, digitSecond.transform1, digitSecond.transform2, xD, yD, zD);

        //fourth digit
        xD = x;
        yD = y + 4;
        zD = z + 1.5f;
        digitFirst = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, xD, yD, zD);
        xD += 0;
        yD += 1.5f;
        zD += -0.5f;
        digitSecond = MirrorCreate(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, xD, yD, zD );
        xD += 0;
        yD += 0.5f;
        zD += -1;
        MirrorCreateSpikes(tempVJoint.transform, digitSecond.transform1, digitSecond.transform2, xD, yD  , zD);

        //fifth digit
        digitFirst = MirrorCreate(tempVJoint.transform, secondJoint.transform1, secondJoint.transform2, x, y + 4.2f, z);
        MirrorCreateSpikes(tempVJoint.transform, digitFirst.transform1, digitFirst.transform2, x, y + 5, z -0.5f);
    }

    public void GenerativeWingsStage()
    {
        float distance = Random.Range(1.5f, 1.75f);
        float logIncline = Random.Range(-0f, -0.1f);
        float linearDifference = Random.Range(-0.25f, -0.5f);
        float linearIncline = linearDifference;
        float offset = Random.Range(-0.5f, -1f);
        int nrJoints = Random.Range(5,8);
        List<symPair> joints = new List<symPair>();
        symPair parent = new symPair(tempVJoint.transform,tempVJoint.transform);

        //bota
        for(int i = 0; i < nrJoints; i++)
        {
            parent = MirrorCreate(tempVJoint.transform, parent.transform1, parent.transform2, 0, logIncline + linearIncline + offset,distance);
            joints.Add(parent);
            logIncline += logIncline;
            linearIncline += linearDifference;
            distance += Random.Range(1.5f, 2f);
        }

        //pene
        float yLinearDistance = Random.Range(0.5f, 1f);
        float zLinearDistance = Random.Range(0.5f, 0f);
        int maximaPoint = Random.Range((nrJoints / 2) + 1, nrJoints-1)  -1;
        float yLogIncline = Random.Range(+0.2f,+0.5f);
        float firstCoef = 2;
        float secondCoef = 2;
        float zLogIncline = Random.Range(0.01f, 0.012f);
        for (int i = 0; i < nrJoints; i++) {
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
                symPair middlePhalanx = MirrorCreate(tempVJoint.transform, joints[i].transform1, joints[i].transform2, 0, (endY - startY) * 3 / 4 + startY, endZ + 0.5f);
                symPair lastPhlanx =MirrorCreateSpikes(tempVJoint.transform, middlePhalanx.transform1, middlePhalanx.transform2, 0,endY,endZ);
                
                yLogIncline /= secondCoef;
            }
            else
            {
                float endY = yLogIncline + yLinearDistance + tempVJoint.transform.position.y - joints[i].transform1.position.y;
                symPair middlePhalanx = MirrorCreate(tempVJoint.transform, joints[i].transform1, joints[i].transform2, 0, (endY - startY) * 3 / 4 + startY, endZ+ 0.5f);
                MirrorCreateSpikes(tempVJoint.transform, middlePhalanx.transform1, middlePhalanx.transform2, 0,endY, endZ);
                yLogIncline *= firstCoef;
            }
        }
    }

    public void HeadStage()
    {
        Vector3 headP = tempVJoint.transform.position;
        headP.y += Random.Range(0f, 1.5f);
        headP.x += Random.Range(0f, 1.5f);
        GameObject head = (GameObject)Instantiate(equipPrefab, headP, Quaternion.identity);
        head.transform.parent = tempVJoint.transform;
        createdObjects.Add(head);
        GameObject crocSkull = (GameObject)Instantiate(skulls[Random.Range(0,skulls.Count)], headP, Quaternion.Euler(new Vector3(0,180,0)));
        crocSkull.transform.parent = head.transform;
        createdObjects.Add(crocSkull);
        //  newBone.transform.rotation = Quaternion.FromToRotation(Vector3.up, tempVJoint.transform.position  - head.transform.position);

        //MeshRenderer meshRenderer = newBone.GetComponent<MeshRenderer>();
        //meshRenderer.bounds.SetMinMax(head.transform.position, head.transform.position);
    }

    public symPair MirrorCreate(Transform symPlanT, Transform parent1T, Transform parent2T, float distanceX, float distanceY, float distanceZ)
    {
        Vector3 initial = symPlanT.position;

        initial.y -= distanceY;
        initial.x -= distanceX;
        Vector3 pos1 = initial;
        pos1.z -= distanceZ;
        GameObject g1 = (GameObject)Instantiate(equipPrefab, pos1, Quaternion.identity);
        g1.transform.parent = parent1T;
        createdObjects.Add(g1);

        Vector3 pos2 = initial;
        pos2.z += distanceZ;
        GameObject g2 = (GameObject)Instantiate(equipPrefab, pos2, Quaternion.identity);
        g2.transform.parent = parent2T;
        createdObjects.Add(g2);
        return new symPair(g1.transform, g2.transform);
    }
    public symPair MirrorCreateSpikes(Transform symPlanT, Transform parent1T, Transform parent2T, float distanceX, float distanceY, float distanceZ)
    {
        Vector3 initial = symPlanT.position;

        initial.y -= distanceY;
        initial.x -= distanceX;
        Vector3 pos1 = initial;
        pos1.z -= distanceZ;
        GameObject g1 = (GameObject)Instantiate(equipPrefab, pos1, Quaternion.identity);
        g1.transform.parent = parent1T;
        spikes.Add(g1);

        Vector3 pos2 = initial;
        pos2.z += distanceZ;
        GameObject g2 = (GameObject)Instantiate(equipPrefab, pos2, Quaternion.identity);
        g2.transform.parent = parent2T;
        spikes.Add(g2);
        return new symPair(g1.transform, g2.transform);
    }
    public void TailStage()
    {
        
        float xDistance = Random.Range(-1f, -1.5f);
        int nrSegments = Random.Range(4, 11);
        Transform parent = tailStumps[0].transform;
        Vector3 position = parent.transform.position;
        position.x += xDistance;
        for(int i = 0; i < nrSegments; i++)
        {
            GameObject tailSegment = (GameObject)Instantiate(equipPrefab, position, Quaternion.identity);
            tailSegment.transform.parent = parent;
            createdObjects.Add(tailSegment);
            position.x += xDistance;
            parent = tailSegment.transform;
            
        }
        GameObject spike = createdObjects[createdObjects.Count-1];
        spikes = new List<GameObject>();
        spikes.Add(spike);
        createdObjects.RemoveAt(createdObjects.Count - 1);
    }

    public void SaveCharacterStage()
    {
            PrefabUtility.SaveAsPrefabAsset(currentSymPlane, "Assets/Dihanii/dihanie.prefab");
    }

    public void pressButton()
    {
        if (fun)
        forFun();
        Cleanup();
        PlaneStage();
        getDirection();
        HSpineStage();
        if (Random.Range(0, 2) == 1)
        {
            TailStage();
        }
        LegsStage();

        if (Random.Range(0, 2) == 1)
        {

            VSpineStage();
            ArmsStage();
            int random = Random.Range(0, 3);
            switch (random)
            {
                case 0:
                    GenerativeWingsStage();
                    //ArchwingStage();
                    break;
                case 1:
                    BatwingsStage();
                    break;
            }
            
        }
       
        HeadStage();
        DrawBonesStage();
        SaveCharacterStage();
    }
}