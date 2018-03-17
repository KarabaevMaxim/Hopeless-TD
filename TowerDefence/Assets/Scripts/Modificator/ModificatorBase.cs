using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModificatorType
{
    ImmediateModificator = 0,
    LastingModificator
}

public class ModificatorBase : MonoBehaviour {

    [Header("Общие характеристики модификатора")]
    public string Name;
    public Sprite Icon;
    public ModificatorType Type;
    protected GameMode gameMode;
   

    virtual protected void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    virtual protected void Update()
    {
       
    }

}
