using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeEffect : MonoBehaviour {

    public float timer = 0.8f;
	// Use this for initialization
	void Start ()
    {
        timer = 0.8f;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(timer <= 0.0f)
        {
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
        }
	}
}
