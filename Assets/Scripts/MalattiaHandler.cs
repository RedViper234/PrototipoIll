using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalattiaHandler : MonoBehaviour
{
    [Header("Guadagno di malattia")]
    public float malattiaGainPerSecond = 0.5f;
    [Range(0,1)]
    public float malattiaResistance = 0;
    public bool malattiaImmunity = false;
    public float currentMalattia = 0;
    private int maxPercMalattia = 100;
    public bool stopTimeMalattiaGain = false;
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

    [Header("Guadagno di corruzione")]
    public float currentCorruzione = 0;
    [Range (0,1)]
    public float corruptionResistance = 0;

    private PlayerController player = null;

    //[Header("Progress bar")]
    //public int MalattiaLevel = 0;
    //public int MalattiaPoints = 0;
    //public List<ProgressBarValueDictionaryEntry> MalattiaProgression = new();

    //public int GuarigioneLevel = 0;
    //public int GuarigionePoints = 0;
    //public List<ProgressBarValueDictionaryEntry> GuarigioneProgression = new();


    private List<coroutineDiCuraMalattia> healOverTimeCoroutine = new();
    private List<coroutineDiDannoMalattia> IllOverTimeCoroutine = new();
    void Start()
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
        StartCoroutine(LateStart());
        //malattiaMultiplier = defaultMalattiaMultiplier;
    }

    IEnumerator LateStart()
    {
        // Attendi un frame
        yield return null;
        //updateIllMultiplier();
        //modifyMalattiaPoints(0);
        //modifyGuarigionePoints(0);
        updateIllBar();
        updateCorruptionBar();
        TakeIllOverTime(malattiaGainPerSecond, DamageType.DamageTypes.Time, 0);
    }
    void Update()
    {
        //HandleImmunity();
        //HanldeMultiplier();
    }

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

    public void updateIllBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentMalattia).ToString(), currentMalattia, UpdateUiBar.barType.MalattiaBar));
    }

    public void updateCorruptionBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentCorruzione).ToString(), currentCorruzione, UpdateUiBar.barType.CorruzioneBar));
    }

    //public void updateIllMultiplier()
    //{
    //    Publisher.Publish(new UpdateUiBar((Math.Round(malattiaMultiplier, 1)).ToString() , 0, UpdateUiBar.barType.MalattiaMultiplier));
    //}

    public void gainMalattia(float percValue, bool ignoreImmunity)
    {
        percValue = MathF.Abs(percValue);
        if (player != null)
        {
            if (!player.isActuallyImmune() || ignoreImmunity)
            {
                currentMalattia = Mathf.Clamp(currentMalattia + (percValue /** malattiaMultiplier*/ * (1 - malattiaResistance)), 0, 100);
                if (currentMalattia >= 100)
                {
                    player.PlayerDeath();
                }
                // Update health bar/UI here

                updateIllBar();

            }
        }
    }

    public void loseMalattia(float percValue)
    {
        percValue = MathF.Abs(percValue);
        float temp = Mathf.Clamp(currentMalattia - percValue, 0, 100);
        currentMalattia = MathF.Max(temp, currentCorruzione);
        updateIllBar();
    }

    public void gainCorruption(float percValue, bool ignoreImmunity)
    {
        percValue = MathF.Abs(percValue);
        if (player != null)
        {
            if (!player.isActuallyImmune() || ignoreImmunity)
            {
                currentCorruzione = Mathf.Clamp(currentCorruzione + (percValue * (1 - corruptionResistance)), 0, 100);
                if (currentMalattia < currentCorruzione)
                {
                    gainMalattia(currentCorruzione - currentMalattia, ignoreImmunity);
                }
                if (!ignoreImmunity)
                {
                    player.setImmunity();
                }
                updateCorruptionBar();
            }
        }
    }

    public void loseCorruption(float percValue)
    {
        percValue = MathF.Abs(percValue);
        currentCorruzione = Mathf.Clamp(currentCorruzione - percValue, 0, 100);
        updateCorruptionBar();
    }

    //private void modifyGuarigionePoints(int expPoints)
    //{
    //    GuarigionePoints = Math.Max(GuarigionePoints + expPoints, 0);
    //    for (int i = 0; i < GuarigioneProgression.Count; i++)
    //    {
    //        if (GuarigionePoints < GuarigioneProgression[i].expRequired)
    //        {
    //            GuarigioneLevel = GuarigioneProgression[i - 1].level;
    //            updateProgressBar(GuarigioneLevel.ToString(), ((GuarigionePoints - GuarigioneProgression[i - 1].expRequired) * 100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired), UpdateUiBar.barType.GuarigioneProgressBar);
    //            //Debug.Log(((GuarigionePoints - GuarigioneProgression[i - 1].expRequired)*100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired));//(( / 100f) * ();
    //            return;
    //        }
    //    }
    //    GuarigioneLevel = GuarigioneProgression[GuarigioneProgression.Count - 1].level;
    //    updateProgressBar("Max", 100, UpdateUiBar.barType.GuarigioneProgressBar);
    //}

    private void updateProgressBar(string text, float perc, UpdateUiBar.barType tipo)
    {
        Publisher.Publish(new UpdateUiBar(text, perc, tipo));
    }

    //private void modifyMalattiaPoints(int expPoints)
    //{
    //    MalattiaPoints = Math.Max(MalattiaPoints + expPoints, 0);
    //    for (int i = 1; i < MalattiaProgression.Count; i++)
    //    {
    //        if (MalattiaPoints < MalattiaProgression[i].expRequired)
    //        {
    //            MalattiaLevel = MalattiaProgression[i - 1].level;
    //            updateProgressBar(MalattiaLevel.ToString(), ((MalattiaPoints - MalattiaProgression[i - 1].expRequired) * 100) / (MalattiaProgression[i].expRequired - MalattiaProgression[i - 1].expRequired), UpdateUiBar.barType.MalattiaProgressBar);
    //            return;
    //        }
    //    }
    //    MalattiaLevel = MalattiaProgression[MalattiaProgression.Count - 1].level;
    //    updateProgressBar("Max", 100, UpdateUiBar.barType.MalattiaProgressBar);
    //}

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

    public void TakeIllOverTime(float illPerSecond, DamageType.DamageTypes tipo, float duration)
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
                if (currentMalattia < maxPercMalattia && tipo != DamageType.DamageTypes.Time || currentMalattia < maxPercMalattia && tipo == DamageType.DamageTypes.Time && !stopTimeMalattiaGain)
                {
                    currentMalattia = Mathf.Min(currentMalattia + (damageAmount * (1 - malattiaResistance)), maxPercMalattia);

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
                if (currentMalattia < maxPercMalattia && tipo != DamageType.DamageTypes.Time || currentMalattia < maxPercMalattia && tipo == DamageType.DamageTypes.Time && !stopTimeMalattiaGain)
                {
                    currentMalattia = Mathf.Min(currentMalattia + (damageAmount * (1 - malattiaResistance)), maxPercMalattia);

                    // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                    updateIllBar();
                }
                yield return new WaitForSeconds(0.1f);
            }
        }


    }

    public void HealingIll(float heal)
    {
        currentMalattia = (currentMalattia - (int)heal < 0 ? 0 : currentMalattia - (int)heal);
        updateIllBar();
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

    public void StopAllHealing()
    {
        foreach (var cor in healOverTimeCoroutine)
        {
            StopCoroutine(cor.healOverTimeCoroutine);            
        }
        healOverTimeCoroutine.Clear();
    }

    public void HealOverTime(float healPerSecond, tipiDiCure tipo, float durata)
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

    private IEnumerator HealOverTimeCoroutine(float healPerSecond, tipiDiCure tipo, float durata)
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

    public enum tipiDiCure
    {
        siero,
    }
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
    public MalattiaHandler.tipiDiCure tipo;
    public float healPerSecond;
}
public class coroutineDiDannoMalattia
{
    public Coroutine IllOverTimeCoroutine;
    public DamageType.DamageTypes tipo;
    public float damagePerSecond;
}