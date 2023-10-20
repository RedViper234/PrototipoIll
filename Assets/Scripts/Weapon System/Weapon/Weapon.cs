using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [field: SerializeField] public WeaponSO WeaponSO { get; set; }
    [field: SerializeField] public Animator animator { get; set;}
    [field: SerializeField] public float BaseDamageWeapon { get; set; }
    [field: SerializeField] public float ComboTimeProgression { get; set; }
    [field: SerializeField] public float PlayerSpeedModifier { get; set; }
    [field: SerializeField] public AttackRange AttackRangeWeapon { get; set; }
    [field: SerializeField] public DamageType.DamageTypes DamageType { get; set; }
    [field: SerializeField] public List<StatusStruct> StatusEffects { get; set; }
    [field: SerializeField] public float KnockbackForceWeapon { get; set; }
    [field: SerializeField] public List<AttackSO> ComboList { get; set; }

    [Header("Debug")]
    [SerializeField, MyReadOnly] private float t_cooldown;

    private Coroutine comboCoroutine;

    public void Awake()
    {
        InitWeaponValues();

        animator = GetComponent<Animator>();

    }

    void Update()
    {
        _ = t_cooldown > 0 ? t_cooldown -= Time.deltaTime : t_cooldown = 0;
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

        comboCoroutine = StartCoroutine(AttackCoroutine());
        
    }

    private void GenerateAttackObject(AttackSO actualAttack)
    {
        GameObject inst_attack;

        inst_attack = Instantiate(actualAttack.AttackPrefab, transform.parent.position, Quaternion.identity, transform.parent);

        inst_attack.GetComponent<Attack>().InitAttackValues(actualAttack);
    }

    private IEnumerator AttackCoroutine()
    {
        if (t_cooldown <= 0)
        {
            GenerateAttackObject(ComboList[0]);
            t_cooldown = ComboTimeProgression;
        }

        yield return null;
    }
}
