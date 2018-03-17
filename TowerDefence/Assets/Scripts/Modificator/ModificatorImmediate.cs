using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModificatorImmediate : ModificatorBase
{
    [Header("Характеристики мгновенных эффектов")]

    [Range(0, 100)] public int CriticalChance;
    [Range(1, 10)] public float CriticalPower;


    override protected void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();
    }

    public bool GetCritical()
    {
        int _rand = Random.Range(0, 99);
        return (_rand >= 0 && _rand <= CriticalChance) ? true : false;
    }
}
