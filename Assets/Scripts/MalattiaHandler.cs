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

    [Header("Progress bar")]
    public int MalattiaLevel = 0;
    public int MalattiaPoints = 0;
    public List<ProgressBarValueDictionaryEntry> MalattiaProgression = new();

    public int GuarigioneLevel = 0;
    public int GuarigionePoints = 0;
    public List<ProgressBarValueDictionaryEntry> GuarigioneProgression = new();

    [Header("Barra di malattie e corruzione")]
    public int maxPercMalattia = 100;
    public float currentMalattia = 0;

    public float currentCorruzione = 0;

    private List<coroutineDiCuraMalattia> healOverTimeCoroutine = new();
    private List<coroutineDiDannoMalattia> IllOverTimeCoroutine = new();
    void Start()
    {
        // Esegui il metodo LateStart una volta, un frame dopo Start
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        // Attendi un frame
        yield return null;
        modifyMalattiaPoints(0);
        modifyGuarigionePoints(0);
        gainMalattia(0);
        modifyCorruption(0);
        TakeIllOverTime(malattiaGainPerSecond, tipiDiDanno.time);
    }

    public void updateIllBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentMalattia).ToString(), currentMalattia, UpdateUiBar.barType.MalattiaBar));
    }

    public void updateCorruptionBar()
    {
        Publisher.Publish(new UpdateUiBar(((int)currentCorruzione).ToString(), currentCorruzione, UpdateUiBar.barType.CorruzioneBar));
    }

    public void gainMalattia(float percValue)
    {
        currentMalattia = Mathf.Clamp(currentMalattia + (percValue * (1 - malattiaResistance)), 0, 100);
        updateIllBar();
    }

    public void loseMalattia(float percValue)
    {
        currentMalattia = Mathf.Clamp(currentMalattia - percValue, 0, 100);
        updateIllBar();
    }

    public void modifyCorruption(float percValue)
    {
        currentCorruzione = Mathf.Clamp(currentCorruzione + percValue, 0, 100);
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
                Publisher.Publish(new UpdateUiBar(GuarigioneLevel.ToString(), ((GuarigionePoints - GuarigioneProgression[i - 1].expRequired) * 100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired), UpdateUiBar.barType.GuarigioneProgressBar));
                //Debug.Log(((GuarigionePoints - GuarigioneProgression[i - 1].expRequired)*100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired));//(( / 100f) * ();
                return;
            }
        }
        GuarigioneLevel = GuarigioneProgression[GuarigioneProgression.Count - 1].level;
        Publisher.Publish(new UpdateUiBar("Max", 100, UpdateUiBar.barType.GuarigioneProgressBar));
    }

    private void modifyMalattiaPoints(int expPoints)
    {
        MalattiaPoints = Math.Max(MalattiaPoints + expPoints, 0);
        for (int i = 1; i < MalattiaProgression.Count; i++)
        {
            if (MalattiaPoints < MalattiaProgression[i].expRequired)
            {
                MalattiaLevel = MalattiaProgression[i - 1].level;
                Publisher.Publish(new UpdateUiBar(MalattiaLevel.ToString(), ((MalattiaPoints - MalattiaProgression[i - 1].expRequired) * 100) / (MalattiaProgression[i].expRequired - MalattiaProgression[i - 1].expRequired), UpdateUiBar.barType.MalattiaProgressBar));

                //levelMalattia.text = MalattiaLevel.ToString();
                return;
            }
        }
        MalattiaLevel = MalattiaProgression[MalattiaProgression.Count - 1].level;
        Publisher.Publish(new UpdateUiBar("Max", 100, UpdateUiBar.barType.MalattiaProgressBar));

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

    public void TakeIllOverTime(float illPerSecond, tipiDiDanno tipo)
    {

        // Se una coroutine di guarigione è già in esecuzione, la interrompiamo.
        //StopCoroutine(healOverTimeCoroutine);
        StopTakeIll(tipo);
        coroutineDiDannoMalattia nuovoDanno = new();
        nuovoDanno.IllOverTimeCoroutine = StartCoroutine(TakeIllOverTimeCoroutine(illPerSecond));
        nuovoDanno.damagePerSecond = illPerSecond;
        nuovoDanno.tipo = tipo;
        IllOverTimeCoroutine.Add(nuovoDanno);
    }

    private IEnumerator TakeIllOverTimeCoroutine(float illPerSecond)
    {
        float damageAmount = 0.1f * illPerSecond;

        while (true)
        {
            if (currentMalattia < maxPercMalattia)
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