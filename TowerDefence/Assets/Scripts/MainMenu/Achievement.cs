using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Achievement
{
    public bool IsCompleted;
    public string Name;
    public string Description;
    public int TargetValue;
    public int CurrentValue;
    public Sprite LockedIcon;
    public Sprite UnlockedIcon;
}
