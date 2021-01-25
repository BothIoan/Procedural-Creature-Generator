using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NewObject : MonoBehaviour
{
    //for fun
    private List<string> nume1 = new List<string>();
    private List<string> nume2 = new List<string>();
    [SerializeField] Text text;
    [SerializeField] bool fun;
    //for fun


    // the Equip prefab - required for instantiation
    public GameObject equipPrefab;
    public GameObject parent;
    public List<GameObject> createdObjects;
    public GameObject currentSymPlane;
    int currentDSign;
    public GameObject tempHJoint;
    public GameObject tempVJoint;
    public List<GameObject> hSpine;
    public List<GameObject> vSpine;


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
        nume2.Add("taganesc");
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

    public void HSpineStage()
    {

        int nrOfJoints = Random.Range(1, 5);
        Vector3 jointP = currentSymPlane.transform.position;
        jointP.y += Random.Range(1f,-1f);
        float distance = 0;
        float incline = Random.Range(-0.5f, 0.5f);
        hSpine = new List<GameObject>();
        
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
      
        float distanceZ = Random.Range(0f, 3f);
        hSpine.ForEach(x =>
        {
            if(spidery)
            {

                float distanceX = Random.Range(0f, -1.5f);
                for(int i = 0; i< 2; i++)
                {
                    float yUp = x.transform.position.y;
                    kneeY = Random.Range(-1f, -3f);
                    kneeY2 = kneeY + Random.Range(1f, 2f);
                    kneeY3 = kneeY2 + Random.Range(1f, 2f);
                    
                    if (i == 1) distanceX *= -1;
                    symPair firstJoint = MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ);
                    symPair secondJoint = MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + Random.Range(0f, 1f));
                    symPair thirdJoint = MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, distanceX, kneeY2, distanceZ + Random.Range(1f, 2f));
                    symPair fourthJoint = MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, distanceX, kneeY3, distanceZ + Random.Range(1f, 2f));
                    MirrorCreate(x.transform, fourthJoint.transform1, fourthJoint.transform2, currentDSign * (distanceX + Random.Range(0f, 1f)), yUp, distanceZ);
                }
            }
            else for (int i = 0; i < Random.Range(1, 3); i++)
            {
                float yUp = x.transform.position.y;
                kneeY = Random.Range(yUp, yDown);
                float distanceX = Random.Range(0f, -1.5f);
                symPair firstJoint = MirrorCreate(x.transform, x.transform, x.transform, 0, 0, distanceZ);
                symPair secondJoint = MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, distanceX, kneeY, distanceZ + Random.Range(0f, 1f));
                symPair thirdJoint = MirrorCreate(x.transform, secondJoint.transform1, secondJoint.transform2, currentDSign * (distanceX + Random.Range(0f, 1f)), yUp, distanceZ);
                MirrorCreate(x.transform, thirdJoint.transform1, thirdJoint.transform2, currentDSign * (distanceX + 1) - 1, yUp, distanceZ);
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

    public void HeadStage()
    {
        Vector3 headP = tempVJoint.transform.position;
        headP.y += Random.Range(0f, 1.5f);
        headP.x += Random.Range(0f, 1.5f);
        GameObject head = (GameObject)Instantiate(equipPrefab, headP, Quaternion.identity);
        head.transform.parent = tempVJoint.transform;
        createdObjects.Add(head);
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

    public void pressButton()
    {
        if (fun)
        forFun();
        Cleanup();
        PlaneStage();
        getDirection();
        HSpineStage();
        LegsStage();

        if (Random.Range(0, 2) == 1)
        {
            VSpineStage();
            ArmsStage();
        }
        HeadStage();
    }
}