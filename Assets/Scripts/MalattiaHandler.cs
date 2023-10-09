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
    public float malattiaMultiplier = 1f;
    public float defaultMalattiaMultiplier = 1f;
    public float MalattiaMultiplier = 1f;
    public float multiplierCap = 10f;
    [Range(0,1)]
    public float currentmalattiaMultiplierIncrease = 1f;
    public float multiplyDurationBeforeReduction = 1f;
    public float multiplyReductionPerSec = 1f;
    public float multiplyReductionTickPerSec = 1f;
    private float multiplyTimer;
    private Coroutine reduceMultiplier;

    [Header("Guadagno di corruzione")]
    public float currentCorruzione = 0;
    [Range (0,1)]
    public float corruptionResistance = 0;

    [Header("Barra di malattie e corruzione")]
    public float immunityFrameDuration = 0f;
    private float immunityTimer = 0;

    [Header("Progress bar")]
    public int MalattiaLevel = 0;
    public int MalattiaPoints = 0;
    public List<ProgressBarValueDictionaryEntry> MalattiaProgression = new();

    public int GuarigioneLevel = 0;
    public int GuarigionePoints = 0;
    public List<ProgressBarValueDictionaryEntry> GuarigioneProgression = new();


    private List<coroutineDiCuraMalattia> healOverTimeCoroutine = new();
    private List<coroutineDiDannoMalattia> IllOverTimeCoroutine = new();
    void Start()
    {
        // Esegui il metodo LateStart una volta, un frame dopo Start
        StartCoroutine(LateStart());
        malattiaMultiplier = defaultMalattiaMultiplier;
    }

    IEnumerator LateStart()
    {
        // Attendi un frame
        yield return null;
        updateIllMultiplier();
        modifyMalattiaPoints(0);
        modifyGuarigionePoints(0);
        updateIllBar();
        updateCorruptionBar();
        TakeIllOverTime(malattiaGainPerSecond, tipiDiDanno.time);
    }
    void Update()
    {
        HandleImmunity();
        HanldeMultiplier();
    }

    private void HanldeMultiplier()
    {
        if (multiplyTimer > 0f)
        {
            multiplyTimer -= Time.deltaTime;
            if (reduceMultiplier != null)
            {
                StopCoroutine(reduceMultiplier);
                reduceMultiplier = null;
            }
        }
        else
        {
            if (reduceMultiplier == null && malattiaMultiplier > defaultMalattiaMultiplier)
            {
                reduceMultiplier = StartCoroutine(reduceMultiplierMalattia());
            }
        }
    }

    public IEnumerator reduceMultiplierMalattia()
    {
        float reduction = multiplyReductionPerSec / multiplyReductionTickPerSec;
        while(malattiaMultiplier > defaultMalattiaMultiplier)
        {
            malattiaMultiplier = MathF.Max(defaultMalattiaMultiplier, malattiaMultiplier - reduction);
            updateIllMultiplier();
            yield return new WaitForSeconds(1/multiplyReductionTickPerSec);
        }
        reduceMultiplier = null;
    }

    private void HandleImmunity()
    {
        if (immunityTimer > 0f)
        {
            // Handle immunity logic, e.g., making the player flash
            immunityTimer -= Time.deltaTime;
        }
    }

    public void updateIllBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentMalattia).ToString(), currentMalattia, UpdateUiBar.barType.MalattiaBar));
    }

    public void updateCorruptionBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentCorruzione).ToString(), currentCorruzione, UpdateUiBar.barType.CorruzioneBar));
    }

    public void updateIllMultiplier()
    {
        Publisher.Publish(new UpdateUiBar((Math.Round(malattiaMultiplier, 1)).ToString() , 0, UpdateUiBar.barType.MalattiaMultiplier));
    }

    public void gainMalattia(float percValue, bool ignoreImmunity)
    {
        percValue = MathF.Abs(percValue);
        if (immunityTimer <= 0f || ignoreImmunity)
        {
            currentMalattia = Mathf.Clamp(currentMalattia + (percValue * malattiaMultiplier * (1 - malattiaResistance)), 0, 100);
            if (!ignoreImmunity)
            {
                immunityTimer = immunityFrameDuration;
                malattiaMultiplier = Mathf.Min(malattiaMultiplier*(1+ currentmalattiaMultiplierIncrease), multiplierCap);
                multiplyTimer = multiplyDurationBeforeReduction;
                updateIllMultiplier();
            }
            updateIllBar();
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
        if (immunityTimer <= 0f || ignoreImmunity)
        {
            currentCorruzione = Mathf.Clamp(currentCorruzione + (percValue * (1 - corruptionResistance)), 0, 100);
            if (currentMalattia < currentCorruzione)
            {
                currentMalattia = currentCorruzione;
                updateIllBar();
            }
            if (!ignoreImmunity)
            {
                immunityTimer = immunityFrameDuration;
            }
            updateCorruptionBar();
        }
    }

    public void loseCorruption(float percValue)
    {
        percValue = MathF.Abs(percValue);
        currentCorruzione = Mathf.Clamp(currentCorruzione - percValue, 0, 100);
        updateCorruptionBar();
    }

    private void modifyGuarigionePoints(int expPoints)
    {
        GuarigionePoints = Math.Max(GuarigionePoints + expPoints, 0);
        for (int i = 0; i < GuarigioneProgression.Count; i++)
        {
            if (GuarigionePoints < GuarigioneProgression[i].expRequired)
            {
                GuarigioneLevel = GuarigioneProgression[i - 1].level;
                updateProgressBar(GuarigioneLevel.ToString(), ((GuarigionePoints - GuarigioneProgression[i - 1].expRequired) * 100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired), UpdateUiBar.barType.GuarigioneProgressBar);
                //Debug.Log(((GuarigionePoints - GuarigioneProgression[i - 1].expRequired)*100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired));//(( / 100f) * ();
                return;
            }
        }
        GuarigioneLevel = GuarigioneProgression[GuarigioneProgression.Count - 1].level;
        updateProgressBar("Max", 100, UpdateUiBar.barType.GuarigioneProgressBar);
    }

    private void updateProgressBar(string text, float perc, UpdateUiBar.barType tipo)
    {
        Publisher.Publish(new UpdateUiBar(text, perc, tipo));
    }

    private void modifyMalattiaPoints(int expPoints)
    {
        MalattiaPoints = Math.Max(MalattiaPoints + expPoints, 0);
        for (int i = 1; i < MalattiaProgression.Count; i++)
        {
            if (MalattiaPoints < MalattiaProgression[i].expRequired)
            {
                MalattiaLevel = MalattiaProgression[i - 1].level;
                updateProgressBar(MalattiaLevel.ToString(), ((MalattiaPoints - MalattiaProgression[i - 1].expRequired) * 100) / (MalattiaProgression[i].expRequired - MalattiaProgression[i - 1].expRequired), UpdateUiBar.barType.MalattiaProgressBar);
                return;
            }
        }
        MalattiaLevel = MalattiaProgression[MalattiaProgression.Count - 1].level;
        updateProgressBar("Max", 100, UpdateUiBar.barType.MalattiaProgressBar);
    }

    public void StopTakeIll(tipiDiDanno tipo)
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
            if (cor.tipo != tipiDiDanno.time)
            {
                StopCoroutine(cor.IllOverTimeCoroutine);
            }
        }
        IllOverTimeCoroutine.RemoveAll(f => f.tipo != tipiDiDanno.time);
    }

    public void TakeIllOverTime(float illPerSecond, tipiDiDanno tipo)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopTakeIll(tipo);
        coroutineDiDannoMalattia nuovoDanno = new();
        nuovoDanno.IllOverTimeCoroutine = StartCoroutine(TakeIllOverTimeCoroutine(illPerSecond, tipo));
        nuovoDanno.damagePerSecond = illPerSecond;
        nuovoDanno.tipo = tipo;
        IllOverTimeCoroutine.Add(nuovoDanno);
    }

    private IEnumerator TakeIllOverTimeCoroutine(float illPerSecond, tipiDiDanno tipo)
    {
        float damageAmount = 0.1f * illPerSecond;

        while (true)
        {
            if (currentMalattia < maxPercMalattia && tipo != tipiDiDanno.time || currentMalattia < maxPercMalattia && tipo == tipiDiDanno.time && !stopTimeMalattiaGain)
            {
                currentMalattia = Mathf.Min(currentMalattia + (damageAmount * (1 - malattiaResistance)), maxPercMalattia);

                // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                updateIllBar();
            }
            yield return new WaitForSeconds(0.1f);
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

    public void HealOverTime(float healPerSecond, tipiDiCure tipo)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopHealing(tipo);
        coroutineDiCuraMalattia nuovaCura = new();
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
            if (currentMalattia > 0)
            {
                currentMalattia = Mathf.Max(currentMalattia - healAmount, 0);

                // Aggiorna la barra della salute o qualsiasi altra UI qui, se necessario.
                updateIllBar();
            }
            yield return new WaitForSeconds(0.1f);
        }

        // Assicurati di reimpostare healOverTimeCoroutine a null quando la coroutine è terminata.
    }

    public enum tipiDiCure
    {
        siero,
    }

    public enum tipiDiDanno
    {
        time,
        miasma,
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
    public MalattiaHandler.tipiDiDanno tipo;
    public float damagePerSecond;
}