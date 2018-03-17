using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTowerActiveBurn : DebuffTowerActiveBase
{
    [Header("Обжигающий дебаф")]
    public float PeriodicalDamage;

    override protected void Start()
    {
        base.Start();
    }


    override protected void Update()
    {
        if (DebuffTime >= Duration)
        {
            // снятие дебафа
            Destroy(gameObject);
        }
        else
        {
            // действие дебафа
            Unit.GetDamage(PeriodicalDamage, TypeElement);
            DebuffTime += Time.deltaTime * GameMode.TimeSpeedMultyplier;
        }

    }
}
