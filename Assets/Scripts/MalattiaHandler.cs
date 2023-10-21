using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalattiaHandler : MonoBehaviour
{
    [Header("Guadagno di malattia")]
    public float malattiaGainPerSecond = 0.5f;
    [Range(0,100)]
    public float currentMalattia = 0;
    public bool stopTimeMalattiaGain = false;

    [Header("Corruzione")]
    public float currentCorruzione = 0;
    [Range (0,1)]

    private PlayerController player = null;

    [Header("Resistenze ai danni")]
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



    private List<coroutineDiCuraMalattia> healOverTimeCoroutine = new();
    private List<coroutineDiDannoMalattia> IllOverTimeCoroutine = new();
    IEnumerator Start()
    {
        if (GetComponent<PlayerController>())
        {
            player = GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogError("Hai messo il malattia handler a qualcosa che non è il player, sei un criminale");
        }
        // Esegui il metodo LateStart una volta, un frame dopo Start
        //malattiaMultiplier = defaultMalattiaMultiplier;
        yield return new WaitForEndOfFrame();
        updateIllBar();
        TakeDamageOverTime(malattiaGainPerSecond, DamageType.DamageTypes.Time, 0);
    }


    #region Damage
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

    private float CalcolaModificatoreDanno(DamageType.DamageTypes tipoDanno)
    {
        float modificatoreBase = 1;
        float maxResistenzaAdditive = 0;
        float maxResistenzaStatic = 0;
        float resCap = 0;
        if (additiveDamageResistances.Find(f => f.tipo == tipoDanno) != null)
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
    
    private void ReceiveDamage(float damage, bool ignoreImmunity, DamageType.DamageTypes tipo)
    {
        if (player != null)
        {
            if (!player.isActuallyImmune() || ignoreImmunity)
            {
                float effectiveDamage = damage * CalcolaModificatoreDanno(tipo);

                switch (tipo)
                {
                    case DamageType.DamageTypes.Malattia:
                        currentMalattia = Mathf.Min(currentMalattia + (int)effectiveDamage, 100);
                        break;
                    case DamageType.DamageTypes.Corruzione:
                        currentCorruzione = Mathf.Min(currentCorruzione + (int)effectiveDamage, 100);
                        currentMalattia = Math.Max(currentMalattia, currentCorruzione);
                        break;
                    default:
                        Debug.LogError("Tipo di danno non valido nel damageable(" + tipo + ")");
                        break;
                }

                if (currentMalattia >= 100 && GetComponent<PlayerController>())
                {
                    player.PlayerDeath();
                }
                // Update health bar/UI here
                updateIllBar();
            }
        }
        else
        {
            Debug.LogError("Coff coff, il malattia handler è su qualcosa che non è il player, risolvi subito");
        }
    }
    
    private void TakeDamageOverTime(float illPerSecond, DamageType.DamageTypes tipo, float duration)
    {
        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopTakeIll(tipo);
        coroutineDiDannoMalattia nuovoDanno = new();
        nuovoDanno.IllOverTimeCoroutine = StartCoroutine(TakeIllOverTimeCoroutine(illPerSecond, tipo, duration));
        nuovoDanno.damagePerSecond = illPerSecond;
        nuovoDanno.tipo = tipo;
        IllOverTimeCoroutine.Add(nuovoDanno);
    }

    private IEnumerator TakeIllOverTimeCoroutine(float illPerSecond, DamageType.DamageTypes tipo, float duration)
    {
        float damageAmount = 0.1f * illPerSecond;

        if (duration > 0)
        {
            while (duration > 0)
            {
                if (tipo != DamageType.DamageTypes.Time || (tipo == DamageType.DamageTypes.Time && !stopTimeMalattiaGain))
                {
                    ReceiveDamage(damageAmount,true, tipo);

                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    updateIllBar();
                }
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }
            StopTakeIll(tipo);
        }
        else
        {
            while (true)
            {
                if (tipo != DamageType.DamageTypes.Time || (tipo == DamageType.DamageTypes.Time && !stopTimeMalattiaGain))
                {
                    ReceiveDamage(damageAmount, true, tipo);

                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    updateIllBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }


    }
    
    public void StopTakeIll(DamageType.DamageTypes tipo)
    {
        foreach (var cor in IllOverTimeCoroutine)
        {
            if (cor.tipo == tipo)
            {
                StopCoroutine(cor.IllOverTimeCoroutine);
            }
        }
        IllOverTimeCoroutine.RemoveAll(f => f.tipo == tipo);
    }

    public void StopAllTakeIll()
    {
        foreach (var cor in IllOverTimeCoroutine)
        {
            if (cor.tipo != DamageType.DamageTypes.Time)
            {
                StopCoroutine(cor.IllOverTimeCoroutine);
            }
        }
        IllOverTimeCoroutine.RemoveAll(f => f.tipo != DamageType.DamageTypes.Time);
    }

    #endregion

    #region Heal
    public bool TakeHeal(List<HealInstance> heals)
    {
        heals = HealInstance.removeZeroHealInstance(heals);
        if (heals.Count == 0 || heals == null)
        {
            return false;
        }
        foreach (var Heal in heals)
        {
            if (Heal.healOverTime)
            {
                TakeHealOverTime(Heal.healValueSingleOrSec, Heal.type, Heal.durationHealOverTime);
            }
            else
            {
                ReceiveHeal(Heal.healValueSingleOrSec, Heal.type);
            }
        }
        return true;
    }

    public bool TakeHeal(HealInstance Heal)
    {
        if (Heal.healValueSingleOrSec <= 0)
        {
            return false;
        }

        if (Heal.healOverTime)
        {
            TakeHealOverTime(Heal.healValueSingleOrSec, Heal.type, Heal.durationHealOverTime);
        }
        else
        {
            ReceiveHeal(Heal.healValueSingleOrSec, Heal.type);
        }

        return true;
    }

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

    private void ReceiveHeal(float heal, HealType.HealTypes tipo)
    {
        if (player != null)
        {
            float effectiveHeal = heal * CalcolaModificatoreCura(tipo);

            switch (tipo)
            {
                case HealType.HealTypes.Malattia:
                    currentMalattia = Mathf.Max(currentMalattia - (int)effectiveHeal, 0);
                    break;
                case HealType.HealTypes.Corruzione:
                    currentCorruzione = Mathf.Min(currentCorruzione - (int)effectiveHeal, 0);
                    break;
                default:
                    Debug.LogError("Tipo di danno non valido nel malattiaHandler(" + tipo + ")");
                    break;
            }
            if (currentMalattia >= 100 && GetComponent<PlayerController>())
            {
                player.PlayerDeath();
            }
            // Update health bar/UI here
            updateIllBar();

        }
        else
        {
            Debug.LogError("Coff coff, il malattia handler è su qualcosa che non è il player, risolvi subito");
        }
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

    public void StopAllHealing()
    {
        foreach (var cor in healOverTimeCoroutine)
        {
            StopCoroutine(cor.healOverTimeCoroutine);            
        }
        healOverTimeCoroutine.Clear();
    }

    public void TakeHealOverTime(float healPerSecond, HealType.HealTypes tipo, float durata)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopHealing(tipo);
        coroutineDiCuraMalattia nuovaCura = new();
        nuovaCura.healOverTimeCoroutine = StartCoroutine(HealOverTimeCoroutine(healPerSecond, tipo, durata));
        nuovaCura.healPerSecond = healPerSecond;
        nuovaCura.tipo = tipo;
        healOverTimeCoroutine.Add(nuovaCura);
    }

    private IEnumerator HealOverTimeCoroutine(float healPerSecond, HealType.HealTypes tipo, float durata)
    {
        float healAmount = 0.1f * healPerSecond;

        if (durata > 0)
        {
            while (durata > 0)
            {
                if (currentMalattia > 0)
                {
                    currentMalattia = Mathf.Max(currentMalattia - healAmount, 0);

                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    updateIllBar();
                }
                yield return new WaitForSeconds(0.1f);
                durata -= 0.1f;
            }
            StopHealing(tipo);
        }
        else
        {
            while (true)
            {
                if (currentMalattia > 0)
                {
                    currentMalattia = Mathf.Max(currentMalattia - healAmount, 0);

                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    updateIllBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }


        // Assicurati di reimpostare healOverTimeCoroutine a null quando la coroutine è terminata.
    }

    #endregion

    #region Damage Resistances and Vulnerability
    public void AddResistance(DamageModifier modifier, bool additive)
    {
        if (additive)
        {
            if (additiveDamageResistances.Find(f => f.tipo == modifier.tipo) != null)
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

    #region Heal Increase and Resistances
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

    #region Update bar
    public void updateIllBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentMalattia).ToString(), currentMalattia, UpdateUiBar.barType.MalattiaBar));
        Publisher.Publish(new UpdateUiBar(((int)currentCorruzione).ToString(), currentCorruzione, UpdateUiBar.barType.CorruzioneBar));
    }
    #endregion

    #region Roba Commentata
    //public float malattiaMultiplier = 1f;
    //public float defaultMalattiaMultiplier = 1f;
    //public float MalattiaMultiplier = 1f;
    //public float multiplierCap = 10f;
    //[Range(0,1)]
    //public float currentmalattiaMultiplierIncrease = 1f;
    //public float multiplyDurationBeforeReduction = 1f;
    //public float multiplyReductionPerSec = 1f;
    //public float multiplyReductionTickPerSec = 1f;
    //private float multiplyTimer;
    //private Coroutine reduceMultiplier;


    //[Header("Progress bar")]
    //public int MalattiaLevel = 0;
    //public int MalattiaPoints = 0;
    //public List<ProgressBarValueDictionaryEntry> MalattiaProgression = new();

    //public int GuarigioneLevel = 0;
    //public int GuarigionePoints = 0;
    //public List<ProgressBarValueDictionaryEntry> GuarigioneProgression = new();


    //private void HanldeMultiplier()
    //{
    //    if (multiplyTimer > 0f)
    //    {
    //        multiplyTimer -= Time.deltaTime;
    //        if (reduceMultiplier != null)
    //        {
    //            StopCoroutine(reduceMultiplier);
    //            reduceMultiplier = null;
    //        }
    //    }
    //    else
    //    {
    //        if (reduceMultiplier == null && malattiaMultiplier > defaultMalattiaMultiplier)
    //        {
    //            reduceMultiplier = StartCoroutine(reduceMultiplierMalattia());
    //        }
    //    }
    //}

    //public IEnumerator reduceMultiplierMalattia()
    //{
    //    float reduction = multiplyReductionPerSec / multiplyReductionTickPerSec;
    //    while(malattiaMultiplier > defaultMalattiaMultiplier)
    //    {
    //        malattiaMultiplier = MathF.Max(defaultMalattiaMultiplier, malattiaMultiplier - reduction);
    //        updateIllMultiplier();
    //        yield return new WaitForSeconds(1/multiplyReductionTickPerSec);
    //    }
    //    reduceMultiplier = null;
    //}

    //private void HandleImmunity()
    //{
    //    if (immunityTimer > 0f)
    //    {
    //        // Handle immunity logic, e.g., making the player flash
    //        immunityTimer -= Time.deltaTime;
    //    }
    //}

    #endregion

}

[System.Serializable]
public class ProgressBarValueDictionaryEntry
{
    public int level;
    public float expRequired;
}
public class coroutineDiCuraMalattia
{
    public Coroutine healOverTimeCoroutine;
    public HealType.HealTypes tipo;
    public float healPerSecond;
}
public class coroutineDiDannoMalattia
{
    public Coroutine IllOverTimeCoroutine;
    public DamageType.DamageTypes tipo;
    public float damagePerSecond;
}