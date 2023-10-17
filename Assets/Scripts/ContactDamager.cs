using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    public bool disactivated = false;
    public List<DamageInstance> damageInstance;
    public bool destroyOnTouch = false;
    public LayerMask targetLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0 && !disactivated)
        {

            if (collision.GetComponent<PlayerController>())
            {
                collision.GetComponent<PlayerController>().PlayerTakeDamage(DamageInstance.removeZeroDamageInstance(damageInstance));
            }
            else
            {
                foreach (var dmg in DamageInstance.removeZeroDamageInstance(damageInstance))
                {
                    if (collision.GetComponent<Damageable>() && !dmg.damageOverTime && dmg.value > 0)
                    {
                        collision.GetComponent<Damageable>().TakeDamage(dmg.value, dmg.ignoreImmunity, dmg.type==DamageType.DamageTypes.Ustioni);
                    }
                    else if (collision.GetComponent<Damageable>() && dmg.damageOverTime && dmg.value > 0)
                    {
                        collision.GetComponent<Damageable>().TakeDamageOverTime(dmg.value, dmg.type, dmg.durationDamageOverTime, dmg.type == DamageType.DamageTypes.Ustioni);
                    }
                }
            }
            if (destroyOnTouch)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0 && !disactivated)
        {
            List<DamageInstance> instanceOfDamageCopy = damageInstance.FindAll(f => f.value > 0);
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(DamageInstance.removeZeroDamageInstance(damageInstance));
            }
            else
            {
                foreach (var dmg in DamageInstance.removeZeroDamageInstance(damageInstance))
                {
                    if (collision.gameObject.GetComponent<Damageable>() && !dmg.damageOverTime && dmg.value > 0)
                    {
                        collision.gameObject.GetComponent<Damageable>().TakeDamage(dmg.value, dmg.ignoreImmunity, dmg.type == DamageType.DamageTypes.Ustioni);
                    }
                    else if (collision.gameObject.GetComponent<Damageable>() && dmg.damageOverTime && dmg.value > 0)
                    {
                        collision.gameObject.GetComponent<Damageable>().TakeDamageOverTime(dmg.value, dmg.type, dmg.durationDamageOverTime, dmg.type == DamageType.DamageTypes.Ustioni);
                    }
                }
            }
            if (destroyOnTouch)
            {
                Destroy(gameObject);
            }
        }
        if (destroyOnTouch)
        {
            Destroy(gameObject);
        }        
    }
}
