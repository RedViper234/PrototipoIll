using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[Serializable]
public struct MultiAttack
{
    public float NumberOfAttacks;
    public float TimeBetweenAttacks;
    public AttackSO[] AttackList;
}

public abstract class AAttack : MonoBehaviour, IAttack
{
    // [field: SerializeField] public AttackSO attackSO { get; set;}
    [field: SerializeField] public AnimationClip attackAnimation { get; set;}
    
    [field: Header("Attack Time Values")]
    [field: SerializeField] public float TimeToActivateHitbox { get; set;}
    [field: SerializeField] public float TimeDurationHitbox { get; set;}
    [field: SerializeField] public float TimeToEndHitbox { get; set;}
    [field: SerializeField] public float TimeComboProgression { get; set;}
    [field: SerializeField] public float AttackCooldown { get; set;}

    [field: Header("Attack Time Values")]
    [field: SerializeField] public float PlayerSpeedModifier { get; set;}
    [field: SerializeField] public PlayerDragStruct PlayerDrag { get; set;}
    [field: SerializeField] public float BaseDamageAttack { get; set;}
    [field: SerializeField] public AttackRange AttackRangeAttack { get; set;}
    [field: SerializeField] public DamageType.DamageTypes DamageType { get; set;}
    [field: SerializeField] public List<StatusStruct> StatusEffects { get; set;}
    [field: SerializeField] public float KnockbackForceAttack { get; set;}
    [field: SerializeField] public MultiAttack MultiAttack { get; set; }
    [field: SerializeField] public DamageInstance damageInstance { get; set; }

    [field: Header("Ranged Attack Values")]
    [field: SerializeField] public float BulletSpeed { get; set; }
    [field: SerializeField] public float BulletAliveTime { get; set; }
    

    [field: Header("Other/Debug")]
    public Collider2D[] attackCollider2d { get; set; }
    [field: SerializeField, MyReadOnly] public AWeapon weaponReference { get; set; }
    [field: SerializeField, MyReadOnly] public UnityEngine.Vector2 ActualDirection { get; set; }


    /// <summary>
    /// Initializes the attack values based on the provided AttackSO.
    /// </summary>
    /// <param name="attackSO">The AttackSO containing the attack values.</param>
    public virtual void InitAttackValues(AttackSO attackSO, AWeapon weaponRef, UnityEngine.Vector2 direction)
    {
        // Set the attack values based on the AttackSO
        TimeToActivateHitbox = attackSO.TimeToActivateHitbox;

        TimeDurationHitbox = attackSO.TimeDurationHitbox;

        TimeToEndHitbox = attackSO.TimeToEndHitbox;

        TimeComboProgression = attackSO.TimeComboProgression;

        AttackCooldown = attackSO.AttackCooldown;

        PlayerSpeedModifier = attackSO.PlayerSpeedModifier;
        PlayerDrag = attackSO.PlayerDrag;

        BaseDamageAttack = BaseDamageAttack > 0 ? BaseDamageAttack : weaponRef.BaseDamageWeapon;
        damageInstance.damageValueAtkOrSec = BaseDamageAttack;
        damageInstance.type = attackSO.DamageType;

        AttackRangeAttack = attackSO.AttackRangeAttack;

        DamageType = attackSO.DamageType;

        StatusEffects = attackSO.StatusEffects;

        KnockbackForceAttack = attackSO.KnockbackForceAttack;

        BulletSpeed = attackSO.BulletSpeed;

        BulletAliveTime = attackSO.BulletAliveTime;

        attackCollider2d = GetComponents<Collider2D>();
        ManageAttackColliders(false);

        weaponReference = weaponRef;
        ActualDirection = direction;

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
        DoBeforeWaitHitboxActivation();

        yield return new WaitForSeconds(TimeToActivateHitbox);

        //Activate Hitbox
        
        DoAfterWaitHitboxActivation();

        Debug.Log("Attivato");

        yield return new WaitForSeconds(TimeDurationHitbox);

        //Deactivate Hitbox
        
        DoBeforeAttackEnd();

        Debug.Log("Disattivato");

        yield return new WaitForSeconds(TimeToEndHitbox);

        //End of attack

        DoInAttackEnd();

        Debug.Log("Fine");
    }

    public virtual void DoBeforeWaitHitboxActivation()
    {
        if(AttackRangeAttack == AttackRange.Ranged) Destroy(this.gameObject, BulletAliveTime);
    }

    public virtual void DoAfterWaitHitboxActivation()
    {
        ManageAttackColliders(true);
    }

    public virtual void DoBeforeAttackEnd()
    {
        ManageAttackColliders(false);
    }

    public virtual void DoInAttackEnd()
    {
        weaponReference.SetTimerComboProgression(TimeComboProgression);
    }

    public virtual void OnDamageableHit(Collider2D other)
    {
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

    public void ManageAttackColliders(bool isEnabled)
    {
        foreach (Collider2D collider in attackCollider2d)
        {
            collider.enabled = isEnabled;
        }
    }
}