using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NormalModifier
{
    public float CriticalChance;
    public static Sprite icon;
    public NormalModifier(float _criticalChance)
    {
        SetCriticalChance(_criticalChance);
    }

    public bool GetCritical()
    {
        int rand = UnityEngine.Random.Range(0, 99);
        if (rand >= 0 && rand <= CriticalChance * 100)
            return true;
        else
            return false;
    }

    public void SetCriticalChance(float _criticalChance)
    {
        CriticalChance = _criticalChance;
    }
}
