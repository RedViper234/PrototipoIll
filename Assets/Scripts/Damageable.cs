using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth = 100;
    public float damageResistance = 0;
    public bool damageImmunity = false;
    private List<coroutineDiCura> healOverTimeCoroutine = new();
    private List<coroutineDiDanno> damageOverTimeCoroutine = new();

    private void Awake()
    {
        if (GetComponent<PlayerController>()) //se è il player lascio che sia la statistica ad impostare la vita
        {
            maxHealth = 0;
            currentHealth = 0;
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

    // Update is called once per frame
    void Update()
    {
        //HandleImmunity();
    }

    //private void HandleImmunity()
    //{
    //    if (immunityTimer > 0f)
    //    {
    //        // Handle immunity logic, e.g., making the player flash
    //        immunityTimer -= Time.deltaTime;
    //    }
    //}

    public bool TakeDamage(float damage)
    {
        if (/*immunityTimer <= 0f && */!damageImmunity)
        {
            float effectiveDamage = damage * (1f - damageResistance);
            currentHealth = Mathf.Max(currentHealth - (int)effectiveDamage, 0);
            //immunityTimer = immunityFrameDuration;

            if (currentHealth <= 0 && !GetComponent<PlayerController>())
            {
                Destroy(gameObject);
            }
            else if (currentHealth <= 0 && GetComponent<PlayerController>())
            {
                PlayerDeath();
            }
            // Update health bar/UI here

            SetCurrentHealthBar();
            
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("sei morto");
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

    public void StopTakeDamage(tipiDiDanno tipo)
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

    public void TakeDamageOverTime(float damagePerSecond, tipiDiDanno tipo)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopTakeDamage(tipo);
        coroutineDiDanno nuovoDanno = new();
        nuovoDanno.damageOverTimeCoroutine = StartCoroutine(TakeDamageOverTimeCoroutine(damagePerSecond));
        nuovoDanno.damagePerSecond = damagePerSecond;
        nuovoDanno.tipo = tipo;
        damageOverTimeCoroutine.Add(nuovoDanno);
    }

    private IEnumerator TakeDamageOverTimeCoroutine(float damagePerSecond)
    {
        float damageAmount = 0.1f * damagePerSecond;

        while (true)
        {
            if (currentHealth > 0)
            {
                currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

                // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                SetCurrentHealthBar();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Healing(float heal)
    {
        currentHealth = (currentHealth + (int)heal > maxHealth ? maxHealth : currentHealth + (int)heal);
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

    public void HealOverTime(float healPerSecond, tipiDiCure tipo)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopHealing(tipo);
        coroutineDiCura nuovaCura = new();
        nuovaCura.healOverTimeCoroutine = StartCoroutine(HealOverTimeCoroutine(healPerSecond));
        nuovaCura.healPerSecond = healPerSecond;
        nuovaCura.tipo = tipo;
        healOverTimeCoroutine.Add(nuovaCura);
    }

    private IEnumerator HealOverTimeCoroutine(float healPerSecond)
    {
        float healAmount = 0.1f * healPerSecond;

        while (true)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);

                // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                SetCurrentHealthBar();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public enum tipiDiCure
    {
        miasma,
    }

    public enum tipiDiDanno
    {
        sanguinamento,
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
    public Damageable.tipiDiDanno tipo;
    public float damagePerSecond;
}
