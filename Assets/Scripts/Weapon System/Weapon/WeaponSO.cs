using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponkSO", menuName = "Weapon/NewWeapon")]

[Serializable]
public class WeaponSO : ScriptableObject
{
    public float BaseDamageWeapon;
    public float ComboTimeProgression;
    [Range(0, 1)] public float PlayerSpeedModifier;
    public AttackRange AttackRangeWeapon;
    public DamageType DamageType;
    [field: SerializeField] public List<StatusStruct> StatusEffects;
    public float KnockbackForceWeapon;
}
