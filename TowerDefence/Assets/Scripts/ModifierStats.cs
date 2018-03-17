using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModifierStats
{
    [Range(0, 1)]public float CriticalChance = 0f;
    [Range(1, 10)] public float Multyple = 1.0f; // сила крита
    public int UpgradeCost;
    public int IncreaseToSell;


}
