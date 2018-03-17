using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string Name;
    public string Description;
    public List<GameObject> Units = new List<GameObject>();
}
