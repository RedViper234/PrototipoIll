using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSO", menuName = "Attack/NewAttack")]
public class AttackSO : ScriptableObject
{
    [Header("Attack Settings")][Space(10)]
    public GameObject AttackPrefab;
    public AnimationClip AttackAnimation;

    [Space(12)]

    [Header("Time Settings")][Space(10)]
    public float TimeToActivateHitbox;
    public float TimeDurationHitbox;
    public float TimeToEndHitbox;
    public float AttackCooldown;
    public float TimeComboProgression;

    [Space(12)]

    [Header("Damage Settings")][Space(10)]
    [Range(0, 1)] public float PlayerSpeedModifier;
    public PlayerDragStruct PlayerDrag;
    public float BaseDamageAttack;
    public AttackRange AttackRangeAttack;
    public DamageType.DamageTypes DamageType;
    [SerializeField] public List<StatusStruct> StatusEffects;
    public float KnockbackForceAttack;
    public MultiAttack MultiAttack;
    public float BulletSpeed;
    public float BulletAliveTime;
}
