using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NewObject : MonoBehaviour
{

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

        for (int i = 0; i < nrOfJoints; i++)
        {
            jointP.y = currentSymPlane.transform.position.y + distance;
            GameObject joint = (GameObject)Instantiate(equipPrefab, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = tempHJoint.transform;
            else joint.transform.parent = vSpine[i - 1].transform;
            createdObjects.Add(joint);
            vSpine.Add(joint);
            distance += Random.Range(2f, 3f);
        }
        tempVJoint = vSpine[nrOfJoints-1];
    }

    public void HSpineStage()
    {

        int nrOfJoints = Random.Range(1, 5);
        Vector3 jointP = currentSymPlane.transform.position;
        jointP.y += Random.Range(1f,-1f);
        float distance = 0;
        hSpine = new List<GameObject>();
        for (int i = 0; i < nrOfJoints;i++)
        {
            jointP.x = currentSymPlane.transform.position.x + distance;
            GameObject joint = (GameObject)Instantiate(equipPrefab, jointP, Quaternion.identity);
            if (i == 0) joint.transform.parent = currentSymPlane.transform;
            else joint.transform.parent = hSpine[i - 1].transform;
            createdObjects.Add(joint);
            hSpine.Add(joint);
            distance += Random.Range(-2f, -3f);
        }
        tempHJoint = hSpine[0];
        //hack to be able to remove the arms stage
        tempVJoint = hSpine[0];
    }

    public void LegsStage()
    {

        //legs must happen between the current point and the ground.
        float yUp = hSpine[0].transform.position.y;
        float yDown = 0f;
        //choose position of knee:
        float kneeY = Random.Range(yUp, yDown);
        float distanceZ = Random.Range(0f, 3f);
        hSpine.ForEach(x =>
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
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
        Cleanup();
        PlaneStage();
        getDirection();
        HSpineStage();
        LegsStage();
        VSpineStage();
        if (Random.Range(0, 2) == 1)
        ArmsStage();
        HeadStage();
    }
}