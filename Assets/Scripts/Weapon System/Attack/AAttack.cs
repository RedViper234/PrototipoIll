using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AAttack : MonoBehaviour, IAttack
{
    // [field: SerializeField] public AttackSO attackSO { get; set;}
    [field: SerializeField] public AnimationClip attackAnimation { get; set;}
    [field: SerializeField] public float TimeToActivateHitbox { get; set;}
    [field: SerializeField] public float TimeDurationHitbox { get; set;}
    [field: SerializeField] public float TimeToEndHitbox { get; set;}
    [field: SerializeField] public float PlayerSpeedModifier { get; set;}
    [field: SerializeField] public PlayerDragStruct PlayerDrag { get; set;}
    [field: SerializeField] public float AttackCooldown { get; set;}
    [field: SerializeField] public float BaseDamageAttack { get; set;}
    [field: SerializeField] public AttackRange AttackRangeAttack { get; set;}
    [field: SerializeField] public DamageType.DamageTypes DamageType { get; set;}
    [field: SerializeField] public List<StatusStruct> StatusEffects { get; set;}
    [field: SerializeField] public float KnockbackForceAttack { get; set;}
    [field: SerializeField] public MultiAttack MultiAttack { get; set; }
    public BoxCollider2D boxCollider2D { get; set; }

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
        boxCollider2D = GetComponent<BoxCollider2D>();

        // Start the coroutine to initialize the attack
        StartCoroutine(InitializeAttack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit");
        OnDamageableHit(other);
    }

    public virtual IEnumerator InitializeAttack()
    {
        //Prima che venga attivata l'hitbox    

        yield return new WaitForSeconds(TimeToActivateHitbox);

        //Activate Hitbox
        
        DoInTimeToActivateHitbox();

        Debug.Log("Attivato");

        yield return new WaitForSeconds(TimeDurationHitbox);

        //Deactivate Hitbox
        
        DoInTimeDurationHitbox();

        Debug.Log("Disattivato");

        yield return new WaitForSeconds(TimeToEndHitbox);

        //End of attack

        DoInTheEnd();

        Debug.Log("Fine");
    }

    public virtual void DoInTimeToActivateHitbox()
    {
        boxCollider2D.enabled = true;
    }

    public virtual void DoInTimeDurationHitbox()
    {
        boxCollider2D.enabled = false;
    }

    public virtual void DoInTheEnd()
    {
        // throw new NotImplementedException();
        Destroy(this.gameObject);
    }

    public void OnDamageableHit(Collider2D other)
    {
        DamageInstance damageInstance = new DamageInstance(
        DamageType,
        BaseDamageAttack,
        false,
        false,
        false,
        0);

        if(other.GetComponent<Damageable>())
        {
            other.GetComponent<Damageable>().TakeDamage(damageInstance);
            //TODO Add knockback
        }
    }

    public void WeaponAnimationStart()
    {
        gameObject.GetComponentInParent<IWeapon>().animator.Play(attackAnimation.name);
    }
}

[Serializable]
public struct MultiAttack
{
    public float NumberOfAttacks;
    public float TimeBetweenAttacks;
}
