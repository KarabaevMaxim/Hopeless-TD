using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class DataSet : MonoBehaviour
{
    public List<GameObject> CindTowers = new List<GameObject>(); // префабы всех видов башен в игре
    public float DifficultyMultyplier = 1.0f;
    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
