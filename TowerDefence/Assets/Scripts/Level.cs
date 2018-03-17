using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Level
{
    public string name;
    public int LevelID;
    [SerializeField] public Scene scene;
    public Sprite icon;
    public bool opened;
    public bool completed;
    public int Rate;
    static public int CurrentLevelID = -1;
    public override string ToString()
    {
        return "Название: " + name + "/ID: " + LevelID + "/Открыта: " + opened + "/Завершена: " + completed + "/Рейтинг: " + Rate;
    }

    void CompleteLevel()
    {
        completed = true;
    }
    void OpenLevel()
    {
        opened = true;
        Debug.Log(name + " Открываю левел");
    }
    void RateLevel(int _rate)
    {
        Rate = _rate;
    }
    public void PassLevel(int _rate, Level _next)
    {
        CompleteLevel();
        _next.OpenLevel();
        RateLevel(_rate);
    }
    public void InitLevel(bool _opened, bool _completed, int _rate)
    {
        opened = _opened;
        completed = _completed;
        Rate = _rate;
    }
}
