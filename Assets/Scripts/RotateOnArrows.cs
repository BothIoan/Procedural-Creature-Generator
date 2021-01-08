using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnArrows : MonoBehaviour
{
    [SerializeField] float speed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * speed*Time.deltaTime,0);
    }
}
