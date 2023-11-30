using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BurnStatus : MonoBehaviour, IStatus
{
    [field: SerializeField] public float effectDuration {get; set;}
    [field: SerializeField] public int ticks {get; set;}
    [field: SerializeField] public float damage {get; set;}
    [field: SerializeField] public DamageType.DamageTypes statusType { get; set; }

    void Start()
    {
        BurnDamage();
    }
    
    //TODO
    private void BurnDamage()
    {
        DamageInstance damageInstance = 
            new DamageInstance(
                statusType, 
                damage,
                false,
                false,
                true,
                effectDuration);
        
        GetComponentInParent<Damageable>().TakeDamage(damageInstance);
    }
}
