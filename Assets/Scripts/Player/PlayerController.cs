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
    public float meleeDamageModifier = 1f;
    public float meleeKnockbackForce = 10f;
    public float meleeKnockbackDuration = 0.5f;
    public float meleeHitTime = 0.2f;
    public float meleeHitDelay = 0.1f;
    public float meleeRecoveryTime = 0.5f;
    private float lastAttackTime = 0f;
    private bool waintingForAttackPerformed = false;
    private float attackRange = 1f;

    [Header("Ranged Attack")]
    public UnityEngine.GameObject rangedProjectilePrefab;
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
    private InputAction move;
    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (actions == null)
            actions = new PlayerInput();
        actions.Enable();
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
    }

    private void Update()
    {
        HandleMovement();
        HandleMeleeAttack();
        HandleRangedAttack();
        HandleDash();
    }

    private void HandleMovement()
    {
        Vector2 input = movementInput.normalized;
        if (!isDashing)
        {
            if (input.magnitude > 0.1f)
            {
                Vector2 velocity = rb.velocity;
                velocity = Vector2.Lerp(velocity, input * baseSpeed * speedModifier, acceleration * Time.fixedDeltaTime);
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
        if (meleeMode && meleeAttacking && Time.timeSinceLevelLoad - lastAttackTime >= meleeRecoveryTime && !waintingForAttackPerformed)
        {
            // Perform melee attack logic
            waintingForAttackPerformed = true;
            Invoke("attackMelee", meleeHitDelay);
        }
    }

    private void attackMelee()
    {
        lastAttackTime = Time.timeSinceLevelLoad;
        UnityEngine.GameObject attack = Instantiate(meleeAttackPrefab, new Vector3(GetComponent<Transform>().position.x + attackRange * direction.x, GetComponent<Transform>().position.y + attackRange * direction.y, 1), Quaternion.identity);
        // Calcola l'angolo di rotazione in base a direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Crea una rotazione basata sull'angolo calcolato
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        // Applica la rotazione a attack
        attack.transform.rotation = targetRotation;
        meleeAttacking = false;
        waintingForAttackPerformed = false;
        Destroy(attack, meleeHitTime);
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
        if (isDashing)
        {
            Vector2 velocity = rb.velocity;
            velocity = Vector2.Lerp(velocity, direction * dashForce, dashTime * Time.fixedDeltaTime);
            rb.velocity = velocity;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        if (movementInput.magnitude > 0 && !isDashing)
        {
            direction = movementInput;
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (canDash && !isDashing && ((lastDash + dashRecoveryTime < Time.timeSinceLevelLoad) || consecutiveDash < maxConsecutiveDash))
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
    }

    IEnumerator rememberDash()
    {
        float currentTime = 0;
        while (currentTime < dashRememberTime)
        {
            if (canDash && !isDashing && ((lastDash + dashRecoveryTime < Time.timeSinceLevelLoad) || consecutiveDash < maxConsecutiveDash))
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
        waintingForAttackPerformed = false;
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
        if (!waintingForAttackPerformed)
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

//[System.Serializable]
//public class statCombat : stat
//{
//    //The Combat statistic influences your damage and the speed of your shots.
//    public List<StatValueDictionaryEntry> damageProgression;
//    public List<StatValueDictionaryEntry> shootProgression;

//    public void setStat()
//    {
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<CombatStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = BattleManager.Instance.player.Combat.livello.ToString();

//        //float valueDamage = BattleManager.Instance.player.Combat.damageProgression.Find(f => f.level == BattleManager.Instance.player.Combat.livello).value;
//        //BattleManager.Instance.player.Damage = (int)valueDamage;
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<DamageStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = valueDamage.ToString();

//        //float valueShootForce = BattleManager.Instance.player.Combat.shootProgression.Find(f => f.level == BattleManager.Instance.player.Combat.livello).value;
//        //BattleManager.Instance.player.shootForce = valueShootForce;
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<ShootForceStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = valueShootForce.ToString();

//    }
//}

//[System.Serializable]
//public class statSpeed : stat
//{
//    public List<StatValueDictionaryEntry> speedProgression;
//    public List<StatValueDictionaryEntry> fireRateProgression;
//    public List<StatValueDictionaryEntry> jumpForceProgression;

//    public void setStat()
//    {
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<SpeedStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = BattleManager.Instance.player.Speed.livello.ToString();

//        //float valueSpeed = BattleManager.Instance.player.Speed.speedProgression.Find(f => f.level == BattleManager.Instance.player.Speed.livello).value;
//        //BattleManager.Instance.player.maxSpeed = valueSpeed;
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<MovementSpeedStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = valueSpeed.ToString();


//        //float valueFireRate = BattleManager.Instance.player.Speed.fireRateProgression.Find(f => f.level == BattleManager.Instance.player.Speed.livello).value;
//        //BattleManager.Instance.player.fireRate = valueFireRate;
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<FireRateStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = valueFireRate.ToString() + " s";


//        //float valueJump = BattleManager.Instance.player.Speed.jumpForceProgression.Find(f => f.level == BattleManager.Instance.player.Speed.livello).value;
//        //BattleManager.Instance.player.jumpForce = valueJump;
//    }
//}

//[System.Serializable]
//public class statResistence : stat
//{
//    public List<StatValueDictionaryEntry> maxLifeProgression;
//    public List<StatValueDictionaryEntry> immunityTimeProgression;

//    public void setStat()
//    {
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<ResistanceStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = BattleManager.Instance.player.Resistance.livello.ToString();

//        //float valueMaxLife = BattleManager.Instance.player.Resistance.maxLifeProgression.Find(f => f.level == BattleManager.Instance.player.Resistance.livello).value;
//        //float difference = 0;
//        //difference = valueMaxLife - GameManager.Instance.playerLifeMax;
//        //BattleManager.Instance.player.GetComponent<Damageable>().MaxLife = (int)valueMaxLife;
//        //if (livello == 1)
//        //{
//        //    BattleManager.Instance.player.GetComponent<Damageable>().life = BattleManager.Instance.player.GetComponent<Damageable>().MaxLife;
//        //}
//        //else
//        //{
//        //    BattleManager.Instance.player.GetComponent<Damageable>().life += (int)difference;
//        //}
//        //if (BattleManager.Instance.player.GetComponent<Damageable>().life > BattleManager.Instance.player.GetComponent<Damageable>().MaxLife)
//        //{
//        //    BattleManager.Instance.player.GetComponent<Damageable>().life = BattleManager.Instance.player.GetComponent<Damageable>().MaxLife;
//        //}

//        //GameManager.Instance.playerLifeMax = (int)valueMaxLife;
//        //if (livello == 1)
//        //{
//        //    GameManager.Instance.playerLife = GameManager.Instance.playerLifeMax;
//        //}
//        //else
//        //{
//        //    GameManager.Instance.playerLife += (int)difference;
//        //}
//        //if (GameManager.Instance.playerLife > GameManager.Instance.playerLifeMax)
//        //{
//        //    GameManager.Instance.playerLife = GameManager.Instance.playerLifeMax;
//        //}

//        //UIManager.Instance.setPlayerMaxLifeUI();
//        //UIManager.Instance.setPlayerActualLifeUI();
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<HealthStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = valueMaxLife.ToString();


//        //float valueImmunityTime = BattleManager.Instance.player.Resistance.immunityTimeProgression.Find(f => f.level == BattleManager.Instance.player.Resistance.livello).value;
//        //BattleManager.Instance.player.GetComponent<Damageable>().playerImmunityTime = valueImmunityTime;
//        //UIManager.Instance.PlayerStats.GetComponentInChildren<ImmunityStat>(true).GetComponentInChildren<ValueText>(true).GetComponent<TextMeshProUGUI>().text = valueImmunityTime.ToString() + " s"; ;

//    }
//}