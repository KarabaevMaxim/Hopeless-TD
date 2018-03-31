using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
public class Globals : MonoBehaviour
{
    public List<Level> GameLevels = new List<Level>();
    public static int CountNonPlayableScenes = 2;
    public byte MaxLevelRate;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadLevelsInfo();

        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LoadLevelsInfo() // загрузка информации обо всех уровнях с диска
    {
        foreach (var item in GameLevels)
        {
            ReadLevelInfo(item.LevelID);
        }
    }
    private void ReadLevelInfo(int _id)
    {
        if (PlayerPrefs.HasKey("Level" + _id))
        {
            Level _level = GameLevels.Where(l => l.LevelID == _id).FirstOrDefault();
            bool _opened = false, _completed = false;
            byte _rate = 0;
            ParseLevelInfo(PlayerPrefs.GetString("Level" + _id), out _opened, out _completed, out _rate);
            _level.InitLevel(_opened, _completed, _rate);
        }
        else
        {
            SaveLevelInfo(_id, 0, _id == 0 ? true : false, false);
        }
    }
    private void ParseLevelInfo(string _content, out bool _opened, out bool _completed, out byte _rate)
    {
        string[] _info = _content.Split(new char[] { ';' });
        _opened = _info[0] == "1" ? true : false;
        _completed = _info[1] == "1" ? true : false;
        if (!byte.TryParse(_info[2], out _rate))
            Debug.LogWarning("Не удалось считать рейтинг миссии.");
    }
    public void SaveLevelInfo(int _id, byte _rate, bool _opened, bool _completed) // сохранение информации об уровне
    {
        Level _level = GameLevels.Where(l => l.LevelID == _id).FirstOrDefault();
        _level.InitLevel(_opened, _completed, _rate);
		string _content = (_opened ? "1" : "0") + ";" + (_completed ? "1" : "0") + ";" + _rate; //_opened;_completed;_rate
        PlayerPrefs.SetString("Level" + _id, _content); // [LevelID] _opened;_completed;_rate
        PlayerPrefs.Save();
    }
    public void CompleteLevel(byte _rate) // пройти уровень
    {
        if (_rate > MaxLevelRate)
            _rate = MaxLevelRate;
        else
        {
            if (_rate < 0)
                _rate = 0;
        }

		SaveLevelInfo(Level.CurrentLevelID, _rate, true, true); // записать информацию об актуальном уровне
        OpenLevel(Level.CurrentLevelID + 1); // открыть следующий за актуальным уровень
    }
	private void OpenLevel(int _index)
	{
        GameLevels[_index].OpenLevel();
        SaveLevelInfo(_index, 0, true, false);
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        LoadLevelsInfo();
    }
    public void LoadLevel(int _index)
    {
        if (_index <= GameLevels.Count)
        {
            SceneManager.LoadScene(_index + CountNonPlayableScenes);
            Level.CurrentLevelID = _index;
        }
        else
            Debug.LogError("Такого уровня нету.");
    }
    public void LoadNextLevel() // загрузить следующий за актуальным уровень
    {
        if (GameLevels[Level.CurrentLevelID + 1].Opened)
            LoadLevel(Level.CurrentLevelID + 1);
        else
            Debug.Log("Уровень не открыт.");
    }
    public void ReloadCurrentLevel() // перезапуск уровня
    {
        LoadLevel(Level.CurrentLevelID);
    }
}
