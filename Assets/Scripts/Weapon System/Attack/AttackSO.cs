using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSO", menuName = "Attack/NewAttack")]
public class AttackSO : ScriptableObject
{
    [Header("Attack Settings")][Space(10)]
    public GameObject AttackPrefab;

    [Space(12)]

    [Header("Time Settings")][Space(10)]
    public float TimeToActivateHitbox;
    public float TimeDurationHitbox;
    public float TimeToEndHitbox;
    public float AttackCooldown;

    [Space(12)]

    [Header("Damage Settings")][Space(10)]
    public float PlayerSpeedModifier;
    public PlayerDragStruct PlayerDrag;
    public float BaseDamageAttack;
    public AttackRange AttackRangeAttack;
    public DamageType DamageType;
    [SerializeField] public List<StatusStruct> StatusEffects;
    public float KnockbackForceAttack;
}
