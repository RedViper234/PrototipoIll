using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangedVariant : AttackRanged
{
    protected override void StartBulletBehavior()
    {
        base.StartBulletBehavior();
    }

    public override void DoAfterWaitHitboxActivation()
    {
        base.DoAfterWaitHitboxActivation();
        Debug.Log("VARIANT BULLET");
    }
}
