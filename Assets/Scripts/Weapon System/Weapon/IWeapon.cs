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

public enum DamageTypex
{
    None,
    Fisico
}

public enum StatusType
{
    None
}

[Serializable]
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
    public WeaponSO WeaponSO { get; set;}
    public Animator animator { get; set;}
    public float BaseDamageWeapon { get; set;}
    public float ComboTimeProgression { get; set;}
    public float PlayerSpeedModifier { get; set;}
    public AttackRange AttackRangeWeapon { get; set;}
    public DamageType.DamageTypes DamageType { get; set;}
    public List<StatusStruct> StatusEffects { get; set;}
    public float KnockbackForceWeapon { get; set;}
    public List<AttackSO> ComboList { get; set;}
    void Awake();
    void InitWeaponValues();
    void ExecuteCombo();
}
