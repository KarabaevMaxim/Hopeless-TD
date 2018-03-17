using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughResoursesPanel : MonoBehaviour {

    GameHUD gameHUD;
    Animator animator;

    void Start ()
    {
        gameHUD = GameObject.FindGameObjectWithTag("UI").GetComponent<GameHUD>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void close()
    {
        animator.SetBool("show", false);
        gameHUD.isNotEnoughResoursesPanelShowed = false;
    }
}
