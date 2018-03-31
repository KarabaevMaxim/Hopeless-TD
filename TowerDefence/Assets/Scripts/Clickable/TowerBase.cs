using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TowerBase : ClickableObject
{
    public List<TowerStats> TowerStats = new List<TowerStats>();
    [HideInInspector] public int MaxLevel = 3;
    public float BulletSpeed;
    public Element TypeElement;
    public AttackType TypeAttack;
    public float RotationSpeed = 0.0f;
    public int StartLevel;

    protected int CurLevel;
    protected int CurCost;
    protected float CurDamage;
    [HideInInspector] public float CurRange;
    protected float CurAttackSpeed;
    protected int CurSellCost;
    protected float AoeRange;
    protected GameObject CurTowerUpper;
    protected GameObject CurTowerBottom;

    public GameObject BulletPrefab;
    [HideInInspector] public Transform BulletSpawnPoint;
    public GameObject AttackRadiusPrefab;
    public Transform AttackRadiusPos;
    
    Vector3 StartScaleRadius;
    //[HideInInspector]
    public GameObject TowerFoundation;
    [SerializeField] private Transform TowerUpperPosition;
    //public Transform TowerUpper; // вращающаяся часть башни

     [HideInInspector] public GameObject Target;

    protected float Reloads;
    protected GameObject bullet;

    protected string UpgradeCostOutput;
    public bool CanAttackFlyingUnits;

    public GameObject IncomeTextPrefab;
    protected ResourceTowerAnimController animController;
    override protected void Start()
    {
        base.Start();
        CurLevel = StartLevel;
        UpdateStats();
        StartScaleRadius = new Vector3(0.2f, 1f, .2f);
        Reloads = CurAttackSpeed;
        
        //основание башни установить в зависимости от лвл башни
        //орудие башни установить в зависимости от лвл башни
    }

    override protected void Update()
    {
        base.Update();
        GetTarget();
    }

    override protected void OnClick()
    {
        base.OnClick();
        if (TypeAttack != AttackType.Income)
        {
            AttackRadiusObject = Instantiate(AttackRadiusPrefab, AttackRadiusPos, false);
            UpdateRadius();
        }
        UpgradeCostOutput = CurLevel + 1 < TowerStats.Count ? TowerStats[CurLevel + 1].Cost.ToString() : "Макс.";
    }
    void UpdateRadius()
    {
        if(CurRange != 0)
            AttackRadiusObject.transform.localScale = new Vector3((StartScaleRadius.x * CurRange), 1, 
                                                    (StartScaleRadius.z * CurRange));
    }

    void GetTarget()
    {
        if (Target == null)
        {
            foreach (var item in gameMode.GameUnits)
            {
                if (item.GetComponent<UnitBase>().Flying && CanAttackFlyingUnits || !item.GetComponent<UnitBase>().Flying)
                {
                    Vector3 _vec = new Vector3(transform.position.x, item.transform.position.y, transform.position.z);
                    if (Vector3.Distance(_vec, item.transform.position) <= CurRange)
                        Target = item;
                }
            }
        }
        else
        {
            if (RotationSpeed > 0)
            {
                Transform _trans = CurTowerUpper.transform;
                _trans.rotation = Quaternion.Slerp(_trans.rotation, Quaternion.LookRotation(new Vector3(Target.transform.position.x, 0,
                                                    Target.transform.position.z) - new Vector3(_trans.position.x, 0, _trans.position.z)), 
                                                    RotationSpeed * Time.deltaTime * GameMode.TimeSpeedMultyplier);
                _trans = null;
                //Vector3 _relativePos = Target.transform.position - _trans.position;
                //Quaternion rot = Quaternion.LookRotation(_relativePos);
                //if()


            }
        }
        //if (GameMode.PriorityTarget != null)
        //{
        //    if (GameMode.PriorityTarget.GetComponent<UnitBase>().Flying && CanAttackFlyingUnits)
        //    {
        //        Vector3 _vec1 = new Vector3(transform.position.x, GameMode.PriorityTarget.transform.position.y, transform.position.z);
        //        if (Vector3.Distance(_vec1, GameMode.PriorityTarget.transform.position) <= CurRange)
        //            Target = GameMode.PriorityTarget;
        //    }
        //}
    }

    public void Sell()
    {
        TowerFoundation.SetActive(true);
        gameMode.Diamonds += CurSellCost;
        gameMode.GameTowers.Remove(gameObject);
        Destroy(gameObject);
    }

    public void UpdateStats()
    {
        CurCost = TowerStats[CurLevel].Cost;
        CurDamage = TowerStats[CurLevel].Damage;
        CurRange = TowerStats[CurLevel].Range;
        CurAttackSpeed = TowerStats[CurLevel].atkSpeed;
        AoeRange = TowerStats[CurLevel].Aoe;
        CurSellCost = TowerStats[CurLevel].SellCost;
        MaxLevel = TowerStats.Count - 1;
        Quaternion _rot = Quaternion.identity;
        if (CurTowerUpper != null)
        {
            _rot = CurTowerUpper.transform.localRotation;
            DestroyImmediate(CurTowerUpper);
            BulletSpawnPoint = null;
        }
        
        CurTowerUpper = Instantiate(TowerStats[CurLevel].TowerUpper, TowerUpperPosition.position, _rot, transform);
        if (TypeAttack != AttackType.Income)
        {
            BulletSpawnPoint = GetComponentInChildren<TowerUpper>().BltSpawnPoint;
            if (CurTowerBottom != null)
                Destroy(CurTowerBottom);
            CurTowerBottom = Instantiate(TowerStats[CurLevel].TowerBottom, transform.position, Quaternion.identity, transform);
        }
        else
        {
            animController = GetComponentInChildren<ResourceTowerAnimController>();
            animController.IncomeValue = (int)CurDamage;
            animController.IncomeTextPrefab = IncomeTextPrefab;
        }

    }

    virtual protected void Attack(ModificatorBase _modificator) // вызывать в дочерних классах
    {
        if (Target != null)
        {
            if (Reloads >= CurAttackSpeed)
            {
                Vector3 _vec = new Vector3(transform.position.x, Target.transform.position.y, transform.position.z);
                if (Vector3.Distance(_vec, Target.transform.position) <= CurRange && !Target.GetComponent<UnitBase>().IsDead)
                {
                    if (BulletSpawnPoint != null)
                    {
                        bullet = Instantiate(BulletPrefab, BulletSpawnPoint.position, Quaternion.identity);
                        BulletBase _bullet = bullet.GetComponent<BulletBase>();
                        _bullet.Target = Target;
                        _bullet.TargetPosition = Target.transform.position;
                        _bullet.Speed = BulletSpeed;
                        _bullet.damageElement = TypeElement;
                        _bullet.Damage = CurDamage;
                        _bullet.Aoe = AoeRange;
                        _bullet.IsPrimary = true;
                        if (_modificator.Type == ModificatorType.LastingModificator)
                            _bullet.Debuff = (_modificator as ModificatorLasting).DebuffPrefab;
                        Reloads = 0.0f;
                    }
                    
                }
                else
                    Target = null;
            }
            else
            {
                Reloads += Time.deltaTime * GameMode.TimeSpeedMultyplier;
            }
        }
    }

    public void Upgrade()
    {
        if (CurLevel < MaxLevel)
        {
            if (gameMode.Diamonds >= CurCost)
            {
                gameMode.Diamonds -= CurCost;
                CurLevel++;
                UpdateStats();
                UpdateRadius();
                UpgradeCostOutput = CurLevel < MaxLevel ? TowerStats[CurLevel + 1].Cost.ToString() : "Макс.";
                GameMode.gameHUD.ShowDescriptionTower(Name, CurDamage, UpgradeCostOutput, CurSellCost, Icon);
            }
            else
                GameMode.gameHUD.showNotEnoughResoursesPanel();
        }
    }
}
