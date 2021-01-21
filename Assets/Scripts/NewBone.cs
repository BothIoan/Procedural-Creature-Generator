using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBone : MonoBehaviour
{
  public void CreateBone()
    {
        Text newText = transform.Find("Text").GetComponent<Text>();
        newText.text = "Pulan beci sa mi-o freci";
    }
}
