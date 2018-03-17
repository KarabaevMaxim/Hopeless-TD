using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTowerAnimController : MonoBehaviour
{
    private Animator anim;
    [HideInInspector] public GameObject IncomeTextObject;
    [HideInInspector] public GameObject IncomeTextPrefab;
    [HideInInspector] public int IncomeValue;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void ToUp()
    {
        anim.SetBool("up", true);
        Debug.Log(anim.GetBool("up"));
        
    }

    void GetIncome()
    {
        ClickableObject.gameMode.Diamonds += IncomeValue;
        anim.SetBool("up", false);
        Debug.Log(anim.GetBool("up"));
        IncomeTextObject = Instantiate(IncomeTextPrefab, GameMode.gameHUD.DynamicObjectsParent);
    }
}
