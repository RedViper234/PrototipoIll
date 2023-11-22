using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BurnStatus : MonoBehaviour, IStatus
{
    [field: SerializeField] public float effectDuration {get; set;}
    [field: SerializeField] public int ticks {get; set;}
    [field: SerializeField] public float damage {get; set;}

    private void BurnDamage()
    {
        DamageInstance damageInstance = 
            new DamageInstance(
                DamageType.DamageTypes.Fisico, 
                damage, 
                false,
                false,
                false,
                0);
        
        GetComponentInParent<Damageable>().TakeDamage(damageInstance);
    }
}
