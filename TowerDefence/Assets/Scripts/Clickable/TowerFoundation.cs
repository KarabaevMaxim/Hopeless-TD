using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerFoundation : ClickableObject
{
    //static int FoundationCount = 0;
    public int id = -1;


    override protected void Start()
    {
        base.Start();
        
        //id = FoundationCount;
        //FoundationCount++;
    }

    override protected void Update ()
    {
        base.Update();
    }

    protected override void OnClick()
    {
        base.OnClick();
        GameMode.gameHUD.targetSelect = gameObject;
        gameMode.selectedGroundTowerID = id;
        GameMode.gameHUD.ShowTowerShop();
    }

    private void OnMouseDown()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
            if (!EventSystem.current.IsPointerOverGameObject())
                OnClick();
    }
}
