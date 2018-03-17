using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBottom : MonoBehaviour
{

    public Transform BuildEffectPosition;
	void Start ()
    {
        if (BuildEffectPosition == null)
            Debug.LogWarning(name + ": BuildEffectPosition не инициализирована. Точка появления эффекта при строительстве не указана");
	}
	
	void Update () {
		
	}
}
