using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackRange
{
    None,
    Ranged,
    Melee
}

public enum DamageType
{
    None,
    Fisico
}

public enum StatusType
{
    None
}

public struct PlayerDragStruct
{
    public float force, waiting, duration;
    public Vector2 direction;
}

[Serializable]
public struct StatusStruct
{
    public StatusType type;
    [Range(0, 1)] public float probability;
}


public interface IWeapon
{
    [SerializeField] public WeaponSO WeaponSO { get; set;}
    public float BaseDamageWeapon { get; set;}
    public float ComboTimeProgression { get; set;}
    public float PlayerSpeedModifier { get; set;}
    public AttackRange AttackRangeWeapon { get; set;}
    public DamageType DamageType { get; set;}
    public List<StatusStruct> StatusEffects { get; set;}
    public float KnockbackForceWeapon { get; set;}
    public List<Attack> ComboList { get; set;}
}
