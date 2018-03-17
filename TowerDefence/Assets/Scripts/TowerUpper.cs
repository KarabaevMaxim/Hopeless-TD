using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpper : MonoBehaviour
{
    public Transform BltSpawnPoint;
	// Use this for initialization
	void Start ()
    {
        if (BltSpawnPoint == null)
            Debug.LogWarning("У объекта " + name + " не проинициализирована переменная BltSpawnPoint");
	}

}
