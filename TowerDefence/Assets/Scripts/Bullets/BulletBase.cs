using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    public GameObject DetonationParticlesPrefab;
    [HideInInspector] public GameObject Target;
    [HideInInspector] public float Damage;
    [HideInInspector] public float Speed;
    [HideInInspector] public bool IsCrit = false;
    [HideInInspector] public Vector3 TargetPosition;
    [HideInInspector] public float Aoe;
    [HideInInspector] public GameMode gameMode;
    [HideInInspector] public Element damageElement;
    [HideInInspector] public Vector3 vec = new Vector3();
    [HideInInspector] public bool IsPrimary = false;
    [HideInInspector] public GameObject Debuff;
    protected Vector3 StartPosition;
    protected Vector3 MiddlePosition;
    public float MiddlePointY;
    virtual protected void Start()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_targetPos - transform.position), 5 * Time.deltaTime);

        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        transform.LookAt(Target.transform.position);
        StartPosition = transform.position;
        MiddlePosition = new Vector3((StartPosition.x + TargetPosition.x) / 2, StartPosition.y + MiddlePointY,
                                        (StartPosition.z + TargetPosition.z) / 2);
    }


    void Update()
    {
        
    }

    float count = 0;
    Vector3 CurrentPosition(float _t, Vector3 _pos)
    {
        //return new Vector3(StartPosition.x * Mathf.Pow(1 - _t, 2) + 2 * MiddlePosition.x * (1 - _t) * _t + TargetPosition.x * Mathf.Pow(_t, 2),
        //                   StartPosition.y * Mathf.Pow(1 - _t, 2) + 2 * MiddlePosition.y * (1 - _t) * _t + TargetPosition.y * Mathf.Pow(_t, 2),
        //                   StartPosition.z * Mathf.Pow(1 - _t, 2) + 2 * MiddlePosition.z * (1 - _t) * _t + TargetPosition.z * Mathf.Pow(_t, 2));
        return new Vector3(StartPosition.x * Mathf.Pow(1 - _t, 2) + 2 * MiddlePosition.x * (1 - _t) * _t + _pos.x * Mathf.Pow(_t, 2),
                          StartPosition.y * Mathf.Pow(1 - _t, 2) + 2 * MiddlePosition.y * (1 - _t) * _t + _pos.y * Mathf.Pow(_t, 2),
                          StartPosition.z * Mathf.Pow(1 - _t, 2) + 2 * MiddlePosition.z * (1 - _t) * _t + _pos.z * Mathf.Pow(_t, 2));
    }

    protected bool moveToTarget(Vector3 _targetPos)
    {
        if (Vector3.Distance(transform.position, _targetPos) >= 0.5f)
        {
            count += Speed;
            transform.position = CurrentPosition(count, _targetPos);
            return true;
        }
        else
            return false;
    }

    protected void PointAttack()
    {
        vec = Target.transform.position;
        Target.GetComponent<UnitBase>().GetDamage(Damage, damageElement);
        GameMode.gameHUD.DamageTarget = Target;
        if (Debuff != null)
            Target.GetComponent<UnitBase>().AddDebuff(Debuff);
        if (IsCrit)
            GameMode.gameHUD.showCritText();
        Destroy(gameObject);
    }
    protected void ChainAttack()
    {
        vec = Target.transform.position;
        Target.GetComponent<UnitBase>().GetDamage(Damage, damageElement);
        GameMode.gameHUD.DamageTarget = Target;
        if (Debuff != null)
            Target.GetComponent<UnitBase>().AddDebuff(Debuff);
        if (IsPrimary)
        {
            foreach (var item in gameMode.GameUnits)
            {
                if (item != Target)
                {
                    if (Vector3.Distance(item.transform.position, Target.transform.position) <= Aoe)
                    {
                        GameObject _bullet = Instantiate(gameObject, transform.position, Quaternion.identity);
                        
                        BulletChain _bull = _bullet.GetComponent<BulletChain>();
                        _bull.Target = item;
                        _bull.Speed = Speed;
                        _bull.damageElement = damageElement;
                        _bull.Damage = Damage / 2;
                        _bull.Aoe = 0;
                        _bull.IsPrimary = false;
                        _bull.Debuff = Debuff;
                       // _bull.MiddlePointY = 0.01f;
                    }
                }
            }
        }
        
        Destroy(gameObject);
    }
    protected void MassiveAttack()
    {
        Instantiate(DetonationParticlesPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
        foreach (var item in gameMode.GameUnits)
        {
            if (Vector3.Distance(item.transform.position, TargetPosition) <= Aoe)
            {
                item.GetComponent<UnitBase>().GetDamage(Damage, damageElement);
                if (Debuff != null)
                    item.GetComponent<UnitBase>().AddDebuff(Debuff);
            }
        }
        Destroy(gameObject);
    }
}
