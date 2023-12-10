using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponkSO", menuName = "Weapon/NewWeapon")]

public class WeaponSO : ScriptableObject
{
    [Header("Weapon Settings")]
    public float BaseDamageWeapon;
    public float CooldownBetweenAttacks;
    public float CooldownSpecialAttack;
    public float ComboTimeProgression;
    public AttackRange AttackRangeWeapon;
    public List<DamageType.DamageTypes> DamageType;
    [field: SerializeField] public List<StatusStruct> StatusEffects;
    public float KnockbackForceWeapon;
    public float RangeAttackMaxDistance;
    public Vector2 HurtboxSize;

    [Space(10)]

    [Header("Player Settings")]
    [Range(0, 1)] public float PlayerSpeedModifier;
}
