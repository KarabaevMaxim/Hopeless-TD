using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ClickableObject : MonoBehaviour
{
    public string Name;
    public Transform CircleSelectPos;
    public GameObject CircleSelectPrefab;
    static public GameObject CircleSelectObject;
    public Sprite Icon;
    public string Tag;
    static public GameMode gameMode;
    static public GameObject AttackRadiusObject;
    
    virtual protected void Start()
    {
        name = Name;
        tag = Tag;
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    virtual protected void OnClick()
    {
        if (CircleSelectObject != null)
            Destroy(CircleSelectObject);
        CircleSelectObject = Instantiate(CircleSelectPrefab, CircleSelectPos, false);
        if (AttackRadiusObject != null)
            Destroy(AttackRadiusObject);
        GameMode.gameHUD.targetSelect = gameObject;
    }

    virtual protected void Update()
    {
    }
}
