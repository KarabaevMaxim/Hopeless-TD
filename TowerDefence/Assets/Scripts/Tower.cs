using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Tower
{
    public string Name;
    public List<TowerStats> Stats = new List<TowerStats>();
    
    [HideInInspector] public int maxLevel = 3;
    public float bltSpeed;
    public Element damageElement;
    public GameObject BulletPref;
    public GameObject BulletSpawn;
    public GameObject PSpref; // в дочерний класс
    [HideInInspector] public GameObject tGround;
    public float rotationSpeed = 1.0f;

    public Sprite Icon;

    public int CurLevel;
    public int CurCost;
    public int CurUpgradeCost;
    public int CurDamage;
    public float CurRange;
    public float CurAtkSpeed;
    public float CurAoe;
    public int CurSellCost;


    // в дочерний
    [HideInInspector] public NormalModifier normalModifier; 
    public List<ModifierStats> ModifierStats;

    public int CurModifierLevel;
    [HideInInspector]public int MaxModifierLevel;
    public float CurCriticalChance;
    public float CurCriticalMultyple; // сила крита
    public int CurUpgradeModifierCost;
    public int CurIncreaseToSell;

    public Tower()
    {
        normalModifier = new NormalModifier(CurCriticalChance);
    }

    public void Update()
    {
        CurCost = Stats[CurLevel].Cost;
       
        CurDamage = Stats[CurLevel].Damage;
        CurRange = Stats[CurLevel].Range;
        CurAtkSpeed = Stats[CurLevel].atkSpeed;
        CurAoe = Stats[CurLevel].Aoe;
        CurSellCost = Stats[CurLevel].SellCost;

        maxLevel = Stats.Count - 1;

    }

    public void Upgrade()
    {
        CurLevel++;
        Update();
    }

    public void UpdateModifier()
    {
        CurCriticalChance = ModifierStats[CurModifierLevel].CriticalChance;
        CurCriticalMultyple = ModifierStats[CurModifierLevel].Multyple;
        CurUpgradeModifierCost = ModifierStats[CurModifierLevel].UpgradeCost;
        CurIncreaseToSell = ModifierStats[CurModifierLevel].IncreaseToSell;
        MaxModifierLevel = ModifierStats.Count - 1;
        normalModifier.SetCriticalChance(CurCriticalChance);
    }

    public void UpgradeModifier()
    {
        CurModifierLevel++;
        UpdateModifier();
    }
}
