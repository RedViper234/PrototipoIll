using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanged : AAttack
{
    protected virtual void StartBulletBehavior()
    {
        // Debug.Log($"Shoot: {BulletSpeed}");
        GetComponent<Rigidbody2D>().velocity = ActualDirection * BulletSpeed;
    }

    public override void DoAfterWaitHitboxActivation()
    {
        base.DoAfterWaitHitboxActivation();
        StartBulletBehavior();
    }
}