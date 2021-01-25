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

    public void LegsStage()
    {
        int nrOfLegs = Random.Range(1,5);
        Vector3 legAttPos = currentSymPlane.transform.position;
        float distanceZ = Random.Range(0f, 3f);
        
        float ammount = 0;
        for (int i = 0; i < nrOfLegs; i++)
        {
            legAttPos.x = currentSymPlane.transform.position.x + ammount;
            GameObject legAtt = (GameObject)Instantiate(equipPrefab, legAttPos, Quaternion.identity);
            legAtt.transform.parent = currentSymPlane.transform;
            createdObjects.Add(legAtt);
            
            float distanceX = -1*(ammount + Random.Range(1f,1.5f));

            symPair joints = MirrorCreate(currentSymPlane.transform,legAtt.transform,legAtt.transform, distanceX, 3, distanceZ);
            MirrorCreate(currentSymPlane.transform,joints.transform1,joints.transform2, (distanceX + 1) * currentDSign, 4, distanceZ);
            if (i == 0)
            {
                SpineStage(legAtt.transform,currentSymPlane.transform);
            }
            ammount += Random.Range(-2f, -3f);
        }
    }

    public void SpineStage(Transform parent, Transform symPlane)
    {
        Vector3 headPos = parent.position;
        headPos.y += Random.Range(3f, 5f);
        GameObject head = (GameObject)Instantiate(equipPrefab, headPos, Quaternion.identity);
        head.transform.parent = parent;
        createdObjects.Add(head);
        MirrorCreate(symPlane, head.transform, head.transform, 0, Random.Range(0.5f, 1f), Random.Range(2f, 3f));
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
    }
}