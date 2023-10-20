using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponkSO", menuName = "Weapon/NewWeapon")]

public class WeaponSO : ScriptableObject
{
    [Header("Weapon Settings")]
    public float BaseDamageWeapon;
    public float ComboTimeProgression;
    public AttackRange AttackRangeWeapon;
    public DamageTypex DamageType;
    [field: SerializeField] public List<StatusStruct> StatusEffects;
    public float KnockbackForceWeapon;

    [Space(10)]

    [Header("Player Settings")]
    [Range(0, 1)] public float PlayerSpeedModifier;
}
