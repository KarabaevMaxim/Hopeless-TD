using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Events;
public class GameHUD : MonoBehaviour
{
    public Text DiamondText;
    public Text lostText;
    private GameMode gameMode;
    public Text waveText;
    public Text timeToNextWaveText;

    [HideInInspector] public GameObject targetSelect;

    Globals globals;


    //магазин
    [SerializeField] private GameObject buttonTowerPrefab;
    [SerializeField] private Transform towerButtonsParent;
    public List<GameObject> buttonTowers = new List<GameObject>();
    public GameObject GameOverPanel; // панель интерфеса после поражения
    public GameObject VictoryPanel; // панель интерфейса после победы
    public GameObject textWarning;
    [HideInInspector] public float timerToCloseWarning = 0;
    [HideInInspector] public float timeToCloseWarning = 2.0f;
    [HideInInspector] public bool IsButtonsTowerShow = false;
    
    

    [HideInInspector] public Animator warningTextAnim;
    [HideInInspector] public bool isWarningShowed;
    public GameObject PausePanel;
    [HideInInspector] public GameObject UI;

    //отображение критов
    [HideInInspector] public bool isShowCritDamage = false;
    public GameObject damageTextPrefab;
    [HideInInspector]public GameObject DamageTarget;



    public GameObject NotEnoughResoursesPanel;
    [HideInInspector]public bool isNotEnoughResoursesPanelShowed = false;
    Animator NotEnoughResoursesPanelAnim;

    //описание юнитов
    public GameObject DescriptionUnitPanel;
    public Text TextUnitName;
    public Text TextUnitArmor;
    public Text TextUnitResistance;
    public Text TextUnitHealth;
    public Image HealthBarRound;
    public Image IconUnit;

    // описание башен
    public GameObject DescriptionTowerPanel;
    public Text TextTowerName;
    public Text TextTowerDamage;
    public Text TextUpgradeCost;
    public Text TextSellCost;
    public Image IconTower;

    public Transform DynamicObjectsParent;

    public void ShowDescriptionTower(string _name, float _damage, string _upgradeCost, int _sellCost, Sprite _icon)
    {
     //   LocalizationManager.instance.Translate();
        CloseDescriptionUnit();
        CloseTowerShop();
        DescriptionTowerPanel.SetActive(true);
        TextTowerName.text = _name;
        TextTowerDamage.text = _damage.ToString();
        TextUpgradeCost.text = LocalizationManager.instance.GetWord("UpgradeTowerBtn") + _upgradeCost;
        TextSellCost.text = LocalizationManager.instance.GetWord("SellTowerBtn") + _sellCost;
        IconTower.sprite = _icon;
    }

    public void CloseDescriptionTower()
    {
        DescriptionTowerPanel.SetActive(false);
    }
    public void ShowDescriptionUnit(string _name, float _armor, float _resist, float _curHealth, float _startHealth, Sprite _icon)
    {
        //LocalizationManager.instance.Translate();
        CloseDescriptionTower();
        CloseTowerShop();
        DescriptionUnitPanel.SetActive(true);
        TextUnitName.text = _name;
        TextUnitArmor.text = Mathf.Round(_armor).ToString();
        TextUnitResistance.text = Mathf.Round(_resist * 100) + "%";
        TextUnitHealth.text = Mathf.Round(_curHealth) + "/" + Mathf.Round(_startHealth);
        HealthBarRound.fillAmount = _curHealth / _startHealth;
        IconUnit.sprite = _icon;
    }
    public void CloseDescriptionUnit()
    {
        DescriptionUnitPanel.SetActive(false);
    }

    public void ShowPausePanel()
    {
        PausePanel.SetActive(true);
        gameMode.pause = true;
    }
    public void ClosePausePanel()
    {
        PausePanel.SetActive(false);
        gameMode.pause = false;
    }
    void Start()
    {
        //LocalizationManager.instance.Translate();
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        PausePanel.SetActive(false);
        CloseDescriptionUnit();
        CloseDescriptionTower();
        GameOverPanel.SetActive(false);
        VictoryPanel.SetActive(false);
        CloseTowerShop();
        timerToCloseWarning = timeToCloseWarning;
        warningTextAnim = textWarning.gameObject.GetComponent<Animator>();
        NotEnoughResoursesPanelAnim = NotEnoughResoursesPanel.gameObject.GetComponent<Animator>();

        isWarningShowed = false;
        UI = GameObject.FindGameObjectWithTag("UI");
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        ClosePausePanel();
    }

