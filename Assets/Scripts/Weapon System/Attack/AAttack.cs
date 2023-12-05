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
    public float force, waiting, duration;
    [MyReadOnly] public Vector2 direction;
    public DragDirection dragDirection;
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
    [field: SerializeField] public DamageType.DamageTypes DamageType;
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
        playerDrag = attackSO.PlayerDrag;

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
        UnityEngine.Debug.Log("Hit");
        OnDamageableHit(other);
    }

    public virtual IEnumerator InitializeAttack()
    {
        UnityEngine.Debug.Log($"Attacco in corso {MultiAttack.AttackList.Count()}");

        for (var i = 0; i < (MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList.Count(): 1); i++)
        {
            UnityEngine.Debug.Log($"MultiAttacco {i+1}/{MultiAttack.AttackList.Count()}: {(MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList[i].name : attackSODefault.name)}");

            if(MultiAttack.AttackList.Count() > 0)
            {
                weaponReference.GenerateAttackObject(
                    MultiAttack.AttackList.Count() > 0 ? MultiAttack.AttackList[i]: attackSODefault, 
                    false);

            }

            //Prima che venga attivata l'hitbox    

            DoBeforeWaitHitboxActivation();

            yield return new WaitForSeconds(TimeToActivateHitbox);

            //Activate Hitbox
            
            DoAfterWaitHitboxActivation();

            UnityEngine.Debug.Log("Attivato");

            yield return new WaitForSeconds(TimeDurationHitbox);

            //Deactivate Hitbox
            
            DoBeforeAttackEnd();

            UnityEngine.Debug.Log("Disattivato");

            yield return new WaitForSeconds(TimeToEndHitbox);

            //End of attack

            DoInAttackEnd();

            UnityEngine.Debug.Log("Fine");
        }   
    }

    public virtual void DoBeforeWaitHitboxActivation()
    {
        if(AttackRangeAttack == AttackRange.Ranged) Destroy(this.gameObject, BulletAliveTime);

        EventManager.HandlePlayerAttackBegin?.Invoke(true);
    }

    public virtual void DoAfterWaitHitboxActivation()
    {
        ManageAttackColliders(true);
        StartCoroutine(DraggingPlayer());
    }

    public virtual void DoBeforeAttackEnd()
    {
        ManageAttackColliders(false);
    }

    public virtual void DoInAttackEnd()
    {
        EventManager.HandlePlayerAttackBegin?.Invoke(false);

        weaponReference.SetTimerComboProgression(TimeComboProgression);

        if(AttackRangeAttack == AttackRange.Melee)
        {
            Destroy(this.gameObject);   
        }
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

    protected IEnumerator DraggingPlayer()
    {
        GameObject player = weaponReference.transform.parent.GetComponent<WeaponController>().playerController.gameObject;
        // Wait for the specified amount of time before applying the drag
        yield return new WaitForSeconds(playerDrag.waiting);

        // Calculate the end time of the dragging effect
        var endDragTime = Time.time + playerDrag.duration;

        ChooseDirection();

        // Continue to apply the drag effect until the end time is reached
        while (Time.time < endDragTime)
        {
            // Apply the dragging force in the specified direction
            player.GetComponent<Rigidbody2D>().AddForce(playerDrag.direction * playerDrag.force);
            yield return null;
        }
    }

    private void ChooseDirection()
    {
        switch(playerDrag.dragDirection)
        {
            case DragDirection.WeaponDefault:
                playerDrag.direction = weaponReference.playerDrag.direction;
                break;
            case DragDirection.Up:
                playerDrag.direction = Vector2.up;
                break;
            case DragDirection.Down:
                playerDrag.direction = Vector2.down;
                break;
            case DragDirection.Left:
                playerDrag.direction = Vector2.left;
                break;
            case DragDirection.Right:
                playerDrag.direction = Vector2.right;
                break;
            case DragDirection.UpLeft:
                playerDrag.direction = Vector2.up + Vector2.left;
                break;
            case DragDirection.UpRight:
                playerDrag.direction = Vector2.up + Vector2.right;
                break;
            case DragDirection.DownLeft:
                playerDrag.direction = Vector2.down + Vector2.left;
                break;
            case DragDirection.DownRight:
                playerDrag.direction = Vector2.down + Vector2.right;
                break;
            case DragDirection.Forward:
                playerDrag.direction = ActualDirection;
                break;
            case DragDirection.Backward:
                playerDrag.direction = -ActualDirection;
                break;
        }
    }
}