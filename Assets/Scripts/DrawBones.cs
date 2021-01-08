using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hacku = Popcron.Gizmos;

public class DrawBones : MonoBehaviour
{

    [SerializeField] public Transform rootNode;
    public Transform[] childNodes;
    [SerializeField] LineRenderer lineRenderer;
    void Update()
    {
        Hacku.Enabled = true;
        lineRenderer.sortingOrder = 1;
       // lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
      //  lineRenderer.material.color = Color.red;
        lineRenderer.positionCount = 0;
        
        if (rootNode != null)
        {
            if (childNodes == null || childNodes.Length == 0)
            {
           
                //get all joints to draw
                PopulateChildren();

            }


            foreach (Transform child in childNodes)
            { 

                if (child == rootNode)
                {
                   // list includes the root, if root then larger, green cube
                   // Gizmos.color = Color.green;
                   // Gizmos.DrawCube(child.position, new Vector3(.1f, .1f, .1f));
                }
                else
                {
                    //  Gizmos.color = Color.blue;
                   //   Gizmos.DrawLine(child.position, child.parent.position);
                     // Gizmos.DrawCube(child.position, new Vector3(.01f, .01f, .01f));
                    Hacku.Line(child.position, child.parent.position);
                    //lineRenderer.positionCount +=2;
                    //lineRenderer.SetPositions(new Vector3[] { child.position, child.parent.position });
                   // Debug.Log("Line1:"+  child.position + " - " + child.parent.position);
                }
            }

        }
    }

    public void PopulateChildren()
    {
        childNodes = rootNode.GetComponentsInChildren<Transform>();
    }
}