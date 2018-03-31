using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
    public string Name;
    public int LevelID;
    [SerializeField] public Scene LevelScene;
    public Sprite Icon;
    public bool Opened;
    public bool Completed;
    public byte Rate;
    static public int CurrentLevelID = -1;
    public override string ToString()
    {
        return "Название: " + Name + "/ID: " + LevelID + "/Открыта: " + Opened + "/Завершена: " + Completed + "/Рейтинг: " + Rate;
    }

    void CompleteLevel()
    {
        Completed = true;
    }
    public void OpenLevel()
    {
        Opened = true;
        Debug.Log(Name + " Открываю левел");
    }
    void RateLevel(byte _rate)
    {
        Rate = _rate;
    }
    public void InitLevel(bool _opened, bool _completed, byte _rate)
    {
        Opened = _opened;
        Completed = _completed;
        Rate = _rate;
    }
}