    void LateUpdate()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
        {
            lostText.text = gameMode.LosesUnits + "/" + gameMode.maxLostUnits;
            DiamondText.text = gameMode.Diamonds.ToString();
            lostText.text = gameMode.LosesUnits + "/" + gameMode.maxLostUnits;
            waveText.text = (gameMode.CurWave) + "/" + gameMode.Waves.Count;
            timeToNextWaveText.text = Math.Round(gameMode.timeWave).ToString();
            if (targetSelect != null)
            {
                if (Input.GetMouseButton(1))
                {
                    if (ClickableObject.CircleSelectObject != null)
                        Destroy(ClickableObject.CircleSelectObject);
                    if (ClickableObject.AttackRadiusObject != null)
                        Destroy(ClickableObject.AttackRadiusObject);
                    CloseDescriptionTower();
                    CloseDescriptionUnit();
                    CloseTowerShop();
                    targetSelect = null;
                }
            }
            if(IsButtonsTowerShow)
            {
                setButtonsTowerPosition();
            }

        }
    }

  


    public void showCritText()
    {
        GameObject _text = Instantiate(damageTextPrefab, UI.transform);
        _text.GetComponent<DamageText>().target = DamageTarget;
        _text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10, -10);
        _text = null;
    }

    public void UpgradeTower()
    {
        targetSelect.GetComponent<TowerBase>().Upgrade();
    }
    public void SellTower()
    {
        targetSelect.GetComponent<TowerBase>().Sell();
        CloseDescriptionTower();
    }

    private void setButtonsTowerPosition()
    {
        List<GameObject> _cindTowers = GameMode.GameData.CindTowers;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(targetSelect.transform.position); // центр круга
        float _r = 10 * _cindTowers.Count; // радиус окружности, на которой будут располагаться кнопки магазина
        float _360degrees = Mathf.PI * 2;
        float _angleStep = _360degrees / (float)_cindTowers.Count;
        for (int i = 0; i < _cindTowers.Count; i++)
        {
            float _angle = (i + 1) * _angleStep;
            RectTransform _rt = buttonTowers[i].GetComponent<RectTransform>();
            Vector2 buttonPos = new Vector2((_r * Mathf.Cos(_angle)) + screenPos.x, //x = R * cos(угол) 
                                            (_r * Mathf.Sin(_angle)) + screenPos.y); //y = R * sin(угол)
            _rt.position = new Vector3(buttonPos.x, buttonPos.y, 0);
            
        }
    }
    public void ShowTowerShop()
    {
        CloseTowerShop();
        List<GameObject> _cindTowers = GameMode.GameData.CindTowers;
        IsButtonsTowerShow = true;
        float _width = 50; // ширина кнопки
        float _height = 50; // высота кнопки
        for (int i = 0; i < _cindTowers.Count; i++)
        {
            if (_cindTowers[i].GetComponent<TowerBase>() != null)
            {
                buttonTowers.Add(Instantiate(buttonTowerPrefab, towerButtonsParent));
                RectTransform _rt = buttonTowers[i].GetComponent<RectTransform>();
                _rt.sizeDelta = new Vector2(_width, _height);
                buttonTowers[i].GetComponent<Image>().sprite = _cindTowers[i].GetComponent<TowerBase>().Icon;
                buttonTowers[i].transform.Find("textCost").GetComponent<Text>().text = _cindTowers[i].GetComponent<TowerBase>().TowerStats[0].Cost.ToString();
                buttonTowers[i].GetComponent<Button>().onClick.AddListener(BuildTowerDelegate(i));
            }
        }
        CloseDescriptionUnit();
        CloseDescriptionTower();
    }
    UnityAction BuildTowerDelegate(int _index)
    {
        return delegate { BuildTower(_index); };
    }
    public void CloseTowerShop()
    {
        IsButtonsTowerShow = false;
        if (buttonTowers.Count > 0)
        {
            for (int i = 0; i < buttonTowers.Count; i++)
            {
                if (buttonTowers[i] != null)
                {
                    buttonTowers[i].GetComponent<Button>().onClick.RemoveListener(() => BuildTower(i));
                    Destroy(buttonTowers[i]);
                }
            }
            buttonTowers.Clear();
        }
    }
    public void showGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
    public void closeGameOverPanel()
    {
        GameOverPanel.SetActive(false);
    }
    public void showTextWarning()
    {
        if (!isWarningShowed)
        {
            warningTextAnim.SetBool("show", true);
            isWarningShowed = true;
        }

    }
    public void RestartLevel()
    {
        globals.ReloadCurrentLevel();
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void NextLevel()
    {
        globals.LoadNextLevel();
    }
    public void showVictoryPanel()
    {
        VictoryPanel.SetActive(true);
    }
    public void closeVictoryPanel()
    {
        VictoryPanel.SetActive(false);
    }

    public void showGameSettings()
    {

    }
    public void closeGameSettings()
    {

    }

    public void showNotEnoughResoursesPanel()
    {
        if (!isNotEnoughResoursesPanelShowed)
        {
            NotEnoughResoursesPanelAnim.SetBool("show", true);
            isWarningShowed = true;
        }
    }

    public void BuildTower(int _buttonIndex)
    {
        gameMode.BuildTower(_buttonIndex);
    }
}
