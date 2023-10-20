using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [field: SerializeField] public WeaponSO WeaponSO { get; set; }
    [field: SerializeField] public Animator animator { get; set;}
    [field: SerializeField] public float BaseDamageWeapon { get; set; }
    [field: SerializeField] public float ComboTimeProgression { get; set; }
    [field: SerializeField] public float PlayerSpeedModifier { get; set; }
    [field: SerializeField] public AttackRange AttackRangeWeapon { get; set; }
    [field: SerializeField] public DamageTypex DamageType { get; set; }
    [field: SerializeField] public List<StatusStruct> StatusEffects { get; set; }
    [field: SerializeField] public float KnockbackForceWeapon { get; set; }
    [field: SerializeField] public List<AttackSO> ComboList { get; set; }

    public void Awake()
    {
        InitWeaponValues();

        animator = GetComponent<Animator>();
    }

    public void InitWeaponValues()
    {
        BaseDamageWeapon = WeaponSO.BaseDamageWeapon;

        ComboTimeProgression = WeaponSO.ComboTimeProgression;

        PlayerSpeedModifier = WeaponSO.PlayerSpeedModifier;

        AttackRangeWeapon = WeaponSO.AttackRangeWeapon;

        DamageType = WeaponSO.DamageType;

        StatusEffects = WeaponSO.StatusEffects;

        KnockbackForceWeapon = WeaponSO.KnockbackForceWeapon;
    }

    public void ExecuteCombo()
    {
        //Debug.Log("ExecuteCombo");
        //TODO 
        /*
        ****Iniziare con il primo attacco della combo****
        ****Start Cooldown tra attacchi****
        ****
        */
    }
}
