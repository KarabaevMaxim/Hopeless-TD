using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerEarth : TowerBase
{
    private ModificatorLasting modificator;
    override protected void Start ()
    {
        base.Start();
        modificator = GetComponent<ModificatorLasting>();
    }
    override protected void Update ()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
        {
            base.Update();
            Attack(modificator);
        }
    }

    private void OnMouseDown()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
            if (!EventSystem.current.IsPointerOverGameObject())
                OnClick();
    }

    override protected void OnClick()
    {
        base.OnClick();
        GameMode.gameHUD.ShowDescriptionTower(Name, CurDamage, UpgradeCostOutput, CurSellCost, Icon);
    }
}
