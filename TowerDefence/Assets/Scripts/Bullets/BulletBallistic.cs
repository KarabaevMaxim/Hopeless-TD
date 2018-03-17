using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBallistic : BulletBase {

    override protected void Start()
    {
        base.Start();
    }

    void Update ()
    {
		if(!moveToTarget(TargetPosition))
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
    
}
