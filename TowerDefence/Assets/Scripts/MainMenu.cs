using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class MainMenu : MonoBehaviour {

    public GameObject mainPanel;
    [Header("Миссии")]
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject levelListPrefab;
    [SerializeField] private Transform levelListParent;
    [SerializeField] private Sprite levelNotOpenedIcon;
    [SerializeField] private Sprite levelOpenedIcon;
    [SerializeField] private Sprite starNotOpenedIcon;
    [SerializeField] private Sprite starOpenedIcon;
    public List<GameObject> LevelList = new List<GameObject>();

    [Header("Достижения")]
    [SerializeField] private GameObject achievementsField;
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private Transform achievementListParent;
    private AchievementSystem achieveSystem;
    private List<GameObject> achieveElements = new List<GameObject>();
    [Header("Настройки")]
    public GameObject settingsPanel;
    public Dropdown LanguageCmb;

    static public Globals globals;
	void Start ()
    {
        //LocalizationManager.instance.Translate();
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
        achieveSystem = globals.GetComponent<AchievementSystem>();
        if (achieveSystem == null)
            Debug.LogWarning("Компонент AchieveSystem не найден у объекта с тегом Globals");
        showMainMenu();
    }
    public void ShowSettings()
    {
        CloseMainMenu();
        settingsPanel.SetActive(true);
        LocalizationManager.instance.Translate();
        LanguageDropDownUpdate();
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void showMainMenu()
    {
        mainPanel.SetActive(true);
        LocalizationManager.instance.Translate();
        CloseSettings();
        CloseAchievements();
        CloseMap();
    }
    public void CloseMainMenu()
    {
        mainPanel.SetActive(false);
    }
    void OpenLevel(int _index)
    {
        globals.LoadLevel(_index);
    }
    UnityAction OpenLevelDelegate(int _index)
    {
        return delegate { OpenLevel(_index); };
    }
    public void ShowMap()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        levelsPanel.SetActive(true);
        for (int i = 0; i < globals.GameLevels.Count; i++)
        {
            LevelList.Add(Instantiate(levelListPrefab, levelListParent));
            LevelListElement _lle = LevelList.Last().GetComponent<LevelListElement>();
            _lle.NumberText.text = (globals.GameLevels[i].LevelID + 1).ToString();
            if(!globals.GameLevels[i].Opened)
            {
                LevelList.Last().GetComponent<Image>().sprite = levelNotOpenedIcon;
                LevelList.Last().GetComponent<Button>().interactable = false;
            }
            else
            {
                LevelList.Last().GetComponent<Image>().sprite = levelOpenedIcon;
                _lle.LockedImg.color += new Color(0, 0, 0, -255);
                if(globals.GameLevels[i].Completed)
                {
                    for (int j = 0; j < globals.GameLevels[i].Rate; j++)
                    {
                        _lle.Stars[j].sprite = starOpenedIcon;
                    }
                }
            }
            LevelList.Last().GetComponent<Button>().onClick.AddListener(OpenLevelDelegate(globals.GameLevels[i].LevelID));
        }
        LocalizationManager.instance.Translate();
    }
    public void CloseMap()
    {
        levelsPanel.SetActive(false);
        if (LevelList.Count > 0)
        {
            foreach (var item in LevelList)
            {
                Destroy(item);
            }
            LevelList.Clear();
        }
    }

    public void UpdateLanguage()
    {
        switch (LanguageCmb.value)
        {
            case 0:
                LocalizationManager.instance.SetLang("RU");
                LocalizationManager.instance.Translate();
                break;
            case 1:
                LocalizationManager.instance.SetLang("EN");
                LocalizationManager.instance.Translate();
                break;
            default:
                break;
        }
    }

    public void LanguageDropDownUpdate()
    {
        string _curLang = LocalizationManager.instance.GetLang();
        switch (_curLang)
        {
            case "RU":
                LanguageCmb.value = 0;
                break;
            case "EN":
                LanguageCmb.value = 1;
                break;
            default:
                break;
        }
    }


    public void ResetButtonClick()
    {
        globals.ResetPlayerPrefs();
    }

    public void ShowAchievements()
    {
        for (int i = 0; i < achieveSystem.Achievements.Count; i++)
        {
            achieveElements.Add(Instantiate(achievementPrefab, achievementListParent));
            RectTransform _rt = achieveElements.Last().GetComponent<RectTransform>();

            AchievementListElement _ale = achieveElements.Last().GetComponent<AchievementListElement>();
            _ale.TitleText.text = achieveSystem.Achievements[i].Name;
            _ale.DescText.text = achieveSystem.Achievements[i].Description;
            _ale.TargetText.text = achieveSystem.Achievements[i].CurrentValue + "/" + achieveSystem.Achievements[i].TargetValue;
            _ale.ProgressBar.fillAmount = achieveSystem.Achievements[i].CurrentValue / (float)achieveSystem.Achievements[i].TargetValue;
            _ale.Icon.sprite = achieveSystem.Achievements[i].IsCompleted ? achieveSystem.Achievements[i].UnlockedIcon : achieveSystem.Achievements[i].LockedIcon;
            
            _rt.anchoredPosition = new Vector2(_rt.sizeDelta.x / -2, -(i + 1) * (_rt.sizeDelta.y));

        }
        CloseMainMenu();
        achievementsField.SetActive(true);
        LocalizationManager.instance.Translate();
    }
    private void CloseAchievements()
    {
        achievementsField.SetActive(false);
    }
}
