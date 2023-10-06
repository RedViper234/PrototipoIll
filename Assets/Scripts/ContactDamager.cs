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
            if ((damage > 0 || Illdamage > 0 || CorruptionDamage > 0) && collision.GetComponent<Damageable>())
            {
                bool damaged = collision.GetComponent<Damageable>().TakeDamage(damage);
                if (Illdamage > 0 && damaged && collision.GetComponent<MalattiaHandler>())
                {
                    collision.GetComponent<MalattiaHandler>().gainMalattia(Illdamage);
                }
                if (CorruptionDamage > 0 && damaged && collision.GetComponent<MalattiaHandler>())
                {
                    collision.GetComponent<MalattiaHandler>().modifyCorruption(CorruptionDamage);
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
            if ((damage > 0 || Illdamage > 0 || CorruptionDamage > 0) && collision.gameObject.GetComponent<Damageable>())
            {
                bool damaged = collision.gameObject.GetComponent<Damageable>().TakeDamage(damage);
                if (Illdamage > 0 && damaged && collision.gameObject.GetComponent<MalattiaHandler>())
                {
                    collision.gameObject.GetComponent<MalattiaHandler>().gainMalattia(Illdamage);
                }
                if (CorruptionDamage > 0 && damaged && collision.gameObject.GetComponent<MalattiaHandler>())
                {
                    collision.gameObject.GetComponent<MalattiaHandler>().modifyCorruption(CorruptionDamage);
                }
            }
            if (destroyOnTouch)
            {
                Destroy(gameObject);
            }
        }
    }
}
