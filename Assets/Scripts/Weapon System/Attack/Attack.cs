using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AAttack
{
    [field: SerializeField] public override AttackSO attackSO { get; set; }
    [field: SerializeField] public override float TimeToActivateHitbox { get; set; }
    [field: SerializeField] public override float TimeDurationHitbox { get; set; }
    [field: SerializeField] public override float TimeToEndHitbox { get; set; }
    [field: SerializeField] public override float PlayerSpeedModifier { get; set; }
    [field: SerializeField] public override PlayerDragStruct PlayerDrag { get; set; }
    [field: SerializeField] public override float AttackCooldown { get; set; }
    [field: SerializeField] public override float BaseDamageAttack { get; set; }
    [field: SerializeField] public override AttackRange AttackRangeAttack { get; set; }
    [field: SerializeField] public override DamageType.DamageTypes DamageType { get; set; }
    [field: SerializeField] public override List<StatusStruct> StatusEffects { get; set; }
    [field: SerializeField] public override float KnockbackForceAttack { get; set; }
    [field: SerializeField] public override MultiAttack MultiAttack { get; set; }
}


