using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour {

    public GameObject mainPanel;
    public GameObject settingsPanel;
    public Text profileNameTextInput;
    public Text StartProfileNameTextInput;
    public Text profileNameText;
    public GameObject mapPanel;
    public GameObject buttonMapPrefab;
    public List<GameObject> mapButtons = new List<GameObject>();
    public GameObject CreateNewProfilePanel;

    //ссылки для локализации
    public Dropdown LanguageCmb;

    static public Globals globals;
	void Start ()
    {
        //LocalizationManager.instance.Translate();
        globals = GameObject.FindGameObjectWithTag("Globals").GetComponent<Globals>();
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
    private void saveProfileName(Text _inputField)
    {
        PlayerPrefs.SetString("Profile_name", _inputField.text);
        globals.playerName = _inputField.text;
        PlayerPrefs.Save();
        CloseProfileNameStartInputWindow();
        updateProfileName();
    }
    public void SaveProfileBtnClick()
    {
        saveProfileName(profileNameTextInput.IsActive() ? profileNameTextInput : StartProfileNameTextInput);
    }
    public void showMainMenu()
    {
       
        mainPanel.SetActive(true);
        LocalizationManager.instance.Translate();
        updateProfileName();
        CloseSettings();
        CloseMap();
    }
    public void CloseMainMenu()
    {
        mainPanel.SetActive(false);
    }
    public void ShowMap()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        mapPanel.SetActive(true);
        List<GameObject> emptyButtons = new List<GameObject>();
        GameObject[] _emptyButtons = GameObject.FindGameObjectsWithTag("EmptyButtonMap");
        int _j = 0;
        foreach (var item in _emptyButtons)
        {
            emptyButtons.Add(_emptyButtons[_j]);
            _j++;
        }
        int _i = 0;
        foreach (var item in globals.gameLevels)
        {
            if (item.opened)
            {
                mapButtons.Add(Instantiate(buttonMapPrefab, emptyButtons[_i].transform));
                RectTransform _rt = mapButtons[_i].GetComponent<RectTransform>();
                _rt.anchorMin = new Vector2(0.045f, 0.05546643f);
                _rt.anchorMax = new Vector2(0.955f, 0.9444665f);
                _rt.offsetMin = new Vector2(0, 0);
                _rt.offsetMax = new Vector2(0, 0);
                mapButtons[_i].GetComponent<LevelLoad>().mapId = item.LevelID;
                mapButtons[_i].GetComponent<Image>().sprite = item.icon;
                Image _img = emptyButtons[_i].GetComponent<Image>();
                _img.color = new Color(_img.color.r,
                                       _img.color.g,
                                       _img.color.b, 
                                       0.0f);
                
            }
            else
            {
                Image _img = emptyButtons[_i].GetComponent<Image>();
                _img.color = new Color(_img.color.r,
                                       _img.color.g,
                                       _img.color.b,
                                      100.0f);
            }
            _i++;
        }
        LocalizationManager.instance.Translate();
    }
    public void CloseMap()
    {
        mapPanel.SetActive(false);
        if (mapButtons.Count > 0)
        {
            foreach (var item in mapButtons)
            {
                Destroy(item);
            }
            mapButtons.Clear();
        }
    }

    public void updateProfileName()
    {
        globals.ReadProfileName();
        if (globals.playerName != "")
            CloseProfileNameStartInputWindow();
        else
            ShowProfileNameStartInputWindow();
        profileNameText.text = globals.playerName;

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

    private void ShowProfileNameStartInputWindow()
    {
        CreateNewProfilePanel.SetActive(true);
    }
    private void CloseProfileNameStartInputWindow()
    {
        CreateNewProfilePanel.SetActive(false);
    }

    public void ResetButtonClick()
    {
        globals.ResetPlayerPrefs();
    }
}
