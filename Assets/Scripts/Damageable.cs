using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth = 100;
    public float ustioniAccumulate = 0;

    [Header("Resitenze ai danni")]
    public List<DamageModifier> additiveDamageResistances;
    public List<DamageModifier> staticDamageResistances;
    public List<DamageModifier> additiveDamageVulnerabilities;
    public List<DamageModifier> staticDamageVulnerabilities;
    public List<DamageModifier> ResistanceDamageCap; //solo additive
    public List<DamageModifier> VulnerabilityDamageCap; //solo additive
    public bool damageImmunity = false;

    [Header("Debolezze ai danni")]
    public List<HealModifier> additiveHealIncrease;
    public List<HealModifier> staticHealIncrease;
    public List<HealModifier> additiveHealResistances;
    public List<HealModifier> staticHealResistances;
    public List<HealModifier> increaseHealCap; //solo additive
    public List<HealModifier> resistancesHealCap; //solo additive

    private List<coroutineDiCura> healOverTimeCoroutine = new();
    private List<coroutineDiDanno> damageOverTimeCoroutine = new();
    private PlayerController player = null;

    private void Awake()
    {
        if (GetComponent<PlayerController>()) //se è il player lascio che sia la statistica ad impostare la vita
        {
            maxHealth = 0;
            currentHealth = 0;
            player = GetComponent<PlayerController>();
        }
    }

    IEnumerator Start()
    {
        //currentHealth = maxHealth;
        yield return null;

        SetMaxHealthBar(0, true);
    }

    #region Damage Resistance and Vulnerability
    public void AddResistence(DamageModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveDamageResistances.Find(f=>f.tipo == modifier.tipo) != null)
            {
                additiveDamageResistances.Find(f => f.tipo == modifier.tipo).value += modifier.value;
            }
            else
            {
                additiveDamageResistances.Add(modifier);
            }
        }
        else
        {
            staticDamageResistances.Add(modifier);
        }
    }

    public void AddVulnerability(DamageModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveDamageVulnerabilities.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveDamageVulnerabilities.Find(f => f.tipo == modifier.tipo).value += modifier.value;
            }
            else
            {
                additiveDamageVulnerabilities.Add(modifier);
            }
        }
        else
        {
            staticDamageVulnerabilities.Add(modifier);
        }
    }

    public bool RemoveResistance(DamageModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveDamageResistances.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveDamageResistances.Find(f => f.tipo == modifier.tipo).value -= modifier.value;
                if (additiveDamageResistances.Find(f => f.tipo == modifier.tipo).value <= 0)
                {
                    additiveDamageResistances.Remove(additiveDamageResistances.Find(f => f.tipo == modifier.tipo));
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (staticDamageResistances.Find(f => f.tipo == modifier.tipo && f.value == modifier.value) != null)
            {
                staticDamageResistances.Remove(staticDamageResistances.Find(f => f.tipo == modifier.tipo && f.value == modifier.value));
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool RemoveVulnerability(DamageModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveDamageVulnerabilities.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveDamageVulnerabilities.Find(f => f.tipo == modifier.tipo).value -= modifier.value;
                if (additiveDamageVulnerabilities.Find(f => f.tipo == modifier.tipo).value <= 0)
                {
                    additiveDamageVulnerabilities.Remove(additiveDamageVulnerabilities.Find(f => f.tipo == modifier.tipo));
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (staticDamageVulnerabilities.Find(f => f.tipo == modifier.tipo && f.value == modifier.value) != null)
            {
                staticDamageVulnerabilities.Remove(staticDamageVulnerabilities.Find(f => f.tipo == modifier.tipo && f.value == modifier.value));
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region Heal Increase and Resistance
    public void AddHealResistance(HealModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveHealResistances.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveHealResistances.Find(f => f.tipo == modifier.tipo).value += modifier.value;
            }
            else
            {
                additiveHealResistances.Add(modifier);
            }
        }
        else
        {
            staticHealResistances.Add(modifier);
        }
    }

    public void AddHealIncrease(HealModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveHealIncrease.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveHealIncrease.Find(f => f.tipo == modifier.tipo).value += modifier.value;
            }
            else
            {
                additiveHealIncrease.Add(modifier);
            }
        }
        else
        {
            staticHealIncrease.Add(modifier);
        }
    }

    public bool RemoveHealResistance(HealModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveHealResistances.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveHealResistances.Find(f => f.tipo == modifier.tipo).value -= modifier.value;
                if (additiveHealResistances.Find(f => f.tipo == modifier.tipo).value <= 0)
                {
                    additiveHealResistances.Remove(additiveHealResistances.Find(f => f.tipo == modifier.tipo));
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (staticHealResistances.Find(f => f.tipo == modifier.tipo && f.value == modifier.value) != null)
            {
                staticHealResistances.Remove(staticHealResistances.Find(f => f.tipo == modifier.tipo && f.value == modifier.value));
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool RemoveHealIncrease(HealModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveHealIncrease.Find(f => f.tipo == modifier.tipo) != null)
            {
                additiveHealIncrease.Find(f => f.tipo == modifier.tipo).value -= modifier.value;
                if (additiveHealIncrease.Find(f => f.tipo == modifier.tipo).value <= 0)
                {
                    additiveHealIncrease.Remove(additiveHealIncrease.Find(f => f.tipo == modifier.tipo));
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (staticHealIncrease.Find(f => f.tipo == modifier.tipo && f.value == modifier.value) != null)
            {
                staticHealIncrease.Remove(staticHealIncrease.Find(f => f.tipo == modifier.tipo && f.value == modifier.value));
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region setHealth

    public void SetCurrentHealthBar()
    {
        if (GetComponent<PlayerController>())
        {
            Publisher.Publish(new UpdateHealthBar(((int)currentHealth).ToString(), maxHealth.ToString(), (currentHealth * 100) / maxHealth));
        }
    }
    public void SetMaxHealthBar(float value, bool additive)
    {
        if (additive)
        {
            maxHealth = Mathf.Max(maxHealth + (int)value, 0);
            currentHealth = Mathf.Max(currentHealth + (int)value, 0);
        }
        else
        {
            maxHealth = (int)value;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        SetCurrentHealthBar();
    }

    #endregion

    #region Damage
    private float CalcolaModificatoreDanno(DamageType.DamageTypes tipoDanno)
    {
        float modificatoreBase = 1;
        float maxResistenzaAdditive = 0;
        float maxResistenzaStatic = 0;
        float resCap = 0;
        if (additiveDamageResistances.Find(f=>f.tipo == tipoDanno) != null)
        {
            maxResistenzaAdditive = additiveDamageResistances.Find(f => f.tipo == tipoDanno).value;
        }
        if (staticDamageResistances.Find(f => f.tipo == tipoDanno) != null)
        {
            maxResistenzaStatic = staticDamageResistances.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        if (ResistanceDamageCap.Find(f => f.tipo == tipoDanno) != null)
        {
            resCap = ResistanceDamageCap.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        float resistence = Math.Max((resCap != 0 ? Math.Min(maxResistenzaAdditive, resCap) : maxResistenzaAdditive), maxResistenzaStatic);

        float maxVulnAdditive = 0;
        float maxVulnStatic = 0;
        float vulnCap = 0;
        if (additiveDamageVulnerabilities.Find(f => f.tipo == tipoDanno) != null)
        {
            maxVulnAdditive = additiveDamageVulnerabilities.Find(f => f.tipo == tipoDanno).value;
        }
        if (staticDamageVulnerabilities.Find(f => f.tipo == tipoDanno) != null)
        {
            maxVulnStatic = staticDamageVulnerabilities.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        if (VulnerabilityDamageCap.Find(f => f.tipo == tipoDanno) != null)
        {
            vulnCap = VulnerabilityDamageCap.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        float vuln = Math.Max((vulnCap != 0 ? Math.Min(maxVulnAdditive, vulnCap) : maxVulnAdditive), maxVulnStatic);
        return (modificatoreBase - resistence + vuln);
    }
    
    public bool TakeDamage(DamageInstance damage)
    {
        if (damage.damageValueAtkOrSec <= 0)
        {
            return false;
        }
        if (damage.damageOverTime)
        {
            TakeDamageOverTime(damage.damageValueAtkOrSec, damage.type, damage.durationDamageOverTime);
        }
        else
        {
            ReceiveDamage(damage.damageValueAtkOrSec, damage.ignoreImmunityFrame, damage.type);
        }
        return true;
    }

    public bool TakeDamage(List<DamageInstance> damages)
    {
        damages = DamageInstance.removeZeroDamageInstance(damages);
        if (damages.Count == 0 || damages == null)
        {
            return false;
        }
        foreach (var damage in damages)
        {
            if (damage.damageOverTime)
            {
                TakeDamageOverTime(damage.damageValueAtkOrSec, damage.type, damage.durationDamageOverTime);
            }
            else
            {
                ReceiveDamage(damage.damageValueAtkOrSec, damage.ignoreImmunityFrame, damage.type);
            }
        }
        return true;
    }

    private void ReceiveDamage(float damage, bool ignoreImmunity, DamageType.DamageTypes tipo)
    {
        if (player != null)
        {
            if (!player.isActuallyImmune() || ignoreImmunity)
            {
                float effectiveDamage = damage * CalcolaModificatoreDanno(tipo);
                bool somethingChanged = false;
                switch (tipo)
                {
                    case DamageType.DamageTypes.Fisico:
                        int tmp = (int)currentHealth;
                        currentHealth = Mathf.Max(currentHealth - effectiveDamage, 0);
                        somethingChanged = ((int)currentHealth != tmp ? true : false);
                        break;
                    case DamageType.DamageTypes.Ustioni:
                        int tmpU = (int)ustioniAccumulate;
                        ustioniAccumulate += effectiveDamage;
                        ustioniAccumulate = Mathf.Min(maxHealth, ustioniAccumulate);
                        somethingChanged = ((int)ustioniAccumulate != tmpU ? true : false);
                        if (currentHealth > maxHealth - ustioniAccumulate)
                        {
                            currentHealth = maxHealth - ustioniAccumulate;
                            somethingChanged = true;
                        }
                        break;
                    case DamageType.DamageTypes.Fuoco:
                        int tmp1 = (int)currentHealth;
                        currentHealth = Mathf.Max(currentHealth - effectiveDamage, 0);
                        somethingChanged = ((int)currentHealth != tmp1 ? true : false);
                        ReceiveDamage(damage/3, true, DamageType.DamageTypes.Ustioni);
                        break;
                    default:
                        Debug.LogError("Tipo di danno non valido nel damageable("+tipo+")");
                        break;
                }

                if (currentHealth <= 0 && GetComponent<PlayerController>())
                {
                    player.PlayerDeath();
                }
                // Update health bar/UI here
                if (somethingChanged)
                {
                    SetCurrentHealthBar();
                }
            }
        }
        else
        {
            if (!damageImmunity || ignoreImmunity)
            {
                float effectiveDamage = damage * CalcolaModificatoreDanno(tipo);
                currentHealth = Mathf.Max(currentHealth - (int)effectiveDamage, 0);
                if (currentHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void StopTakeDamage(DamageType.DamageTypes tipo)
    {
        foreach (var cor in damageOverTimeCoroutine)
        {
            if (cor.tipo == tipo)
            {
                StopCoroutine(cor.damageOverTimeCoroutine);
            }
        }
        damageOverTimeCoroutine.RemoveAll(f => f.tipo == tipo);
    }

    private void TakeDamageOverTime(float damagePerSecond, DamageType.DamageTypes tipo, float duration)
    {
        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopTakeDamage(tipo);
        coroutineDiDanno nuovoDanno = new();
        nuovoDanno.damageOverTimeCoroutine = StartCoroutine(TakeDamageOverTimeCoroutine(damagePerSecond, duration, tipo));
        nuovoDanno.damagePerSecond = damagePerSecond;
        nuovoDanno.tipo = tipo;
        damageOverTimeCoroutine.Add(nuovoDanno);
    }

    private IEnumerator TakeDamageOverTimeCoroutine(float damagePerSecond, float duration, DamageType.DamageTypes tipo)
    {
        float damageAmount = 0.1f * damagePerSecond;
        if (duration > 0)
        {
            while (duration > 0)
            {
                if (currentHealth > 0)
                {
                    //currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
                    ReceiveDamage(damageAmount, true, tipo);
                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    //SetCurrentHealthBar();
                }
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }
            StopTakeDamage(tipo);
        }
        else
        {
            while (true)
            {
                if (currentHealth > 0)
                {
                    //currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
                    ReceiveDamage(damageAmount, true, tipo);
                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    //SetCurrentHealthBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
    #endregion

    #region Healing
    private float CalcolaModificatoreCura(HealType.HealTypes tipoDanno)
    {
        float modificatoreBase = 1;
        float maxResistenzaAdditive = 0;
        float maxResistenzaStatic = 0;
        float resCap = 0;
        if (additiveHealResistances.Find(f => f.tipo == tipoDanno) != null)
        {
            maxResistenzaAdditive = additiveHealResistances.Find(f => f.tipo == tipoDanno).value;
        }
        if (staticHealResistances.Find(f => f.tipo == tipoDanno) != null)
        {
            maxResistenzaStatic = staticHealResistances.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        if (resistancesHealCap.Find(f => f.tipo == tipoDanno) != null)
        {
            resCap = resistancesHealCap.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        float resistence = Math.Max((resCap != 0 ? Math.Min(maxResistenzaAdditive, resCap) : maxResistenzaAdditive), maxResistenzaStatic);

        float maxIncreaseAdditive = 0;
        float maxIncreaseStatic = 0;
        float increaseCap = 0;
        if (additiveHealIncrease.Find(f => f.tipo == tipoDanno) != null)
        {
            maxIncreaseAdditive = additiveHealIncrease.Find(f => f.tipo == tipoDanno).value;
        }
        if (staticHealIncrease.Find(f => f.tipo == tipoDanno) != null)
        {
            maxIncreaseStatic = staticHealIncrease.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        if (increaseHealCap.Find(f => f.tipo == tipoDanno) != null)
        {
            increaseCap = increaseHealCap.Where(f => f.tipo == tipoDanno).Max(f => f.value);
        }
        float increase = Math.Max((increaseCap != 0 ? Math.Min(maxIncreaseAdditive, increaseCap) : maxIncreaseAdditive), maxIncreaseStatic);
        return (modificatoreBase - resistence + increase);
    }

    public bool TakeHealing(HealInstance heal)
    {
        if (heal.healValueSingleOrSec <= 0)
        {
            return false;
        }

        if (heal.healOverTime)
        {
            HealOverTime(heal.healValueSingleOrSec, heal.durationHealOverTime, heal.type);
        }
        else
        {
            ReceiveHealing(heal.healValueSingleOrSec, heal.type);
        }

        return true;
    }

    public bool TakeHealing(List<HealInstance> heals)
    {
        heals = HealInstance.removeZeroHealInstance(heals);
        if (heals.Count == 0 || heals == null)
        {
            return false;
        }
        foreach (var heal in heals)
        {
            if (heal.healOverTime)
            {
                HealOverTime(heal.healValueSingleOrSec, heal.durationHealOverTime, heal.type);
            }
            else
            {
                ReceiveHealing(heal.healValueSingleOrSec, heal.type);
            }
        }
        return true;
    }
    
    private void ReceiveHealing(float heal, HealType.HealTypes tipo)
    {
        if (player != null)
        {
                float effectiveHealing = heal * CalcolaModificatoreCura(tipo);

                switch (tipo)
                {
                    case HealType.HealTypes.Fisico:
                        currentHealth = Mathf.Min(currentHealth + (int)effectiveHealing, maxHealth-ustioniAccumulate);
                        break;
                    case HealType.HealTypes.Ustioni:
                        ustioniAccumulate = Mathf.Max(0, ustioniAccumulate - (int)effectiveHealing);
                        break;
                    default:
                        Debug.LogError("Tipo di danno non valido nel damageable(" + tipo + ")");
                        break;
                }
                // Update health bar/UI here
                SetCurrentHealthBar();            
        }
        else
        {
            float effectiveHealing = heal * CalcolaModificatoreCura(tipo);
            currentHealth = Mathf.Min(currentHealth + (int)effectiveHealing, maxHealth);
        }
    }

    public void FullHealing()
    {
        currentHealth = maxHealth - ustioniAccumulate;
        SetCurrentHealthBar();
    }

    public void StopHealing(HealType.HealTypes tipo)
    {
        foreach (var cor in healOverTimeCoroutine)
        {
            if (cor.tipo == tipo)
            {
                StopCoroutine(cor.healOverTimeCoroutine);
            }
        }
        healOverTimeCoroutine.RemoveAll(f => f.tipo == tipo);
    }

    public void HealOverTime(float healPerSecond, float duration, HealType.HealTypes tipo)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopHealing(tipo);
        coroutineDiCura nuovaCura = new();
        nuovaCura.healOverTimeCoroutine = StartCoroutine(HealOverTimeCoroutine(healPerSecond, duration, tipo));
        nuovaCura.healPerSecond = healPerSecond;
        nuovaCura.tipo = tipo;
        healOverTimeCoroutine.Add(nuovaCura);
    }

    private IEnumerator HealOverTimeCoroutine(float healPerSecond, float duration, HealType.HealTypes tipo)
    {
        float healAmount = 0.1f * healPerSecond;

        if (duration > 0)
        {
            while (duration > 0)
            {
                if (currentHealth < maxHealth)
                {
                    //currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
                    ReceiveHealing(healAmount, tipo);
                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    //SetCurrentHealthBar();
                }
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }
            StopHealing(tipo);
        }
        else
        {
            while (true)
            {
                if (currentHealth < maxHealth)
                {
                    //currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
                    ReceiveHealing(healAmount, tipo);
                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    //SetCurrentHealthBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    #endregion
}

public class coroutineDiCura
{
    public Coroutine healOverTimeCoroutine;
    public HealType.HealTypes tipo;
    public float healPerSecond;
}
public class coroutineDiDanno
{
    public Coroutine damageOverTimeCoroutine;
    public DamageType.DamageTypes tipo;
    public float damagePerSecond;
}
