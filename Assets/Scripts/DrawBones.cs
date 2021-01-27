using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Hacku = Popcron.Gizmos;

public class DrawBones : MonoBehaviour
{
    [SerializeField] public GameObject bone;
    [SerializeField] public Transform rootNode;
    public Transform[] childNodes;
    [SerializeField] Color customColor;
    Color color;
    void Start()
    {
        if (rootNode != null) childNodes = rootNode.GetComponentsInChildren<Transform>();
        color = Color.red;
        color.r = customColor.r;
        color.g = customColor.g;
        color.b = customColor.b;
    }
    void Update()
    {
        Hacku.Enabled = true;
        if (rootNode != null)
        {
            foreach (Transform child in childNodes)
            {
                if (child != rootNode && child.parent != rootNode) 
                   {
                    
                    Hacku.Line(child.position, child.parent.position,color);

                }
            }
        }
    }

}