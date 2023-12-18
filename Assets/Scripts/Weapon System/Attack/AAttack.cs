using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

[Serializable]
public struct MultiAttack
{
    public float NumberOfAttacks;
    public float TimeBetweenAttacks;
    public AttackSO[] AttackList;
}

[Serializable]
public struct PlayerDragStruct
{
    public bool canDrag;
    public float force, waiting, duration;
    [MyReadOnly] public Vector2 direction;
    public DragDirection dragDirection;
    public PlayerDragActiovationMoment activationMoment;
}

[Serializable]
public enum PlayerDragActiovationMoment
{
    BeforeAttack,
    AfterAttack,
    None
}

[Serializable]
public enum DragDirection
{
    WeaponDefault,
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight,
    Forward,
    ForwardInMovement,
    Backward,
    BackWardInMovement
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
    [field: SerializeField] public PlayerDragStruct playerDrag;
    [field: SerializeField] public float BaseDamageAttack;
    [field: SerializeField] public AttackRange AttackRangeAttack;
    [field: SerializeField] public List<DamageType.DamageTypes> DamageTypeAttack;
    [field: SerializeField] public List<StatusStruct> StatusEffects;
    [field: SerializeField] public float KnockbackForceAttack;
    [field: SerializeField] public MultiAttack MultiAttack;
    [field: SerializeField] public DamageInstance damageInstance;

    [field: Header("Ranged Attack Values")]
    [field: SerializeField] public float BulletSpeed;
    [field: SerializeField] public float BulletAliveTime;
    
    [field: Header("Other/Debug")]
    [field: SerializeField, MyReadOnly] public Collider2D[] attackCollider2d;
    [field: SerializeField, MyReadOnly] public AWeapon weaponReference;
    [field: SerializeField, MyReadOnly] public Vector2 ActualDirection;
    [field: SerializeField, MyReadOnly] public AttackSO attackSODefault;
    [field: SerializeField, MyReadOnly] public GameObject playerDragCoroutine;


    /// <summary>
    /// Initializes the attack values based on the provided AttackSO.
    /// </summary>
    /// <param name="attackSO">The AttackSO containing the attack values.</param>
    public virtual void InitAttackValues(AttackSO attackSO, AWeapon weaponRef, Vector2 direction)
    {
        weaponReference = weaponRef;

        ActualDirection = direction;

        attackSODefault = attackSO;

        ManageAttackColliders(false);

        // Set the attack values based on the AttackSO
        TimeToActivateHitbox = attackSO.TimeToActivateHitbox;

        TimeDurationHitbox = attackSO.TimeDurationHitbox;

        TimeToEndHitbox = attackSO.TimeToEndHitbox;

        TimeComboProgression = attackSO.TimeComboProgression;

        AttackCooldown = attackSO.AttackCooldown;

        PlayerSpeedModifier = attackSO.PlayerSpeedModifier;
        
        if(attackSO.PlayerDrag.canDrag == true)
        {
            playerDrag = attackSO.PlayerDrag.force == 0 ? weaponRef.playerDrag : attackSO.PlayerDrag;  
        }
        else
        {
            playerDrag.force = 0;
            playerDrag.duration = 0;
            playerDrag.waiting = 0;
            playerDrag.activationMoment = PlayerDragActiovationMoment.None;
        }
        
        DamageTypeAttack = attackSO.DamageType.Count > 0 ? attackSO.DamageType : weaponRef.DamageType;
        BaseDamageAttack = BaseDamageAttack > 0 ? BaseDamageAttack : weaponRef.BaseDamageWeapon;
        damageInstance.damageValueAtkOrSec = BaseDamageAttack;
        damageInstance.type = DamageTypeAttack[0];

        AttackRangeAttack = attackSO.AttackRangeAttack != AttackRange.None ? attackSO.AttackRangeAttack : weaponRef.AttackRangeWeapon;

        StatusEffects = attackSO.StatusEffects.Count > 0 ? attackSO.StatusEffects : weaponRef.StatusEffects;

        KnockbackForceAttack = attackSO.KnockbackForceAttack == 0 ? weaponRef.KnockbackForceWeapon : attackSO.KnockbackForceAttack;

        MultiAttack = attackSO.MultiAttack;

        BulletSpeed = attackSO.BulletSpeed;

        BulletAliveTime = attackSO.BulletAliveTime;

        attackCollider2d = GetComponents<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // UnityEngine.Debug.Log("Hit");
        OnDamageableHit(other);
    }

    public virtual IEnumerator InitializeAttack()
    {
        // UnityEngine.Debug.Log($"Attacco in corso {MultiAttack.AttackList.Count()}");

        for (var i = 0; i < (MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList.Count(): 1); i++)
        {
            // UnityEngine.Debug.Log($"MultiAttacco {i+1}/{MultiAttack.AttackList.Count()}: {(MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList[i].name : attackSODefault.name)}");

            if(MultiAttack.AttackList.Count() > 0)
            {
                weaponReference.GenerateAttackObject(
                    MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList[i]: attackSODefault, 
                    false);

            }

            //Prima che venga attivata l'hitbox    

            DoBeforeWaitHitboxActivation();

            yield return new WaitForSeconds(TimeToActivateHitbox + (playerDrag.activationMoment == PlayerDragActiovationMoment.BeforeAttack ? playerDrag.duration : 0));

            //Activate Hitbox
            
            DoAfterWaitHitboxActivation();

            // UnityEngine.Debug.Log("Attivato");

            yield return new WaitForSeconds(TimeDurationHitbox);

            //Deactivate Hitbox
            
            DoBeforeAttackEnd();

            // UnityEngine.Debug.Log("Disattivato");

            yield return new WaitForSeconds(TimeToEndHitbox + (playerDrag.activationMoment == PlayerDragActiovationMoment.AfterAttack ? playerDrag.duration : 0));

            //End of attack

            DoInAttackEnd();

            // UnityEngine.Debug.Log("Fine");
        }   
    }

    public virtual void DoBeforeWaitHitboxActivation()
    {
        if(AttackRangeAttack == AttackRange.Melee) EventManager.HandlePlayerAttackBegin?.Invoke(true);

        if(AttackRangeAttack == AttackRange.Ranged) Destroy(this.gameObject, BulletAliveTime);

        if(playerDrag.activationMoment == PlayerDragActiovationMoment.BeforeAttack) CreateDragObject();
    }

    public virtual void DoAfterWaitHitboxActivation()
    {
        ManageAttackColliders(true);
    }


    public virtual void DoBeforeAttackEnd()
    {
        ManageAttackColliders(false);

        if(playerDrag.activationMoment == PlayerDragActiovationMoment.AfterAttack) CreateDragObject();
    }

    public virtual void DoInAttackEnd()
    {
        weaponReference.SetTimerComboProgression(TimeComboProgression);

        EventManager.HandlePlayerAttackBegin?.Invoke(false);

        weaponReference.transform.parent.GetComponent<WeaponController>().playerController.ManageMovement(true);
    
        if(AttackRangeAttack == AttackRange.Melee) Destroy(this.gameObject);   
    }

    public virtual void OnDamageableHit(Collider2D other)
    {
        if(other.GetComponent<Damageable>())
        {
            // UnityEngine.Debug.Log("HIT");

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

    private void CreateDragObject()
    {
        GameObject dragobj = new GameObject();
        dragobj.name = "DragObject";
        dragobj.transform.parent = weaponReference.transform;
        dragobj.AddComponent<PlayerDrag>();
        dragobj.GetComponent<PlayerDrag>().StartDragging(weaponReference, playerDrag, ActualDirection);
    }
}