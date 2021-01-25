using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NewObject : MonoBehaviour
{

    // the Equip prefab - required for instantiation
    public GameObject equipPrefab;
    public GameObject parent;
    public List<GameObject> createdObjects = new List<GameObject>();
    private float minX, maxX, minY, maxY;
    public GameObject currentSymPlane;
    int currentDSign;
    public GameObject tempHJoint;

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
        maxX = currentSymPlane.GetComponent<MeshRenderer>().bounds.size.x;
        float currentDirection = maxX / 2 + currentSymPlane.transform.position.x;
        if (currentDirection > currentSymPlane.transform.position.x)
        {
            currentDSign = 1;
        }
    }

    public void VSpineStage()
    {
        Vector3 headPos = tempHJoint.transform.position;
        headPos.y += Random.Range(3f, 5f);
        GameObject head = (GameObject)Instantiate(equipPrefab, headPos, Quaternion.identity);
        head.transform.parent = tempHJoint.transform;
        createdObjects.Add(head);
        MirrorCreate(currentSymPlane.transform, head.transform, head.transform, 0, Random.Range(0.5f, 1f), Random.Range(2f, 3f));
    }

    public List<GameObject> HSpineStage()
    {
        List<GameObject> hSpine = new List<GameObject>();
        int nrOfJoints = Random.Range(1, 5);
        Vector3 jointP = currentSymPlane.transform.position;
        float distance = 0;

        for (int i = 0; i < nrOfJoints;i++)
        {
            jointP.x = currentSymPlane.transform.position.x + distance;
            GameObject joint = (GameObject)Instantiate(equipPrefab, jointP, Quaternion.identity);
            joint.transform.parent = currentSymPlane.transform;
            createdObjects.Add(joint);
            hSpine.Add(joint);
            distance += Random.Range(-2f, -3f);
        }
        tempHJoint = hSpine[0];
        return hSpine;
    }

    public void LegsStage()
    {
        float distanceZ = Random.Range(0f, 3f);
        List<GameObject> hSpine = HSpineStage();
        hSpine.ForEach(x =>
        {
            float distanceX = Random.Range(-1f, -1.5f);
            symPair firstJoint = MirrorCreate(x.transform, x.transform, x.transform, distanceX , 3, distanceZ);
            MirrorCreate(x.transform, firstJoint.transform1, firstJoint.transform2, currentDSign * (distanceX + 1), 4, distanceZ);
        });
    }

    public void ArmsStage()
    {

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
        LegsStage();
        VSpineStage();
    }
}