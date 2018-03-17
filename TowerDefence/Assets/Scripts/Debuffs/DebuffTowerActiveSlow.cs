using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTowerActiveSlow : DebuffTowerActiveBase
{
    [Header("Замедляющий дебаф")]
    [Range(0, 1)] public float SlowPercent;
   
    override protected void Start()
    {
        base.Start();
        Unit.Slow(SlowPercent); // замедление/стан
    }


    override protected void Update()
    {
        if (DebuffTime >= Duration)
        {
            // снятие дебафа
            Unit.RestoreSpeed();
            Destroy(gameObject);
        }
        else
        {
            // действие дебафа
            DebuffTime += Time.deltaTime * GameMode.TimeSpeedMultyplier;
        }

    }
}
