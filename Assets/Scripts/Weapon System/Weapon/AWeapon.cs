using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public enum AttackRange
{
    None,
    Ranged,
    Melee
}

[Serializable]
public struct StatusStruct
{
    public DamageType.DamageTypes type;
    [Range(0, 1)] public float probability;
}

public abstract class AWeapon : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponSO;

    [field: Header("Weapon Animation")]
    [field: SerializeField] public Animator animator;
    
    [field: Header("Weapon Properties Values")]
    [field: SerializeField] public float BaseDamageWeapon;
    [field: SerializeField] public float CooldownBetweenAttacks;
    [field: SerializeField] public float CooldownSpecialAttack;
    [field: SerializeField] public float ComboTimeProgression;
    [field: SerializeField] public float PlayerSpeedModifier;
    [field: SerializeField] public AttackRange AttackRangeWeapon;
    [field: SerializeField] public List<DamageType.DamageTypes> DamageType;
    [field: SerializeField] public List<StatusStruct> StatusEffects;
    [field: SerializeField] public float KnockbackForceWeapon;
    [field: SerializeField] public PlayerDragStruct playerDrag;
    [field: SerializeField] public List<AttackSO> ComboList;
    [field: SerializeField] public AttackSO SpecialAttack;
    [field: SerializeField] public float RangeAttackMaxDistance;
    [field: SerializeField] public Vector2 HurtboxSize;
    [field: SerializeField, MyReadOnly] public Vector2 ActualDirection;

    [field: Header("Debug")]
    [field: SerializeField, MyReadOnly] protected float t_cooldownAttack;
    [field: SerializeField, MyReadOnly] protected float t_cooldownSpecialAttack;
    [field: SerializeField, MyReadOnly] protected float t_currentCombo;
    [field: SerializeField] protected int comboIndex = 0;
    [SerializeField] protected float offsetLowAim = 0f;
    [SerializeField] protected float offsetUpAim = 1f;
    [SerializeField, MyReadOnly] protected int counter = 0;
    private Coroutine comboCoroutine;

    public virtual void Awake()
    {
        InitWeaponValues();

        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        //Cooldown del tempo di attacco
        SetTimerCooldown(ref t_cooldownAttack);

        //Tempo di reset combo index
        _ = t_currentCombo > 0 ? t_currentCombo -= Time.deltaTime : comboIndex = 0;

        //Cooldown del tempo di attacco Speciale
        SetTimerCooldown(ref t_cooldownSpecialAttack);

        WeaponRotation(offsetLowAim, offsetUpAim);   
    }

    public void InitWeaponValues()
    {
        BaseDamageWeapon = WeaponSO.BaseDamageWeapon;

        CooldownBetweenAttacks = WeaponSO.CooldownBetweenAttacks;

        CooldownSpecialAttack = WeaponSO.CooldownSpecialAttack;

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

    public void ExecuteSpecialAttack()
    {
        if(SpecialAttack != null) StartCoroutine(SpecialAttackCoroutine());
    }

    private IEnumerator SpecialAttackCoroutine()
    {
       if (t_cooldownSpecialAttack <= 0 && !CheckAttackChildren())
        {
            GenerateAttackObject(SpecialAttack, true);

            t_cooldownSpecialAttack = CooldownSpecialAttack;
        }

        yield return null;
    }

    public void StopCombo()
    {
        StopCoroutine(comboCoroutine);
    }
    
    public void GenerateAttackObject(AttackSO actualAttack, bool startCoroutine)
    {

        GameObject inst_attack = Instantiate(
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

        // Debug.Log("Attacco Generato");
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        if (t_cooldownAttack <= 0 && !CheckAttackChildren())
        {
            if(comboIndex == ComboList.Count) 
            {
                LastComboAttack();
            }

            GenerateAttackObject(ComboList[comboIndex], true);

            t_cooldownAttack = CooldownBetweenAttacks;

            SetTimerComboProgression(5f);
            
            comboIndex++;
        }

        yield return null;
    }

    protected void WeaponRotation(float lowerLimit, float upperLimit)
    {
        if(!CheckAttackChildren())
        {
            // Ottieni la posizione del mouse nello spazio del mondo
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calcola la direzione dal player alla posizione del mouse
            Vector2 direction = mousePosition - (Vector2)transform.parent.position;

            // Calcola l'angolo tra la direzione calcolata e il vettore "up" del player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Ruota l'arma attorno al player utilizzando l'angolo calcolato
            float angleFromFloor = Math.Abs(angle % 45);

            if (angleFromFloor <= lowerLimit)
            {
                transform.parent.rotation = Quaternion.AngleAxis(angle > 0 ? angle - angleFromFloor : angle + angleFromFloor, Vector3.forward);
            } 
            else if (angleFromFloor >= upperLimit)
            {
                float angleDiff = Math.Abs((angleFromFloor - 45));
                transform.parent.rotation = Quaternion.AngleAxis(angle > 0 ? angle + angleDiff : angle - angleDiff, Vector3.forward);
            }
        }
    }

    protected bool CheckAttackChildren()
    {
        return GetComponentInChildren<AAttack>();
    }

    public void SetTimerComboProgression(float time)
    {
        t_currentCombo = time > 0 ? time : ComboTimeProgression;
    }

    protected void SetTimerCooldown(ref float time) 
    {
        time = time > 0 ? time -= Time.deltaTime : 0;
    }

    protected virtual void LastComboAttack()
    {
        comboIndex = 0;
    }
}