using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtnController : MonoBehaviour
{
    GameHUD gameHUD;
    [HideInInspector] public int ID;
	void Start ()
    {
        gameHUD = GameObject.FindGameObjectWithTag("UI").GetComponent<GameHUD>();

    }

    public void Close()
    {
       // gameHUD.buttonTowers[ID] = null;
        Destroy(gameObject);
    }
}
