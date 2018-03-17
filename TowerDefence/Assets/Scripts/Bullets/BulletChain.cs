using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletChain : BulletBase
{
    override protected void Start()
    {
        base.Start();
    }

    void Update ()
    {
        if (Target != null)
        {
            if (!moveToTarget(Target.transform.position))
                ChainAttack();
        }
        else
            Destroy(gameObject);  
    }
}
