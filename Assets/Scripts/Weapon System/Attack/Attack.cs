using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour, IAttack
{
    public AttackSO attackSO { get; set;}
    public GameObject AttackPrefab { get; set;}
    public float TimeToActivateHitbox { get; set;}
    public float TimeDurationHitbox { get; set;}
    public float TimeToEndHitbox { get; set;}
    public float PlayerSpeedModifier { get; set;}
    public PlayerDragStruct PlayerDrag { get; set;}
    public float AttackCooldown { get; set;}
    public float BaseDamageAttack { get; set;}
    public AttackRange AttackRangeAttack { get; set;}
    public DamageType DamageType { get; set;}
    public List<StatusStruct> StatusEffects { get; set;}
    public float KnockbackForceAttack { get; set;}
}
