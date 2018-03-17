using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class Globals : MonoBehaviour {

	public string playerName;
    public List<Level> gameLevels = new List<Level>();
    public static int CountNonPlayableScenes = 2;
    [SerializeField] private string PlayerPrefsStringTemplate;
    [SerializeField] private char delimiter;
    public int MaxLevelRate;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        ReadLevels();
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReadLevels()
    {
        string _template = "";
        for (int i = 0; i < gameLevels.Count; i++)
        {
            gameLevels[i].LevelID = i;
            if (!PlayerPrefs.HasKey("Level" + gameLevels[i].LevelID)) // первичная инициализация
            {
                PlayerPrefs.SetString("Level" + gameLevels[i].LevelID, GetSaveString(i == 0 ? 1 : 0, 0, 0));
                Debug.Log(GetSaveString(i == 0 ? 1 : 0, 0, 0));
            }
            else
            {
               // _template = PlayerPrefs.GetString("Level" + gameLevels[i].LevelID);

                //ParseString(_template, out _opened, out _completed, out _rate);
                //gameLevels[i].InitLevel(_opened, _completed, _rate);
            }
            bool _opened, _completed;
            int _rate;
            _template = PlayerPrefs.GetString("Level" + gameLevels[i].LevelID);
            ParseString(_template, out _opened, out _completed, out _rate);
            gameLevels[i].InitLevel(_opened, _completed, _rate);
            //    Debug.Log(gameLevels[i].ToString());
        }
        
    }
    string GetSaveString(int _isOpened, int _isCompleted, int _rate)
    {
        if (_isOpened > 1)
            _isOpened = 1;
        if (_isCompleted > 1)
            _isCompleted = 1;
        if (_rate > MaxLevelRate)
            _rate = MaxLevelRate;
        return _isOpened.ToString() + delimiter + _isCompleted + delimiter + _rate; // получается строка вида 0;0;0
    }
    void ParseString(string _loadedString, out bool _isOpened, out bool _isCompleted, out int _rate)
    {
        if(_loadedString.Length != PlayerPrefsStringTemplate.Length)
        {
            Debug.Log(_loadedString.Length + " " + PlayerPrefsStringTemplate.Length);
            Debug.LogError("Строка \"" + _loadedString + "\" имеет неверный формат");
            _isOpened = false;
            _isCompleted = false;
            _rate = 0;
            return;
        }
        _isOpened = Convert.ToBoolean(int.Parse(_loadedString[0].ToString()));
        _isCompleted = Convert.ToBoolean(int.Parse(_loadedString[2].ToString()));
        _rate = int.Parse(_loadedString[4].ToString());
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        ReadLevels();
    }
    public void ReadProfileName()
    {
        if (PlayerPrefs.HasKey("Profile_name"))
            playerName = PlayerPrefs.GetString("Profile_name");
        else
            playerName = "";
    }
    public void PassLevel(int _rate)
    {
        if (_rate > MaxLevelRate)
            _rate = MaxLevelRate;
        if (_rate < 0)
            _rate = 0;
        gameLevels[Level.CurrentLevelID].PassLevel(_rate, gameLevels[Level.CurrentLevelID + 1]);
        if (PlayerPrefs.HasKey("Level" + Level.CurrentLevelID)) 
        {
            PlayerPrefs.SetString("Level" + Level.CurrentLevelID, "1;1;" + _rate);
        }
        else
        {
            Debug.LogError("Записи Level" + Level.CurrentLevelID + " не существует.");
        }
    }

    public void LoadLevel(int _index)
    {
        if (_index <= gameLevels.Count)
        {
            SceneManager.LoadScene(_index + CountNonPlayableScenes);
            Level.CurrentLevelID = _index;
        }
        else
            Debug.LogError("Такого уровня нету.");
    }
    public void LoadNextLevel()
    {
        if (gameLevels[Level.CurrentLevelID + 1].opened)
            LoadLevel(Level.CurrentLevelID + 1);
        else
            Debug.Log("Уровень не открыт.");
    }
    public void ReloadCurrentLevel()
    {
        LoadLevel(Level.CurrentLevelID);
    }
}
