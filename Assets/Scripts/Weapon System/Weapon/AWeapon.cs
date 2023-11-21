using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum AttackRange
{
    None,
    Ranged,
    Melee
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

public struct StatusStruct
{
    public StatusType type;
    [Range(0, 1)] public float probability;
}

public abstract class AWeapon : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponSO { get; set; }

    [field: Header("Weapon Animation")]
    [field: SerializeField] public Animator animator { get; set; }
    
    [field: Header("Weapon Properties Values")]
    [field: SerializeField] public float BaseDamageWeapon { get; set; }
    [field: SerializeField] public float CooldownBetweenAttacks { get; set; }
    [field: SerializeField] public float ComboTimeProgression { get; set; }
    [field: SerializeField] public float PlayerSpeedModifier { get; set; }
    [field: SerializeField] public AttackRange AttackRangeWeapon { get; set; }
    [field: SerializeField] public List<DamageType.DamageTypes> DamageType { get; set; }
    [field: SerializeField] public List<StatusStruct> StatusEffects { get; set; }
    [field: SerializeField] public float KnockbackForceWeapon { get; set; }
    [field: SerializeField] public PlayerDragStruct playerDrag { get; set; }
    [field: SerializeField] public List<AttackSO> ComboList { get; set; }
    [field: SerializeField] public float RangeAttackMaxDistance { get; set; }
    [field: SerializeField] public Vector2 HurtboxSize { get; set; }
    [field: SerializeField, MyReadOnly] public Vector2 ActualDirection { get; set; }

    [field: Header("Debug")]
    [field: SerializeField, MyReadOnly] protected float t_cooldown { get; set; }
    [field: SerializeField, MyReadOnly] protected float t_currentCombo { get; set; }
    [field: SerializeField] protected int comboIndex { get; set; }
    private Coroutine comboCoroutine;

    public virtual void Awake()
    {
        InitWeaponValues();

        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //Cooldown del tempo di attacco
        _ = t_cooldown > 0 ? t_cooldown -= Time.deltaTime : t_cooldown = 0;

        //Tempo di reset combo index
        _ = t_currentCombo > 0 ? t_currentCombo -= Time.deltaTime : comboIndex = 0;

        if(!CheckAttackChildren())
        {
            WeaponRotation(); 
        }
    }

    public void InitWeaponValues()
    {
        BaseDamageWeapon = WeaponSO.BaseDamageWeapon;

        CooldownBetweenAttacks = WeaponSO.CooldownBetweenAttacks;

        ComboTimeProgression = WeaponSO.ComboTimeProgression;

        PlayerSpeedModifier = WeaponSO.PlayerSpeedModifier;

        AttackRangeWeapon = WeaponSO.AttackRangeWeapon;

        DamageType = WeaponSO.DamageType;

        StatusEffects = WeaponSO.StatusEffects;

        KnockbackForceWeapon = WeaponSO.KnockbackForceWeapon;
    }

    public void ExecuteCombo()
    {
        StartCoroutine(AttackCoroutine());
    }

    public void StopCombo()
    {
        StopCoroutine(comboCoroutine);
    }
    
    public void GenerateAttackObject(AttackSO actualAttack, bool startCoroutine)
    {
        GameObject inst_attack;

        inst_attack = Instantiate(
            actualAttack.AttackPrefab, 
            transform.position, 
            transform.parent.rotation, 
            transform);

        inst_attack.GetComponent<AAttack>().InitAttackValues(
                                                            actualAttack, 
                                                            this, 
                                                            (Vector2)(this.transform.position - this.transform.parent.position));
        
        if(startCoroutine) comboCoroutine = StartCoroutine(inst_attack.GetComponent<AAttack>().InitializeAttack());

        if(AttackRangeWeapon == AttackRange.Ranged) inst_attack.transform.parent = null;
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        if (t_cooldown <= 0)
        {
            if(comboIndex == ComboList.Count) 
            {
                LastComboAttack();
            }

            GenerateAttackObject(ComboList[comboIndex], true);

            comboIndex++;

            t_cooldown = CooldownBetweenAttacks;
        }

        yield return null;
    }

    protected virtual void LastComboAttack()
    {
        comboIndex = 0;
    }

    protected void WeaponRotation()
    {
        // Ottieni la posizione del mouse nello spazio del mondo
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcola la direzione dal player alla posizione del mouse
        Vector2 direction = mousePosition - (Vector2)transform.parent.position;

        // Calcola l'angolo tra la direzione calcolata e il vettore "up" del player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Ruota l'arma attorno al player utilizzando l'angolo calcolato
        transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    protected bool CheckAttackChildren()
    {
        return GetComponentInChildren<AAttack>();
    }

    public void SetTimerComboProgression(float time)
    {
        t_currentCombo = time > 0 ? time : ComboTimeProgression;
    }
}
