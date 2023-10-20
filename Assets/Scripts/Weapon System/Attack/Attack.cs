using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{
    [field: SerializeField] public AttackSO attackSO { get; set;}

    // [field: SerializeField] public GameObject AttackPrefab { get; set;}
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

    void Awake()
    {
        
    }

    public void InitAttackValues(AttackSO attackSO)
    {
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

        StartCoroutine(InitializeAttack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<Damageable>())
        {
            other.GetComponent<Damageable>().TakeDamage(BaseDamageAttack, false, false);
        }
    }

    private IEnumerator InitializeAttack()
    {
        yield return new WaitForSeconds(TimeToActivateHitbox);

        //Activate Hitbox
        Debug.Log("Attivato");

        yield return new WaitForSeconds(TimeDurationHitbox);

        //Deactivate Hitbox
        Debug.Log("Disattivato");

        yield return new WaitForSeconds(TimeToEndHitbox);

        //End of attack
        Debug.Log("Fine");
    }
}


