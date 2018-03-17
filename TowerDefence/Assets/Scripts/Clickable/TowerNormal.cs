using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TowerNormal : TowerBase {

    [HideInInspector] public ModificatorBase Modificator;
    override protected void Start ()
    {
        base.Start();
        Modificator = GetComponent<ModificatorBase>();
    }

    override protected void Update ()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
        {
            base.Update();
            Attack(Modificator);
        }
    }

    override protected void Attack(ModificatorBase _modificator)
    {
        if (Target != null)
        {
            if (Reloads >= CurAttackSpeed)
            {
                Vector3 _vec = new Vector3(transform.position.x, Target.transform.position.y, transform.position.z);
                if (Vector3.Distance(_vec, Target.transform.position) <= CurRange)
                {
                    bullet = Instantiate(BulletPrefab, BulletSpawnPoint.transform.position, Quaternion.identity);
                    BulletBase _bullet = bullet.GetComponent<BulletBase>();
                    _bullet.Target = Target;
                    _bullet.TargetPosition = Target.transform.position;
                    _bullet.Speed = BulletSpeed;
                    _bullet.damageElement = TypeElement;
                    _bullet.Damage = CurDamage;
                    _bullet.Aoe = AoeRange;
                    Reloads = 0.0f;
                    if(_modificator.Type == ModificatorType.ImmediateModificator)
                    {
                        bool _b = (_modificator as ModificatorImmediate).GetCritical();
                        _bullet.IsCrit = _b;
                        _bullet.Damage = CurDamage * (_modificator as ModificatorImmediate).CriticalPower;
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

    private void OnMouseDown()
    {
        if (!gameMode.gameOver && !gameMode.victory && !gameMode.pause)
            if (!EventSystem.current.IsPointerOverGameObject())
                OnClick();
    }

    override protected void OnClick()
    {
        base.OnClick();
        GameMode.gameHUD.ShowDescriptionTower(Name, CurDamage, UpgradeCostOutput, CurSellCost, Icon);
    }
}
