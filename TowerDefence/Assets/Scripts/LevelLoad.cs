using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class LevelLoad : MonoBehaviour
{
    public int mapId;

    public void MouseClick()
    {
        MainMenu.globals.LoadLevel(mapId);
    }
}
