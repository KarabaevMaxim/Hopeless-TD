using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWarningScript : MonoBehaviour {

    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void close()
    {
        animator.SetBool("show", false);
        GameMode.gameHUD.isWarningShowed = false;
    }
}
