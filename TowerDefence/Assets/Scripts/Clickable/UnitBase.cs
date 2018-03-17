using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

[Serializable]
public class UnitBase : ClickableObject
{
    [Header("Характеристики")]
    public float BaseHealth;
    public float BaseArmor;
    public float BaseSpeed;
    public int BaseReward;

    [HideInInspector] public float StartHealth;
    [HideInInspector] public float StartArmor;
    [HideInInspector] public float StartSpeed;
    [HideInInspector] public int StartReward;

    [HideInInspector] public float CurHealth;
    [HideInInspector] public float CurArmor;
    [HideInInspector] public float CurSpeed;
    [HideInInspector] public int CurReward;

    [HideInInspector] public float Resistance;
    public Element TypeElement;
    public float MultiplierResist = 0.5f;
    [HideInInspector] public float DistanceToFinish;

    [Header("Кружок приоритетной цели")]
    public GameObject PriorityTargetSelectPrefab;
    public Transform PriorityTargetSelectPos;
    static public GameObject PriorityTargetSelectObject;
    private int stage;

    static private float clickTime;

    [Header("Полоса здоровья")]
    public GameObject HealthBarPrefab;
    GameObject HealthBarParentObject;
    Image HealthBarChildObject;

    public Color StartHealthBarColor;
    Color CurHealthBarColor;
    public bool Flying;
    public float HeightSpawn;

    public List<GameObject> Debuffs = new List<GameObject>();
    private Animator animator;
    public bool IsDead = false;
    [SerializeField] private float timeToDestroyBody; // время до исчезновения трупа
    private float timerToDestroyBody; // таймер
    [SerializeField] private Transform debuffPos;
    override protected void Start()
    {
        base.Start();
        StartHealth =           BaseHealth * GameMode.DifficultyMultyplier;
        CurHealth =             StartHealth;

        StartArmor =            BaseArmor * GameMode.DifficultyMultyplier;

        ChangeArmor(StartArmor);

        StartSpeed =            BaseSpeed * GameMode.DifficultyMultyplier * GameMode.TimeSpeedMultyplier;
        CurSpeed =              StartSpeed;
        

        float _multiplier =     GameMode.DifficultyMultyplier >= 2.0f ? 0.05f : 1.0f;
        StartReward =           Mathf.RoundToInt(BaseReward * GameMode.DifficultyMultyplier * _multiplier);
        CurReward =             StartReward;
        IsDead =                false;
        stage =                 1;
        clickTime =             Time.time;
        timerToDestroyBody =    0;


        HealthBarParentObject = Instantiate(HealthBarPrefab, GameMode.gameHUD.DynamicObjectsParent);
        HealthBarChildObject =  HealthBarParentObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        HealthBarParentObject.SetActive(false);
        Vector3 _vec = gameMode.Areas[1].transform.position;
        transform.LookAt(new Vector3(_vec.x, transform.position.y, _vec.z));
        _vec = Vector3.zero;
        animator = GetComponent<Animator>();
        animator.SetFloat("Speed", CurSpeed);
        RestoreHealthBarColor();
    }

    public void AddDebuff(GameObject _debuff)
    {
        DebuffTowerActiveBase _dtab = _debuff.GetComponent<DebuffTowerActiveBase>();
        GameObject _go = Debuffs.Where(d => d.GetComponent<DebuffTowerActiveBase>().ID ==
                                        _dtab.ID).FirstOrDefault();
        if (_go == null)
        {
            Debuffs.Add(Instantiate(_debuff, debuffPos.position, Quaternion.identity, transform));
            ChangeHeathBarColor(_dtab.HealthBarColor);
        }
        else
        {
            _go.GetComponent<DebuffTowerActiveBase>().DebuffTime = 0;
        }
    }
    public void RemoveDebuff(int _id)
    {
        GameObject _go = Debuffs.Where(d => d.GetComponent<DebuffTowerActiveBase>().ID == _id).FirstOrDefault();
        
        Debuffs.Remove(_go);
        if (Debuffs.Count > 0)
            ChangeHeathBarColor(Debuffs.Last().GetComponent<DebuffTowerActiveBase>().HealthBarColor);
        else
            RestoreHealthBarColor();
    }

    override protected void Update()
    {
        base.Update();
        if (!IsDead)
        {
            Move();
            ShowHealthBar();
        }
        else
        {
            if (timerToDestroyBody >= timeToDestroyBody)
                Destroy(gameObject);
            else
                timerToDestroyBody += Time.deltaTime * GameMode.TimeSpeedMultyplier;
            // Запустить счетчик до исчезновения
        }
    }
    public void RestoreSpeed()
    {
        CurSpeed = StartSpeed;
        animator.SetFloat("Speed", CurSpeed);
    }
    public void Slow(float _percent)
    {
        CurSpeed = StartSpeed;
        CurSpeed *=  (1 - _percent);
        animator.SetFloat("Speed", CurSpeed);
    }

    
    public void DecreaseArmor(float _decreasePercent)
    {
        ChangeArmor(StartArmor);
        ChangeArmor(CurArmor * _decreasePercent);
    }
    public void RestoreArmor()
    {
        ChangeArmor(StartArmor);
    }
    public void ChangeArmor(float _armor)
    {
        CurArmor = _armor;
        Resistance = (0.06f * CurArmor) / (1 + 0.06f * CurArmor);
    } 

