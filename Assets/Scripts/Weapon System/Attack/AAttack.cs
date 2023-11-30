using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct MultiAttack
{
    public float NumberOfAttacks;
    public float TimeBetweenAttacks;
    public AttackSO[] AttackList;
}

public abstract class AAttack : MonoBehaviour
{
    [field: SerializeField] public AnimationClip attackAnimation;
    
    [field: Header("Attack Time Values")]
    [field: SerializeField] public float TimeToActivateHitbox;
    [field: SerializeField] public float TimeDurationHitbox;
    [field: SerializeField] public float TimeToEndHitbox;
    [field: SerializeField] public float TimeComboProgression;
    [field: SerializeField] public float AttackCooldown;

    [field: Header("Attack Properties Values")]
    [field: SerializeField] public float PlayerSpeedModifier;
    [field: SerializeField] public PlayerDragStruct PlayerDrag;
    [field: SerializeField] public float BaseDamageAttack;
    [field: SerializeField] public AttackRange AttackRangeAttack;
    [field: SerializeField] public DamageType.DamageTypes DamageType;
    [field: SerializeField] public List<StatusStruct> StatusEffects;
    [field: SerializeField] public float KnockbackForceAttack;
    [field: SerializeField] public MultiAttack MultiAttack;
    [field: SerializeField] public DamageInstance damageInstance;

    [field: Header("Ranged Attack Values")]
    [field: SerializeField] public float BulletSpeed;
    [field: SerializeField] public float BulletAliveTime;
    
    [field: Header("Other/Debug")]
    public Collider2D[] attackCollider2d;
    [field: SerializeField, MyReadOnly] public AWeapon weaponReference;
    [field: SerializeField, MyReadOnly] public Vector2 ActualDirection;
    [field: SerializeField,MyReadOnly] public AttackSO attackSODefault;


    /// <summary>
    /// Initializes the attack values based on the provided AttackSO.
    /// </summary>
    /// <param name="attackSO">The AttackSO containing the attack values.</param>
    public virtual void InitAttackValues(AttackSO attackSO, AWeapon weaponRef, Vector2 direction)
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

        MultiAttack = attackSO.MultiAttack;

        BulletSpeed = attackSO.BulletSpeed;

        BulletAliveTime = attackSO.BulletAliveTime;

        attackCollider2d = GetComponents<Collider2D>();

        ManageAttackColliders(false);

        weaponReference = weaponRef;
        ActualDirection = direction;
        attackSODefault = attackSO;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit");
        OnDamageableHit(other);
    }

    public virtual IEnumerator InitializeAttack()
    {
        Debug.Log($"Attacco in corso {MultiAttack.AttackList.Count()}");

        for (var i = 0; i < (MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList.Count(): 1); i++)
        {
            Debug.Log($"MultiAttacco {i+1}/{MultiAttack.AttackList.Count()}: {(MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList[i].name : attackSODefault.name)}");

            weaponReference.GenerateAttackObject(
                MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList[i]: attackSODefault, 
                false);

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
            EventManager.HandleOnPlayerHit?.Invoke(other.gameObject);

            other.GetComponent<Damageable>().TakeDamage(damageInstance);
        }
    }

    public void WeaponAnimationStart()
    {
        gameObject.GetComponentInParent<AWeapon>().animator.Play(attackAnimation.name);
    }

    public void ManageAttackColliders(bool isEnabled)
    {
        foreach (Collider2D collider in attackCollider2d)
        {
            collider.enabled = isEnabled;
        }
    }
}