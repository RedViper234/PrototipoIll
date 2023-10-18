using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackSO", menuName = "Attack/NewAttack")]
public class AttackSO : ScriptableObject
{
    public GameObject AttackPrefab;
    public float TimeToActivateHitbox;
    public float TimeDurationHitbox;
    public float TimeToEndHitbox;
    public float PlayerSpeedModifier;
    public PlayerDragStruct PlayerDrag;
    public float AttackCooldown;
    public float BaseDamageAttack;
    public AttackRange AttackRangeAttack;
    public DamageType DamageType;
    [SerializeField] public List<StatusStruct> StatusEffects;
    public float KnockbackForceAttack;
}
