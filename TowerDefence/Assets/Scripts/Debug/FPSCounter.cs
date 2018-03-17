using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour
{
    Text Text;
	void Start ()
    {
        Text = gameObject.GetComponent<Text>();
    }
	
	void Update ()
    {
        Text.text = (Mathf.Round(1.0f / Time.deltaTime)).ToString();
    }
}
