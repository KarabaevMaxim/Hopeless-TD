using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerWater : TowerBase
{

    [HideInInspector] public ModificatorLasting Modificator;


    override protected void Start()
    {
        base.Start();
        Modificator = GetComponent<ModificatorLasting>();
    }

    override protected void Update()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
        {
            base.Update();
            Attack(Modificator);
        }
    }

    override protected void OnClick()
    {
        base.OnClick();
        GameMode.gameHUD.ShowDescriptionTower(Name, CurDamage, UpgradeCostOutput, CurSellCost, Icon);
    }
    private void OnMouseDown()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
            if (!EventSystem.current.IsPointerOverGameObject())
                OnClick();
    }

    
}
