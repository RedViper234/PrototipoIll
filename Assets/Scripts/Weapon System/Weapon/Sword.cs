using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [field: SerializeField] public WeaponSO WeaponSO { get; set; }
    [field: SerializeField] public float BaseDamageWeapon { get; set; }
    [field: SerializeField] public float ComboTimeProgression { get; set; }
    [field: SerializeField] public float PlayerSpeedModifier { get; set; }
    [field: SerializeField] public AttackRange AttackRangeWeapon { get; set; }
    [field: SerializeField] public DamageType DamageType { get; set; }
    [field: SerializeField] public List<StatusStruct> StatusEffects { get; set; }
    [field: SerializeField] public float KnockbackForceWeapon { get; set; }
    [field: SerializeField] public List<Attack> ComboList { get; set; }
}
