using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTowerActiveDecreaseArmor : DebuffTowerActiveBase
{
    [Header("Снижающий броню дебаф")]
    [Range(0, 1)] public float DecreaceArmor;

    override protected void Start()
    {
        base.Start();
        Unit.DecreaseArmor(DecreaceArmor);
    }


    override protected void Update()
    {
        if (DebuffTime >= Duration)
        {
            // снятие дебафа
            Unit.RestoreArmor();
            Destroy(gameObject);
        }
        else
        {
            // действие дебафа

            DebuffTime += Time.deltaTime * GameMode.TimeSpeedMultyplier;
        }

    }
}
