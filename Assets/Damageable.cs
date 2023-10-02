using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth = 100;
    public float damageResistance = 0;
    public bool damageImmunity = false;
    public float immunityFrameDuration = 0f;
    private float immunityTimer = 0;
    private List<coroutineDiCura> healOverTimeCoroutine = new();
    private List<coroutineDiDanno> damageOverTimeCoroutine = new();
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HandleImmunity();
    }

    private void HandleImmunity()
    {
        if (immunityTimer > 0f)
        {
            // Handle immunity logic, e.g., making the player flash
            immunityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        if (immunityTimer <= 0f && !damageImmunity)
        {
            float effectiveDamage = damage * (1f - damageResistance);
            currentHealth -= (int)effectiveDamage;
            immunityTimer = immunityFrameDuration;

            // Update health bar/UI here
        }
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

        while (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

            // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.

            yield return new WaitForSeconds(0.1f);
        }

        // Assicurati di reimpostare healOverTimeCoroutine a null quando la coroutine è terminata.
    }

    public void Healing(float heal)
    {
        currentHealth = (currentHealth + (int)heal > maxHealth ? maxHealth : currentHealth + (int)heal);
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

        while (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);

            // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.

            yield return new WaitForSeconds(0.1f);
        }

        // Assicurati di reimpostare healOverTimeCoroutine a null quando la coroutine è terminata.
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
