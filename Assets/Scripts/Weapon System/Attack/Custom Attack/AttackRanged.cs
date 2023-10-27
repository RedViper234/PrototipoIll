using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRanged : AAttack
{
    void Awake()
    {
       
    }

    private void Shoot()
    {
        Debug.Log($"Shoot: {BulletSpeed}");
        GetComponent<Rigidbody2D>().velocity = ActualDirection * BulletSpeed;
    }

    public override void DoInTimeDurationHitbox()
    {
        base.DoInTimeDurationHitbox();
        Shoot();
    }
}
