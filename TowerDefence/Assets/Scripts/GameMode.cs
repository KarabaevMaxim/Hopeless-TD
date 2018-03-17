using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
public enum AttackType
{
    Target = 0,
    NonTarget,
    Chain,
    Income
}
public enum Element
{
    Normal = 0,
    Fire,
    Water,
    Electric,
    Earth
}

public class GameMode : MonoBehaviour
{
    public int Diamonds = 0;
    [HideInInspector] public int LosesUnits = 0;
    public int maxLostUnits = 100;
    public int CountWaves;
    //[HideInInspector]
    public int CurWave;
    public int NextWave;
    [HideInInspector] public List<GameObject> GameUnits = new List<GameObject>();
    [HideInInspector] public List<GameObject> GameTowers = new List<GameObject>(); // построенные башни
    public List<GameObject> CindTowers = new List<GameObject>(); // префабы всех видов башен в игре
     public List<GameObject> GroundTower = new List<GameObject>();
    [HideInInspector] public int selectedGroundTowerID = -1;
    public List<Wave> Waves = new List<Wave>();
    public List<GameObject> Areas = new List<GameObject>();

    public float timeToNextWave = 5.0f;
    public float timeWave = 5.0f;
    public float timeToNextUnit = 1.5f;
    private float timeUnit = 1.5f;
    [HideInInspector] public float timeToWarning = 2.0f;

    private bool created = true;
    private int key = 0;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector]public bool victory = false;
    [HideInInspector]public bool pause = false;
    static public GameHUD gameHUD;
    [HideInInspector] public Globals globals;
    //время
    [HideInInspector] public int timeM, timeS;
    private float _timeMS;

    static public float TimeSpeedMultyplier = 1.0f;
    static public float DifficultyMultyplier = 1.0f;

    static public GameObject PriorityTarget;
    public int CurrentLevelRate = 10;
    private void Start()
    {
        gameOver = false;
        victory = false;
        timeWave = timeToNextWave;
        timeUnit = 0.0f;
        CountWaves = Waves.Count;
        CurWave = 0;
        foreach (var item in GameObject.FindGameObjectsWithTag("Tower"))
        {
            GameTowers.Add(item);
        }
        int _i = 0;
        GroundTower = GameObject.FindGameObjectsWithTag("TowerFoundation").ToList();
        foreach (var item in GroundTower)
        {
            item.GetComponent<TowerFoundation>().id = _i;
            _i++;
        }
        gameHUD = GameObject.FindGameObjectWithTag("UI").GetComponent<GameHUD>();
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
    }

    void Update()
    {
        //время
        if (!gameOver && !victory && !pause)
        {
            if (_timeMS >= 1.0f)
            {
                _timeMS = 0.0f;
                timeS++;
                if (timeS >= 60)
                {
                    timeS = 0;
                    timeM++;
                }
            }
            else
            {
                _timeMS += Time.deltaTime;
            }
            //поражение
            if (LosesUnits >= maxLostUnits)
                GameOver();
            else
                if (GameUnits.Count <= 0 && !created)
                Victory();

            if (timeWave <= 0.0f && created) 
            {
                NextWave = CurWave + 1;
                if (CurWave < CountWaves)
                {              
                    if (timeUnit <= 0)
                    {
                        if (key <= Waves[CurWave].Units.Count - 1)
                        {
                            GameUnits.Add(Instantiate(Waves[CurWave].Units[key], 
                            new Vector3(Areas[0].transform.position.x, 
                                        Areas[0].transform.position.y + Waves[CurWave].Units[key].GetComponent<UnitBase>().HeightSpawn,
                                        Areas[0].transform.position.z),
                                        Quaternion.identity));
                            key++;
                            timeUnit = timeToNextUnit;
                        }
                        else
                        {
                            CurWave++;
                            key = 0;
                            timeUnit = timeToNextUnit;
                            if (CurWave < CountWaves)
                            {
                                
                                timeWave = timeToNextWave;
                            }

                        }
                    }
                    else
                    {
                        timeUnit -= Time.deltaTime;
                    }
                    
                }
                else
                {
                    created = false;
                }
            }
            else
            {
                if (CurWave < CountWaves)
                {
                    timeWave -= Time.deltaTime * TimeSpeedMultyplier;
                    if (timeWave <= timeToWarning)
                    {
                        gameHUD.showTextWarning();
                    }
                }
            }
            for (int i = 0; i < GameUnits.Count; i++)
            {
                for (int j = 0; j < GameUnits.Count - 1; j++)
                {
                    if (GameUnits[j].GetComponent<UnitBase>().DistanceToFinish > GameUnits[j + 1].GetComponent<UnitBase>().DistanceToFinish)
                    {
                        GameObject _go = GameUnits[j];
                        GameUnits[j] = GameUnits[j + 1];
                        GameUnits[j + 1] = _go;
                    }
                }
            }
        }
    }
    public void BuildTower(int _buttonIndex)
    {
        TowerBase _cindTower = CindTowers[_buttonIndex].GetComponentInChildren<TowerBase>();
        if (Diamonds >= _cindTower.TowerStats[0].Cost)
        {
            GameTowers.Add(Instantiate(CindTowers[_buttonIndex], 
                     GroundTower.Where(gt => gt.GetComponent<TowerFoundation>().id == selectedGroundTowerID).FirstOrDefault().transform.position, 
                     Quaternion.identity));
            TowerBase _gameTower = GameTowers[GameTowers.Count - 1].GetComponentInChildren<TowerBase>();
            _gameTower.TowerFoundation = GroundTower[selectedGroundTowerID];
            _gameTower.TowerFoundation.SetActive(false);
            Diamonds -= _cindTower.TowerStats[0].Cost;
            gameHUD.closeTowerShop();
        }
        else
        {
            gameHUD.showNotEnoughResoursesPanel();
        }
    }

    public void GameOver()
    {
        gameHUD.showGameOverPanel();
        gameOver = true;
    }
    public void Victory()
    {
        gameHUD.showVictoryPanel();
        victory = true;
        globals.PassLevel(CurrentLevelRate);
        
    }
    public void OpenNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 <= globals.gameLevels.Count)
        {
            int _nextLevelNumber = SceneManager.GetActiveScene().buildIndex - 1;
            globals.gameLevels[_nextLevelNumber].opened = true;// открываем следующий уровень
          //  PlayerPrefs.SetString("Level" + _nextLevelNumber, lan);
        }
        else
            Debug.Log("пройден последний уровень");
    }
}
