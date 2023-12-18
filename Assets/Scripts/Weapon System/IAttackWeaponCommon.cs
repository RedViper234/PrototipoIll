using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackWeaponCommon
{
    float TimeToActivateHitbox { get; }
    float TimeDurationHitbox { get; }
    float TimeToEndHitbox { get; }
    float TimeComboProgression { get; }
    float AttackCooldown { get; }
    float PlayerSpeedModifier { get; }
    PlayerDragStruct PlayerDrag { get; }
    List<DamageType.DamageTypes> DamageType { get; }
    float BaseDamageWeapon { get; }
    AttackRange AttackRangeWeapon { get; }
    List<StatusStruct> StatusEffects { get; }
    float KnockbackForceWeapon { get; }
}

