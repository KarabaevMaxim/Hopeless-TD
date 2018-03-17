using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TowerResource : TowerBase {

    

    
    
    override protected void Start()
    {
        base.Start();
        Reloads = 0;
    }

    override protected void Update()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
        {
            base.Update();
            if (Reloads >= CurAttackSpeed)
            {
                animController.ToUp();
                Debug.Log("Приход");
                Reloads = 0;
            }
            else
            {
                Reloads += Time.deltaTime * GameMode.TimeSpeedMultyplier;

            }
            ShowIncomeText();
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

    void ShowIncomeText()
    {
        if (animController.IncomeTextObject != null)
        {
            animController.IncomeTextObject.GetComponentInChildren<Text>().text = "+" + CurDamage.ToString();
            Vector3 _screenPos = Camera.main.WorldToScreenPoint(transform.position);
            animController.IncomeTextObject.GetComponent<RectTransform>().position = new Vector3(_screenPos.x, _screenPos.y + 100, 0);
        }
    }

}
