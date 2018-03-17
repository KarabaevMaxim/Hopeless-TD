using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;

public class LocalizationManager : MonoBehaviour {

	public static LocalizationManager instance = null;

	public string[] tags;

	public TextAsset languageFile;

	private string lang;

	private string Lang
    {
		get
        {
			return lang;
		}

		set
        {
			PlayerPrefs.SetString("SLanguageL", value);
			lang = value;
		}
	}

	public string GetLang ()
    {
        if (!PlayerPrefs.HasKey("SLanguageL"))
            PlayerPrefs.SetString("SLanguageL", "RU");
        lang = PlayerPrefs.GetString("SLanguageL");
        return lang;
    }

	public void SetLang (string lan)
    {
		PlayerPrefs.SetString("SLanguageL", lan);
	}

	private Dictionary<string, Dictionary<string, string>> languages;

	private XmlDocument xmlDoc;
	private XmlReader reader;

	void Awake ()
    {
		instance = this;
        xmlDoc = new XmlDocument();
    }

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
		if(!PlayerPrefs.HasKey("SLanguageL"))
			Lang = tags[0];
		else
			Lang = PlayerPrefs.GetString("SLanguageL");

		languages = new Dictionary<string, Dictionary<string, string>>();
		reader = XmlReader.Create(new StringReader(languageFile.text));
		xmlDoc.Load(reader);

		for(int i = 0; i < tags.Length; i++)
        {
			languages.Add(tags[i], new Dictionary<string, string>());
			XmlNodeList langs = xmlDoc["Data"].GetElementsByTagName(tags[i]);
			for (int j = 0; j < langs.Count; j++)
            {
				languages[tags[i]].Add(langs[j].Attributes["Key"].Value, langs[j].Attributes["Word"].Value);
			}
		}
	}

	void Update ()
    {
		
	}

	public string GetWord(string lan, string key)
    {
		return languages[lan][key];
	}

	public string GetWord(string key)
    {
        try
        {
            return languages[lang][key];
        }
        catch (KeyNotFoundException ex)
        {
            Debug.LogError("Ошибка: " + ex.Message + " " + key);
            return languages[lang]["Error"];
        }
		
	}
    public void Translate()
    {
        GetLang();
        GameObject[] _objects = GameObject.FindGameObjectsWithTag("TranslatableObjects");
        foreach (var item in _objects)
        {
            item.GetComponent<Text>().text = instance.GetWord(item.name);
        }
        _objects = null;
    }
}
