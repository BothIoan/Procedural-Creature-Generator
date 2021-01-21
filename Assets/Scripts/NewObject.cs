using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// rename this class to suit your needs
public class NewObject : MonoBehaviour
{
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

    // the Equip prefab - required for instantiation
    public GameObject equipPrefab;
    public GameObject parent;
    // list that holds all created objects - deleate all instances if desired
    public List<GameObject> createdObjects = new List<GameObject>();
    private float minX, maxX, minY, maxY;

    void Start()
    {
        // get the screen bounds
        float camDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector2 bottomCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector2 topCorner = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        minX = bottomCorner.x;
        maxX = topCorner.x;
        minY = bottomCorner.y;
        maxY = topCorner.y;
    }
    public void LegsStage()
    {
        //temporary, to see different results;
        createdObjects.ForEach((x) =>
        {
            Destroy(x);
        });
        createdObjects.Clear();
        //temp

        //plane stage
        GameObject symPlane = symPlaneCreate();
        maxX = symPlane.GetComponent<MeshRenderer>().bounds.size.x;

        //direction is used for the legs.
        float direction = maxX / 2 + symPlane.transform.position.x;
        Vector3 test = symPlane.transform.position;
        test.x = direction;
        //
        int nrOfLegs = Random.Range(1,5);
        Vector3 legAttPos = symPlane.transform.position;
        float distanceZ = Random.Range(0f, 3f);
        float directionSign;
        if(direction> symPlane.transform.position.x)
        {
            directionSign = 1;
        }
        else
        {
            directionSign = -1;
        }
        float ammount = 0;
        for (int i = 0; i < nrOfLegs; i++)
        {
            
            ammount += Random.Range(-2f, -3f);
            legAttPos.x = symPlane.transform.position.x + ammount;
            GameObject legAtt = (GameObject)Instantiate(equipPrefab, legAttPos, Quaternion.identity);
            legAtt.transform.parent = symPlane.transform;
            createdObjects.Add(legAtt);
            
            float distanceX = -1*(ammount + Random.Range(1f,1.5f));

            symPair joints = MirrorCreate(symPlane.transform,legAtt.transform,legAtt.transform, distanceX, 3, distanceZ);
            MirrorCreate(symPlane.transform,joints.transform1,joints.transform2, (distanceX + 1) * directionSign, 4, distanceZ);
            if (i == 0)
            {
                SpineStage(legAtt.transform,symPlane.transform);
            }
        }
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

    public GameObject symPlaneCreate()
    {
        if (equipPrefab != null)
        {
            Vector3 initial = new Vector3(501, 5, 510);
            GameObject symPlane = (GameObject)Instantiate(parent, initial, Quaternion.identity);
            createdObjects.Add(symPlane);
            return symPlane;
        }
        return null;
    }

    public void SpineStage(Transform parent,Transform symPlane)
    {
        Vector3 headPos = parent.position;
        headPos.y += Random.Range(3f,5f);
        GameObject head = (GameObject)Instantiate(equipPrefab, headPos, Quaternion.identity);
        head.transform.parent = parent;
        createdObjects.Add(head);
        MirrorCreate(symPlane, head.transform, head.transform, 0, Random.Range(0.5f, 1f), Random.Range(2f, 3f));
    }

    public void ArmsStage()
    {

    }
}