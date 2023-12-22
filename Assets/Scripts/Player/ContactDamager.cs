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
                if (collision.GetComponent<Damageable>())
                {
                    collision.GetComponent<Damageable>().TakeDamage(damageInstance);
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
            List<DamageInstance> instanceOfDamageCopy = damageInstance.FindAll(f => f.damageValueAtkOrSec > 0);
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                collision.gameObject.GetComponent<PlayerController>().PlayerTakeDamage(DamageInstance.removeZeroDamageInstance(damageInstance));
            }
            else
            {
                if (collision.gameObject.GetComponent<Damageable>())
                {
                    collision.gameObject.GetComponent<Damageable>().TakeDamage(damageInstance);
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
