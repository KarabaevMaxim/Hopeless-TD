using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
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
    public GameObject shopBackgroud;
    public List<Text> textCost = new List<Text>();
    public List<GameObject> buttonTowers = new List<GameObject>();
    public GameObject GameOverPanel; // панель интерфеса после поражения
    public GameObject VictoryPanel; // панель интерфейса после победы
    public GameObject textWarning;
    [HideInInspector] public float timerToCloseWarning = 0;
    [HideInInspector] public float timeToCloseWarning = 2.0f;
    

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
        closeTowerShop();
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
      //  LocalizationManager.instance.Translate();
        CloseDescriptionTower();
        closeTowerShop();
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
        closeTowerShop();
        timerToCloseWarning = timeToCloseWarning;
        warningTextAnim = textWarning.gameObject.GetComponent<Animator>();
        NotEnoughResoursesPanelAnim = NotEnoughResoursesPanel.gameObject.GetComponent<Animator>();
        int i = 0;
        foreach (var item in buttonTowers) // кнопок пока строго 5
        {
            if (gameMode.CindTowers[i].GetComponent<TowerBase>() != null)
            {
                item.GetComponent<Image>().sprite = gameMode.CindTowers[i].GetComponent<TowerBase>().Icon;
                item.transform.Find("textCost").GetComponent<Text>().text = gameMode.CindTowers[i].GetComponent<TowerBase>().TowerStats[0].Cost.ToString();
                item.transform.Find("textName").GetComponent<Text>().text = gameMode.CindTowers[i].GetComponent<TowerBase>().Name;
            }
            i++;
        }
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
                    closeTowerShop();
                    targetSelect = null;
                }
            }
        }
    }

  


    public void showCritText()
    {
        GameObject _text = Instantiate(damageTextPrefab, UI.transform);
        _text.GetComponent<DamageText>().target = DamageTarget;
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


    public void showTowerShop()
    {
        shopBackgroud.SetActive(true);
        CloseDescriptionUnit();
        CloseDescriptionTower();
    }
    public void closeTowerShop()
    {
        if (shopBackgroud.activeInHierarchy)
        {
            shopBackgroud.SetActive(false);
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
