using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject DetonationParticlesPrefab;
    public GameObject Target;
    [HideInInspector] public float Damage;
    [HideInInspector] public bool IsCrit = false;
    [HideInInspector] public float Speed;
    [HideInInspector] public AttackType TypeAttack;
    [HideInInspector] public Vector3 TargetPosition;
    [HideInInspector] public float Aoe;
    [HideInInspector] public GameMode gameMode;
    [HideInInspector] public Element damageElement;
    [HideInInspector] public Vector3 vec = new Vector3();
    [HideInInspector] public bool IsPrimary = false;
    [HideInInspector] public GameObject Debuff;
    // Use this for initialization
    void Start ()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
        transform.LookAt(Target.transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Target != null)
        {
            switch (TypeAttack)
            {
                case AttackType.Target:
                    PointAttack();
                    break;
                case AttackType.NonTarget:
                    MassiveAttack();
                    break;
                case AttackType.Chain:
                    ChainAttack();
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (TypeAttack)
            {
                case AttackType.Target:
                    if (!moveToTarget(vec))
                        Destroy(gameObject);
                    break;
                case AttackType.NonTarget:
                    MassiveAttack();
                    break;
                case AttackType.Chain:
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }

    void ChainAttack()
    {
        vec = Target.transform.position;
        if (!moveToTarget(Target.transform.position))
        {
            Target.GetComponent<UnitBase>().GetDamage(Damage, damageElement);
            GameMode.gameHUD.DamageTarget = Target;
            if(Debuff != null)
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
                            _bullet.GetComponent<Bullet>().Target = item;
                            _bullet.GetComponent<Bullet>().Speed = Speed;
                            _bullet.GetComponent<Bullet>().damageElement = damageElement;
                            _bullet.GetComponent<Bullet>().Damage = Damage / 2;
                            _bullet.GetComponent<Bullet>().TypeAttack = TypeAttack;
                            _bullet.GetComponent<Bullet>().Aoe = Aoe;
                            _bullet.GetComponent<Bullet>().IsPrimary = false;
                            _bullet.GetComponent<Bullet>().Debuff = Debuff;
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
    }
    void PointAttack()
    {
        vec = Target.transform.position;
        if (!moveToTarget(Target.transform.position))
        {
            Target.GetComponent<UnitBase>().GetDamage(Damage, damageElement);
            GameMode.gameHUD.DamageTarget = Target;
            if (Debuff != null)
                Target.GetComponent<UnitBase>().AddDebuff(Debuff);
            if (IsCrit)
                GameMode.gameHUD.showCritText();
            Destroy(gameObject);
        }
    }

    void MassiveAttack()
    {
        if (!moveToTarget(TargetPosition))
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
    bool moveToTarget(Vector3 _targetPos)
    {
        if (Vector3.Distance(transform.position, _targetPos) >= 0.05f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_targetPos - transform.position), 5 * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Speed * Time.deltaTime * GameMode.TimeSpeedMultyplier);
            return true;
        }
        else
            return false;
    }
}
