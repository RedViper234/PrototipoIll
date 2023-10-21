using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AAttack : MonoBehaviour, IAttack
{
    public abstract AttackSO attackSO { get; set;}
    public abstract float TimeToActivateHitbox { get; set;}
    public abstract float TimeDurationHitbox { get; set;}
    public abstract float TimeToEndHitbox { get; set;}
    public abstract float PlayerSpeedModifier { get; set;}
    public abstract PlayerDragStruct PlayerDrag { get; set;}
    public abstract float AttackCooldown { get; set;}
    public abstract float BaseDamageAttack { get; set;}
    public abstract AttackRange AttackRangeAttack { get; set;}
    public abstract DamageType.DamageTypes DamageType { get; set;}
    public abstract List<StatusStruct> StatusEffects { get; set;}
    public abstract float KnockbackForceAttack { get; set;}
    public abstract MultiAttack MultiAttack { get; set; }

    /// <summary>
    /// Initializes the attack values based on the provided AttackSO.
    /// </summary>
    /// <param name="attackSO">The AttackSO containing the attack values.</param>
    public virtual void InitAttackValues(AttackSO attackSO)
    {
        // Set the attack values based on the AttackSO
        TimeToActivateHitbox = attackSO.TimeToActivateHitbox;
        TimeDurationHitbox = attackSO.TimeDurationHitbox;
        TimeToEndHitbox = attackSO.TimeToEndHitbox;
        PlayerSpeedModifier = attackSO.PlayerSpeedModifier;
        PlayerDrag = attackSO.PlayerDrag;
        AttackCooldown = attackSO.AttackCooldown;
        BaseDamageAttack = attackSO.BaseDamageAttack;
        AttackRangeAttack = attackSO.AttackRangeAttack;
        DamageType = attackSO.DamageType;
        StatusEffects = attackSO.StatusEffects;
        KnockbackForceAttack = attackSO.KnockbackForceAttack;

        // Start the coroutine to initialize the attack
        StartCoroutine(InitializeAttack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Damageable>())
        {
            other.GetComponent<Damageable>().TakeDamage(BaseDamageAttack == 0 ? GetComponentInParent<IWeapon>().BaseDamageWeapon: BaseDamageAttack , false, false);
            //TODO Add knockback
        }
    }

    public virtual IEnumerator InitializeAttack()
    {
        yield return new WaitForSeconds(TimeToActivateHitbox);

        //Activate Hitbox
        // Debug.Log("Attivato");
        DoInTimeToActivateHitbox();
        GetComponent<BoxCollider2D>().enabled = true;

        yield return new WaitForSeconds(TimeDurationHitbox);

        //Deactivate Hitbox
        // Debug.Log("Disattivato");
        DoInTimeDurationHitbox();
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(TimeToEndHitbox);

        //End of attack
        DoInTheEnd();
        // Destroy(this.gameObject);
        // Debug.Log("Fine");
    }

    public virtual void DoInTimeToActivateHitbox()
    {
        // throw new NotImplementedException();
    }

    public virtual void DoInTimeDurationHitbox()
    {
        // throw new NotImplementedException();
    }

    public virtual void DoInTheEnd()
    {
        // throw new NotImplementedException();
    }
}

[Serializable]
public struct MultiAttack
{
    public float NumberOfAttacks;
    public float TimeBetweenAttacks;
}