    public void RestoreStats()
    {
        CurSpeed = StartSpeed;
        ChangeArmor(StartArmor);
        ChangeHeathBarColor(StartHealthBarColor);
    }
    public void GetDamage(float _damage, Element _damageElement)
    {
        if (_damageElement != Element.Normal)
        {
            if (TypeElement == _damageElement)
                CurHealth -= _damage * (1 - Resistance);
            else
            {
                if (TypeElement > _damageElement) // уточнить для последнего и первого элемента
                    CurHealth -= _damage * (1 - Resistance * (1 + MultiplierResist));
                else
                    CurHealth -= _damage * (1 - Resistance * (1 - MultiplierResist));
            }
        }
        else
        {
            CurHealth -= _damage * (1 - Resistance);
        }
        if (CurHealth < 0)
            Death();
        
        if(GameMode.gameHUD.targetSelect == gameObject)
            GameMode.gameHUD.ShowDescriptionUnit(Name, CurArmor, Resistance, CurHealth, StartHealth, Icon);
    }

    public void PropertyChanged() // вызывать этот метод при каждом изменении характеристик
    {
        GameMode.gameHUD.ShowDescriptionUnit(Name, CurArmor, Resistance, CurHealth, StartHealth, Icon);
    }

    void Move()
    {
        if (stage < gameMode.Areas.Count)
        {
            Vector3 _vec = gameMode.Areas[stage].transform.position;
            DistanceToFinish = Vector3.Distance(transform.position, 
                new Vector3(_vec.x, transform.position.y, _vec.z)); // касяк, тут дистанция не до финиша

            if (DistanceToFinish > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_vec.x, _vec.y + HeightSpawn, _vec.z),
                                                        CurSpeed * Time.deltaTime * GameMode.TimeSpeedMultyplier);
            }
            else
            {
                stage++;
                if (stage < gameMode.Areas.Count)      
                    transform.LookAt(gameMode.Areas[stage].transform);
                
            }
        }
        else
        {
            Finish();
        }
    }

    void Death()
    {
        if (!IsDead)
        {
            gameMode.Diamonds += CurReward;
            gameMode.GameUnits.Remove(gameObject);
            Destroy(HealthBarParentObject);
            animator.SetBool("Death", true);
            IsDead = true;
            Destroy(GetComponent<BoxCollider>());
            GameMode.gameHUD.CloseDescriptionUnit();
            if(GameMode.gameHUD.targetSelect == gameObject)
                GameMode.gameHUD.targetSelect = null;
            if (PriorityTargetSelectObject != null)
            {
                Destroy(PriorityTargetSelectObject);
                PriorityTargetSelectObject = null;
            }
            if (CircleSelectObject != null)
                Destroy(CircleSelectObject);
        }
    }
    void Finish()
    {
        gameMode.LosesUnits++;
        gameMode.GameUnits.Remove(gameObject);
        Destroy(HealthBarParentObject);
        HealthBarParentObject = null;
        Destroy(gameObject);
    }

    protected override void OnClick()
    {
        base.OnClick();
        GameMode.gameHUD.ShowDescriptionUnit(Name, CurArmor, Resistance, CurHealth, StartHealth, Icon);
        if ((Time.time - clickTime) < 0.5f)
        {
            if (PriorityTargetSelectObject != null)
                Destroy(PriorityTargetSelectObject);
            PriorityTargetSelectObject = Instantiate(PriorityTargetSelectPrefab, PriorityTargetSelectPos, false);
            foreach (var item in gameMode.GameTowers)
            {
                if (item.GetComponent<TowerBase>().CanAttackFlyingUnits && Flying || !Flying)
                {
                    Vector3 _vec = new Vector3(item.transform.position.x, transform.position.y, item.transform.position.z);
                    if (Vector3.Distance(_vec, transform.position) <= item.GetComponent<TowerBase>().CurRange)
                        item.GetComponent<TowerBase>().Target = gameObject;
                }
                    
            }
        }
        clickTime = Time.time;
    }

    void ShowHealthBar()
    {
        if (HealthBarParentObject != null)
        {
            HealthBarParentObject.SetActive(true);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            HealthBarParentObject.GetComponent<RectTransform>().position = new Vector3(screenPos.x, screenPos.y + 30, 0);
            HealthBarChildObject.fillAmount = CurHealth / StartHealth;
        }
    }

    public void ChangeHeathBarColor(Color _color)
    {
        if (HealthBarParentObject != null)
            HealthBarChildObject.color = _color;
    }

    public void RestoreHealthBarColor()
    {
        if (HealthBarChildObject != null)
        {
            if (Debuffs.Count == 0)
                HealthBarChildObject.color = StartHealthBarColor;
            else
                HealthBarChildObject.color = Debuffs.Last().GetComponent<DebuffTowerActiveBase>().HealthBarColor;
        }
    }
}
