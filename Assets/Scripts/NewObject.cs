using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// rename this class to suit your needs
public class NewObject : MonoBehaviour
{
    // the Equip prefab - required for instantiation
    public GameObject equipPrefab;
    public GameObject parent;
    [SerializeField] private float distance;
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

    public void CreateObject()
    {
        // a prefab is need to perform the instantiation
        if (equipPrefab != null)
        {
            // get a random postion to instantiate the prefab - you can change this to be created at a fied point if desired
            Vector3 position = new Vector3(501, 3, 510);

            GameObject parentSphere = (GameObject)Instantiate(parent, position, Quaternion.identity);
            createdObjects.Add(parentSphere);
            position.y -= 2;

            // instantiate the object
            Vector3 pos1 = position;
            pos1.z -= distance;
            GameObject g1 = (GameObject)Instantiate(equipPrefab, pos1, Quaternion.identity);
            g1.transform.parent = parentSphere.transform;
            createdObjects.Add(g1);

            Vector3 pos2 = position;
            pos2.z += distance;
            GameObject g2 = (GameObject)Instantiate(equipPrefab, pos2, Quaternion.identity);
            g2.transform.parent = parentSphere.transform;
            createdObjects.Add(g2);


        }
    }
}