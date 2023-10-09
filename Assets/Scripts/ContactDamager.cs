using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamager : MonoBehaviour
{
    public bool disactivated = false;
    public int damage = 10;
    public int Illdamage = 10;
    public int CorruptionDamage = 0;
    public bool destroyOnTouch = false;
    public LayerMask targetLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0 && !disactivated)
        {
            if (damage > 0 || Illdamage > 0 || CorruptionDamage > 0)
            {
                if (damage > 0 && collision.GetComponent<Damageable>())
                {
                    collision.GetComponent<Damageable>().TakeDamage(damage);
                }
                if (Illdamage > 0 && collision.GetComponent<MalattiaHandler>())
                {
                    collision.GetComponent<MalattiaHandler>().gainMalattia(Illdamage, false);
                }
                if (CorruptionDamage > 0 && collision.GetComponent<MalattiaHandler>())
                {
                    collision.GetComponent<MalattiaHandler>().gainCorruption(CorruptionDamage, false);
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
            if (damage > 0 || Illdamage > 0 || CorruptionDamage > 0)
            {
                if (damage > 0 && collision.gameObject.GetComponent<Damageable>())
                {
                    collision.gameObject.GetComponent<Damageable>().TakeDamage(damage);
                }
                if (Illdamage > 0 && collision.gameObject.GetComponent<MalattiaHandler>())
                {
                    collision.gameObject.GetComponent<MalattiaHandler>().gainMalattia(Illdamage, false);
                }
                if (CorruptionDamage > 0 && collision.gameObject.GetComponent<MalattiaHandler>())
                {
                    collision.gameObject.GetComponent<MalattiaHandler>().gainCorruption(CorruptionDamage, false);
                }
            }
        }
        if (destroyOnTouch)
        {
            Destroy(gameObject);
        }        
    }
}
