using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth = 100;
    public float ustioniAccumulate = 0;
    public float damageResistance = 0;
    public float ustioniResistance = 0;
    public bool damageImmunity = false;
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

    void Start()
    {
        //currentHealth = maxHealth;
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        // Attendi un frame
        yield return null;

        SetMaxHealthBar(0, true);        
    }

    public void TakeDamage(float damage, bool ignoreImmunity, bool inflictUstioni)
    {
        if (player != null)
        {
            if (!player.isActuallyImmune() || ignoreImmunity)
            {
                if (!inflictUstioni)
                {
                    float effectiveDamage = damage * (1f - ustioniResistance);
                    currentHealth = Mathf.Max(currentHealth - (int)effectiveDamage, 0);
                }
                else
                {
                    float effectiveDamage = damage * (1f - damageResistance);
                    ustioniAccumulate += Mathf.Min(currentHealth, effectiveDamage);
                }

                if (currentHealth <= 0 && !GetComponent<PlayerController>())
                {
                    Destroy(gameObject);
                }
                else if (currentHealth <= 0 && GetComponent<PlayerController>())
                {
                    player.PlayerDeath();
                }
                // Update health bar/UI here

                SetCurrentHealthBar();
            }
        }
        else
        {
            if (!damageImmunity || ignoreImmunity)
            {
                float effectiveDamage = damage * (1f - damageResistance);
                currentHealth = Mathf.Max(currentHealth - (int)effectiveDamage, 0);
                Destroy(gameObject);
            }
        }
    }

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

    public void TakeDamageOverTime(float damagePerSecond, DamageType.DamageTypes tipo, float duration, bool inflictUstioni)
    {
        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopTakeDamage(tipo);
        coroutineDiDanno nuovoDanno = new();
        nuovoDanno.damageOverTimeCoroutine = StartCoroutine(TakeDamageOverTimeCoroutine(damagePerSecond, duration, tipo, inflictUstioni));
        nuovoDanno.damagePerSecond = damagePerSecond;
        nuovoDanno.tipo = tipo;
        damageOverTimeCoroutine.Add(nuovoDanno);
    }

    private IEnumerator TakeDamageOverTimeCoroutine(float damagePerSecond, float duration, DamageType.DamageTypes tipo, bool inflictUstioni)
    {
        float damageAmount = 0.1f * damagePerSecond;
        if (duration > 0)
        {
            while (duration > 0)
            {
                if (currentHealth > 0)
                {
                    //currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
                    TakeDamage(damageAmount, true, inflictUstioni);
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
                    TakeDamage(damageAmount, true, inflictUstioni);
                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    //SetCurrentHealthBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

    }

    public void Healing(float heal, bool ustioni)
    {
        if (ustioni)
        {
            ustioniAccumulate = (ustioniAccumulate - (int)heal > 0 ? ustioniAccumulate - (int)heal : 0);
            SetCurrentHealthBar();
        }
        else
        {
            currentHealth = (currentHealth + (int)heal > maxHealth - ustioniAccumulate ? maxHealth - ustioniAccumulate : currentHealth + (int)heal);
            SetCurrentHealthBar();
        }

    }

    public void FullHealing()
    {
        currentHealth = maxHealth - ustioniAccumulate;
        SetCurrentHealthBar();
    }

    public void StopHealing(tipiDiCure tipo)
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

    public void HealOverTime(float healPerSecond, float duration, tipiDiCure tipo, bool ustioni)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopHealing(tipo);
        coroutineDiCura nuovaCura = new();
        nuovaCura.healOverTimeCoroutine = StartCoroutine(HealOverTimeCoroutine(healPerSecond, duration, tipo, ustioni));
        nuovaCura.healPerSecond = healPerSecond;
        nuovaCura.tipo = tipo;
        healOverTimeCoroutine.Add(nuovaCura);
    }

    private IEnumerator HealOverTimeCoroutine(float healPerSecond, float duration, tipiDiCure tipo, bool ustioni)
    {
        float healAmount = 0.1f * healPerSecond;

        if (duration > 0)
        {
            while (duration > 0)
            {
                if (currentHealth < maxHealth)
                {
                    //currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
                    Healing(healAmount, ustioni);
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
                    Healing(healAmount, ustioni);
                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    //SetCurrentHealthBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public enum tipiDiCure
    {
        miasma,
    }
}

public class coroutineDiCura
{
    public Coroutine healOverTimeCoroutine;
    public Damageable.tipiDiCure tipo;
    public float healPerSecond;
}
public class coroutineDiDanno
{
    public Coroutine damageOverTimeCoroutine;
    public DamageType.DamageTypes tipo;
    public float damagePerSecond;
}
