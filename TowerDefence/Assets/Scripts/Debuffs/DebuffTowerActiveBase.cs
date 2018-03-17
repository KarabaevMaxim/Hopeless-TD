using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTowerActiveBase : MonoBehaviour {

    [Header("Общие характеристики дебафа")]
    public int ID = -1;
    protected GameObject Parent;
    protected UnitBase Unit;
    public string Name;
    public float Duration;
    public Element TypeElement;
    [HideInInspector] public float DebuffTime;
    public Color HealthBarColor;
    virtual protected void Start ()
    {
        Parent = transform.parent.gameObject;
        Unit = Parent.GetComponent<UnitBase>();
        DebuffTime = 0;
    }

    virtual protected void Update ()
    {
		
	}

    private void OnDestroy()
    {
        if (Unit != null)
            Unit.RemoveDebuff(ID);
    }
}
