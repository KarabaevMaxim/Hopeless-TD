using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnitSimple : UnitBase
{
    override protected void Start ()
    {
        base.Start();
	}
	
	override protected void Update ()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
            base.Update();
		// сюда добавлять уникальные способности юнитов
	}

    private void OnMouseDown()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
            if (!EventSystem.current.IsPointerOverGameObject())
                OnClick();
    }

    protected override void OnClick()
    {
        base.OnClick();
    }
}
