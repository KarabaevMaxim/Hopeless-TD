using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AchievementSystem : MonoBehaviour
{
    public List<Achievement> Achievements = new List<Achievement>();
    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
    public void Save()
    {
        string _stringToSave = "";
        // сохранение по шаблону [id];[CurrentValue];[IsCompleted]|[id];[CurrentValue];[IsCompleted]|[id];[CurrentValue];[IsCompleted]
        for (int i = 0; i < Achievements.Count; i++) // | - разделитель между записями, ; - разделитель между полями
        {
            _stringToSave += i.ToString() + ";" + // id
                             Achievements[i].CurrentValue.ToString() + ";" + // CurrentValue
                             Achievements[i].IsCompleted + ";" + // IsCompleted
                             "|"; // разделитель
        }
        PlayerPrefs.SetString("Achievements", _stringToSave);
        PlayerPrefs.Save();
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("Achievements"))
        {
            string[] loadedRecords = PlayerPrefs.GetString("Achievements").Split(new char[] { '|' });
            string[] loadedFields;
            for (int i = 0; i < loadedRecords.Length; i++)
            {
                loadedFields = loadedRecords[i].Split(new char[] { ';' });

                int _achIndex = -1;
                if (int.TryParse(loadedFields[0], out _achIndex))
                {
                    int _curValue = -1;
                    if (int.TryParse(loadedFields[1], out _curValue))
                    {
                        Achievements[_achIndex].CurrentValue = _curValue;
                    }
                    bool _isComp = false;
                    if (bool.TryParse(loadedFields[2], out _isComp))
                    {
                        Achievements[_achIndex].IsCompleted = _isComp;
                    }
                }
            }

        }
    }
    public void SetAchieve(int _id, int _addValue)
    {
        if (Achievements.Count < _id || Achievements[_id].IsCompleted) return;
        Achievements[_id].CurrentValue += _addValue;
        if(Achievements[_id].CurrentValue >= Achievements[_id].TargetValue)
        {
            Achievements[_id].CurrentValue = Achievements[_id].TargetValue;
            Achievements[_id].IsCompleted = true;
        }
    }
}
