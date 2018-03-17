using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSimple : BulletBase
{
	override protected void Start ()
    {
        base.Start();
	}


	void Update ()
    {
        if (Target != null)
        {
            if (!moveToTarget(Target.transform.position))
            {
                PointAttack();
            }
        }
        else
        {
            if (!moveToTarget(vec))
            {
                Destroy(gameObject);
            }
        }
	}

    
}
