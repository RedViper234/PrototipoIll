using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 direction = Vector2.down;
    public float acceleration = 5f;
    public float deceleration = 5f;
    public float baseSpeed = 5f;
    public float speedModifier = 1f;

    [Header("Melee Attack")]
    public UnityEngine.GameObject meleeAttackPrefab;
    //public float meleeDamage = 1f;
    public float meleeDamageModifier = 1f;
    public float meleeKnockbackModifier = 10f;
    public float meleeKnockbackDuration = 0.5f;
    public float meleeHitTime = 0.2f;
    public float meleeHitDelay = 0.1f;
    public float meleeRecoveryTime = 0.5f;
    public float stunTime = 0.1f;
    [Range(0,1)]
    public float critChance = 0.05f;
    [Range(0, 1)]
    public float atkSlow = 0.5f;
    private float lastAttackTime = 0f;
    private bool waitingForAttackPerformed = false;
    private float attackRange = 1f;
    private Vector2 atkDirection;
    private Coroutine atkRememberCor;


    [Header("Ranged Attack")]
    public UnityEngine.GameObject rangedProjectilePrefab;
    public float rangedDamage = 1f;
    public float rangedDamageModifier = 1f;
    public float projectileSpeed = 10f;
    //public float projectileSize = 1f;
    //public float projectileDeceleration = 2f;
    public float projectileDuration = 2f;
    //public bool destroyProjectileOnImpact = true;
    public float rangedAttackCooldown = 1f;
    private float lastShotTime = 0f;

    [Header("Dash")]
    public bool isDashing;
    public bool canDash = true;
    public float dashForce = 1;
    public float dashTime = 1;
    public float dashRecoveryTime = 1f;
    private float lastDash = 0;
    private int consecutiveDash = 0;
    public int maxConsecutiveDash = 1;
    public float dashRememberTime = 0.2f;
    private Coroutine dashRememberCor;

    [Header("Altre opzioni")]
    [Range(0,1)]
    public float merchantSales = 0;
    private float immunityTimer = 0;
    public float immunityFrameDuration = 0;

    private Vector2 movementInput;
    private Vector2 aimInput;
    private bool meleeAttacking;
    private bool rangedAttacking;
    private float meleeAttackTimer;
    private float rangedAttackTimer;
    private bool usingController = false;
    private Vector2 mousePos;
    private bool meleeMode = true;

    private Rigidbody2D rb;
    //public statCombat Forza = new statCombat();
    //public statSpeed Veloctà = new statSpeed();
    //public statResistence Costituzione = new statResistence();

    public PlayerInput actions;
    [Header("Player stats")]
    public statStrenght Strenght = new statStrenght();
    public statSpeed Speed = new statSpeed();
    public statAim Aim = new statAim();
    public statConstitution Constitution = new statConstitution();
    public statLuck Luck = new statLuck();

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (actions == null)
            actions = new PlayerInput();
        actions.Enable();
        setLevelsToStat();
    }


    private void OnEnable()
    {
        actions.Player.Move.Enable();
        actions.Player.Move.performed += OnMove;
        actions.Player.Move.canceled += OnMove;
        actions.Player.MousePos.performed += mousePosition;
        actions.Player.ChangeAttackMode.Enable();
        actions.Player.ChangeAttackMode.performed += ChangeMode;
        actions.Player.Dash.Enable();
        actions.Player.Dash.performed += OnDash;
        if (usingController)
        {
            actions.Player.Fire.Enable();
            actions.Player.Fire.performed += OnAimController;
            actions.Player.Fire.canceled += OnAimController;
        }
        else
        {
            actions.Player.FireMouse.Enable();
            actions.Player.FireMouse.started += OnAim;
            actions.Player.FireMouse.canceled += OffAim;
        }

        actions.Player.Meeleattack.Enable();
        actions.Player.Meeleattack.performed += OnMeleeAttack;

    }

    private void OnDisable()
    {
        actions.Player.Move.Disable();
        actions.Player.Move.performed -= OnMove;
        actions.Player.Move.canceled -= OnMove;
        actions.Player.Fire.Disable();
        actions.Player.FireMouse.Disable();
        actions.Player.FireMouse.performed -= OnAim;
        actions.Player.FireMouse.canceled -= OffAim;
        actions.Player.Fire.performed -= OnAimController;
        actions.Player.Fire.canceled -= OnAimController;
        actions.Player.Meeleattack.Disable();
        actions.Player.Meeleattack.performed -= OnAim;
    }
    private void Start()
    {
        Strenght.setStat();
        Speed.setStat();
        Constitution.setStat();
        Aim.setStat();
        Luck.setStat();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleMeleeAttack();
        HandleRangedAttack();
        HandleDash();
        HandleImmunity();
    }

    public void PlayerTakeDamage(List<DamageInstance> dmg)
    {
        foreach (var item in dmg)
        {
            if (isActuallyImmune() && !item.ignoreImmunityFrame)
            {
                continue;
            }
            switch (item.type)
            {
                case DamageType.DamageTypes.Fisico:                   
                    if (GetComponent<Damageable>())
                    {
                        GetComponent<Damageable>().TakeDamage(item);
                    }
                    else
                    {
                        Debug.LogError("Manca il damageable");
                    }
                    break;
                case DamageType.DamageTypes.Fuoco:
                    if (GetComponent<Damageable>())
                    {
                        GetComponent<Damageable>().TakeDamage(item);
                    }
                    else
                    {
                        Debug.LogError("Manca il damageable");
                    }
                    break;
                case DamageType.DamageTypes.Ustioni:
                    if (GetComponent<Damageable>())
                    {
                        GetComponent<Damageable>().TakeDamage(item);
                    }
                    else
                    {
                        Debug.LogError("Manca il damageable");
                    }
                    break;
                case DamageType.DamageTypes.Malattia:
                    if (GetComponent<MalattiaHandler>())
                    {
                        GetComponent<MalattiaHandler>().TakeDamage(item);
                    }
                    else
                    {
                        Debug.LogError("Manca il malattia handler");
                    }
                    break;
                case DamageType.DamageTypes.Corruzione:
                    if (GetComponent<MalattiaHandler>())
                    {
                        GetComponent<MalattiaHandler>().TakeDamage(item);
                    }
                    else
                    {
                        Debug.LogError("Manca il malattia handler");
                    }
                    break;
                default:
                    break;
            }
        }

        setImmunity();
    }

    public void PlayerTakeDamage(DamageInstance dmg)
    {
            if (isActuallyImmune() && !dmg.ignoreImmunityFrame)
            {
                return;
            }
            switch (dmg.type)
            {
                case DamageType.DamageTypes.Fisico:
                    if (GetComponent<Damageable>())
                    {
                        GetComponent<Damageable>().TakeDamage(dmg);
                    }
                    else
                    {
                        Debug.LogError("Manca il damageable");
                    }
                    break;
                case DamageType.DamageTypes.Fuoco:
                    if (GetComponent<Damageable>())
                    {
                        GetComponent<Damageable>().TakeDamage(dmg);
                    }
                    else
                    {
                        Debug.LogError("Manca il damageable");
                    }
                    break;
                case DamageType.DamageTypes.Ustioni:
                    if (GetComponent<Damageable>())
                    {
                        GetComponent<Damageable>().TakeDamage(dmg);
                    }
                    else
                    {
                        Debug.LogError("Manca il damageable");
                    }
                    break;
                case DamageType.DamageTypes.Malattia:
                    if (GetComponent<MalattiaHandler>())
                    {
                        GetComponent<MalattiaHandler>().TakeDamage(dmg);
                    }
                    else
                    {
                        Debug.LogError("Manca il malattia handler");
                    }
                    break;
                case DamageType.DamageTypes.Corruzione:
                    if (GetComponent<MalattiaHandler>())
                    {
                        GetComponent<MalattiaHandler>().TakeDamage(dmg);
                    }
                    else
                    {
                        Debug.LogError("Manca il malattia handler");
                    }
                    break;
                default:
                    break;
            }        
        setImmunity();
    }
    public void PlayerDeath()
    {
        Debug.Log("sei morto");
    }

    private void HandleImmunity()
    {
        if (immunityTimer > 0f)
        {
            // Handle immunity logic, e.g., making the player flash
            immunityTimer -= Time.deltaTime;
        }
    }

    public bool isActuallyImmune()
    {
        return (immunityTimer > 0);
    }

    public void setImmunity()
    {
        immunityTimer = immunityFrameDuration;
    }

    public void setImmunity(float time)
    {
        immunityTimer = time;
    }

    private void HandleMovement()
    {
        Vector2 input = (waitingForAttackPerformed ? atkDirection.normalized : movementInput.normalized);
        if (!isDashing)
        {
            if (input.magnitude > 0.1f)
            {
                Vector2 velocity = rb.velocity;
                velocity = Vector2.Lerp(velocity, input * baseSpeed * speedModifier * (waitingForAttackPerformed ? 1 - atkSlow : 1), acceleration * Time.fixedDeltaTime);
                rb.velocity = velocity;

            }
            else
            {
                Vector2 velocity = rb.velocity;
                velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
                rb.velocity = velocity;
            }
        }
    }

    private void HandleMeleeAttack()
    {
        if (meleeMode && meleeAttacking && Time.timeSinceLevelLoad - lastAttackTime >= meleeRecoveryTime && !waitingForAttackPerformed)
        {
            if (!isDashing)
            {
                // Perform melee attack logic
                waitingForAttackPerformed = true;
                atkDirection = direction;
                Invoke("attackMelee", meleeHitDelay);
            }
            else
            {
                atkRememberCor = StartCoroutine(rememberAtk());
            }
        }
    }

    IEnumerator rememberAtk()
    {
        yield return new WaitUntil(()=> !isDashing);
        HandleMeleeAttack();
        atkRememberCor = null;
    }

    private void attackMelee()
    {
        lastAttackTime = Time.timeSinceLevelLoad;
        UnityEngine.GameObject attack = Instantiate(meleeAttackPrefab, new Vector3(GetComponent<Transform>().position.x + attackRange * atkDirection.x, GetComponent<Transform>().position.y + attackRange * atkDirection.y, 1), Quaternion.identity);
        // Calcola l'angolo di rotazione in base a direction
        float angle = Mathf.Atan2(atkDirection.y, atkDirection.x) * Mathf.Rad2Deg;

        // Crea una rotazione basata sull'angolo calcolato
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        // Applica la rotazione a attack
        attack.transform.rotation = targetRotation;
        meleeAttacking = false;
        StartCoroutine(destroyattack(attack));
    }

    public IEnumerator destroyattack(GameObject attack)
    {
        yield return new WaitForSeconds(meleeHitTime);
        waitingForAttackPerformed = false;
        Destroy(attack);
    }

    private void HandleRangedAttack()
    {
        if (!meleeMode && rangedAttacking && Time.timeSinceLevelLoad - lastShotTime >= rangedAttackCooldown)
        {
            UnityEngine.GameObject projectile = Instantiate(rangedProjectilePrefab, GetComponent<Transform>().position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (!usingController)
            {
                Vector2 direction = (Camera.main.ScreenToWorldPoint(mousePos) - GetComponent<Transform>().position).normalized;
                direction = new Vector2(direction.x * 1000, direction.y * 1000).normalized;
                Vector2 force = direction * projectileSpeed;

                rb.AddForce(force, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(aimInput * projectileSpeed, ForceMode2D.Impulse);
            }
            Destroy(projectile, projectileDuration);
            lastShotTime = Time.timeSinceLevelLoad;
        }
    }

    private void HandleDash()
    {
        if (isDashing && !waitingForAttackPerformed)
        {
            Vector2 velocity = rb.velocity;
            velocity = Vector2.Lerp(velocity, direction * dashForce, dashTime * Time.fixedDeltaTime);
            rb.velocity = velocity;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        if (movementInput.magnitude > 0 /*&& !isDashing*/)
        {
            direction = movementInput;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (canDash && !isDashing && !waitingForAttackPerformed && ((lastDash + dashRecoveryTime < Time.timeSinceLevelLoad) || consecutiveDash < maxConsecutiveDash))
        {
            if (lastDash + dashRecoveryTime < Time.timeSinceLevelLoad)
            {
                consecutiveDash = 0;
            }
            consecutiveDash++;
            isDashing = true;
            Invoke("StopDash", dashTime);
        }
        else if (canDash && isDashing)
        {
            if (dashRememberCor != null)
            {
                StopCoroutine(dashRememberCor);
            }
            dashRememberCor = StartCoroutine(rememberDash());
        }
        else if (canDash && waitingForAttackPerformed)
        {
            if (dashRememberCor != null)
            {
                StopCoroutine(dashRememberCor);
            }
            dashRememberCor = StartCoroutine(rememberDash());
        }
    }

    IEnumerator rememberDash()
    {
        float currentTime = 0;
        while (currentTime < dashRememberTime)
        {
            if (canDash && !isDashing && !waitingForAttackPerformed && ((lastDash + dashRecoveryTime < Time.timeSinceLevelLoad) || consecutiveDash < maxConsecutiveDash))
            {
                InputAction.CallbackContext cont = new();
                OnDash(cont);
                break;
            }
            else
            {
                currentTime += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
        dashRememberCor = null;
    }

    public void StopDash()
    {
        isDashing = false;
        lastDash = Time.timeSinceLevelLoad;
        rb.velocity = baseSpeed * speedModifier * direction;
    }

    public void mousePosition(InputAction.CallbackContext context)
    {
        mousePos = context.ReadValue<Vector2>();
    }

    public void ChangeMode(InputAction.CallbackContext context)
    {
        meleeMode = !meleeMode;
        meleeAttacking = false;
        rangedAttacking = false;
        waitingForAttackPerformed = false;
    }


    public void OnAimController(InputAction.CallbackContext context)
    {
        if (usingController)
        {
            aimInput = context.ReadValue<Vector2>();
            if (aimInput.magnitude > 0)
            {
                rangedAttacking = true;
            }
            else
            {
                rangedAttacking = false;
            }
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!usingController)
        {
            rangedAttacking = true;
        }
    }

    public void OffAim(InputAction.CallbackContext context)
    {
        if (!usingController)
        {
            rangedAttacking = false;
        }
    }

    public void OnMeleeAttack(InputAction.CallbackContext context)
    {
        if (!waitingForAttackPerformed)
        {
            meleeAttacking = true;
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rangedAttacking = true;
        }
        else if (context.canceled)
        {
            rangedAttacking = false;
        }
    }
    private void setLevelsToStat()
    {
        Strenght.pc = this;
        foreach (var item in Strenght.damageProgression)
        {
            item.level = Strenght.damageProgression.IndexOf(item) + 1;
        }
        foreach (var item in Strenght.knockBackProgression)
        {
            item.level = Strenght.knockBackProgression.IndexOf(item) + 1;
        }
        //foreach (var item in Strenght.stunTimeProgression)
        //{
        //    item.level = Strenght.stunTimeProgression.IndexOf(item) + 1;
        //}

        Speed.pc = this;
        foreach (var item in Speed.speedProgression)
        {
            item.level = Speed.speedProgression.IndexOf(item) + 1;
        }
        foreach (var item in Speed.dashRecoverProgression)
        {
            item.level = Speed.dashRecoverProgression.IndexOf(item) + 1;
        }
        foreach (var item in Speed.attackRateProgression)
        {
            item.level = Speed.attackRateProgression.IndexOf(item) + 1;
        }

        Aim.pc = this;
        foreach (var item in Aim.fireRateProgression)
        {
            item.level = Aim.fireRateProgression.IndexOf(item) + 1;
        }
        foreach (var item in Aim.fireDamageProgression)
        {
            item.level = Aim.fireDamageProgression.IndexOf(item) + 1;
        }
        foreach (var item in Aim.projectileSpeedProgression)
        {
            item.level = Aim.projectileSpeedProgression.IndexOf(item) + 1;
        }

        Constitution.pc = this;
        foreach (var item in Constitution.healthProgression)
        {
            item.level = Constitution.healthProgression.IndexOf(item) + 1;
        }
        foreach (var item in Constitution.illResistanceProgression)
        {
            item.level = Constitution.illResistanceProgression.IndexOf(item) + 1;
        }
        foreach (var item in Constitution.illGainRateProgression)
        {
            item.level = Constitution.illGainRateProgression.IndexOf(item) + 1;
        }
        //foreach (var item in Constitution.corruptionResistanceProgression)
        //{
        //    item.level = Constitution.corruptionResistanceProgression.IndexOf(item) + 1;
        //}

        Luck.pc = this;
        //foreach (var item in Resolve.merchantSalesProgression)
        //{
        //    item.level = Resolve.merchantSalesProgression.IndexOf(item) + 1;
        //}
        foreach (var item in Luck.critChanceProgression)
        {
            item.level = Luck.critChanceProgression.IndexOf(item) + 1;
        }
    }

}

[System.Serializable]
public class stat
{
    public string name;
    [Range(1, 5)]
    public int livello = 1;
    [TextArea]
    public string description;
    public Sprite image;
}

[System.Serializable]
public class StatValueDictionaryEntry
{
    [MyReadOnly]
    public int level;
    public float value;
}

[System.Serializable]
public class statStrenght : stat
{
    //The Combat statistic influences your damage and the speed of your shots.
    public List<StatValueDictionaryEntry> damageProgression;
    public List<StatValueDictionaryEntry> knockBackProgression;
    //public List<StatValueDictionaryEntry> stunTimeProgression;
    [HideInInspector]
    public PlayerController pc;

    public void setStat()
    {
        pc.meleeKnockbackModifier = pc.Strenght.knockBackProgression.Find(f => f.level == pc.Strenght.livello).value;
        pc.meleeDamageModifier =  pc.Strenght.damageProgression.Find(f => f.level == pc.Strenght.livello).value;
        //pc.stunTime = pc.Strenght.stunTimeProgression.Find(f => f.level == pc.Strenght.livello).value;
    }
}

[System.Serializable]
public class statSpeed : stat
{
    public List<StatValueDictionaryEntry> speedProgression;
    public List<StatValueDictionaryEntry> attackRateProgression;
    public List<StatValueDictionaryEntry> dashRecoverProgression;
    [HideInInspector]
    public PlayerController pc;

    public void setStat()
    {
        pc.baseSpeed = pc.Speed.speedProgression.Find(f => f.level == pc.Speed.livello).value;
        pc.dashRecoveryTime = pc.Speed.dashRecoverProgression.Find(f => f.level == pc.Speed.livello).value;
        pc.meleeRecoveryTime = pc.Speed.attackRateProgression.Find(f => f.level == pc.Speed.livello).value;
    }
}

[System.Serializable]
public class statAim : stat
{
    public List<StatValueDictionaryEntry> fireRateProgression;
    public List<StatValueDictionaryEntry> fireDamageProgression;
    public List<StatValueDictionaryEntry> projectileSpeedProgression;
    [HideInInspector]
    public PlayerController pc;


    public void setStat()
    {
        pc.rangedDamage = pc.Aim.fireDamageProgression.Find(f => f.level == pc.Aim.livello).value;
        pc.rangedAttackCooldown = pc.Aim.fireRateProgression.Find(f => f.level == pc.Aim.livello).value;
        pc.projectileSpeed = pc.Aim.projectileSpeedProgression.Find(f => f.level == pc.Aim.livello).value;
    }
}

[System.Serializable]
public class statConstitution : stat
{
    public List<StatValueDictionaryEntry> healthProgression;
    public List<StatValueDictionaryEntry> illGainRateProgression;
    public List<StatValueDictionaryEntry> illResistanceProgression;
    //public List<StatValueDictionaryEntry> corruptionResistanceProgression;
    [HideInInspector]
    public PlayerController pc;


    public void setStat()
    {
        if (pc.GetComponent<Damageable>())
            pc.GetComponent<Damageable>().SetMaxHealthBar(pc.Constitution.healthProgression.Find(f => f.level == pc.Constitution.livello).value, true);
        else
            Debug.LogError("Non c'è il damageable");

        if (pc.GetComponent<MalattiaHandler>())
        {
            pc.GetComponent<MalattiaHandler>().malattiaGainPerSecond = pc.Constitution.illGainRateProgression.Find(f => f.level == pc.Constitution.livello).value;
            if (pc.Constitution.livello > 1)
            {

            }
            else
            {
                DamageModifier illResistance = new DamageModifier();
                illResistance.tipo = DamageType.DamageTypes.Malattia;
                illResistance.value = pc.Constitution.illResistanceProgression.Find(f => f.level == pc.Constitution.livello).value;
                pc.GetComponent<MalattiaHandler>().AddResistance(illResistance, true);
            }
            
            //pc.GetComponent<MalattiaHandler>().corruptionResistance = pc.Constitution.corruptionResistanceProgression.Find(f => f.level == pc.Constitution.livello).value;
        }
        else
            Debug.LogError("Non c'è il malattiaHandler");
    }
}

[System.Serializable]
public class statLuck : stat
{
    public List<StatValueDictionaryEntry> critChanceProgression;
    [HideInInspector]
    public PlayerController pc;


    public void setStat()
    {
        //pc.merchantSales = pc.Resolve.merchantSalesProgression.Find(f => f.level == pc.Resolve.livello).value;
        pc.critChance = pc.Luck.critChanceProgression.Find(f => f.level == pc.Luck.livello).value;
    }
}

