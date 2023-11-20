using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BurnStatus : MonoBehaviour
{
    [SerializeField, MyReadOnly] private Damageable damageable; 
    public float timeToDie = 5f;
    public int ticks = 5;
    public float damage = 10f;

    void Update()
    {

    }

    private void BurnDamage()
    {
        DamageInstance damageInstance = 
            new DamageInstance(
                DamageType.DamageTypes.Ustioni, 
                damage, 
                false,
                false,
                false,
                0);
        
        damageable.TakeDamage(damageInstance);
    }
}
