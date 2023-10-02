using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalattiaHandler : MonoBehaviour
{
    public int MalattiaLevel = 0;
    public int MalattiaPoints = 0;
    public List<ProgressBarValueDictionaryEntry> MalattiaProgression = new();
    public MeshMaskUI MalattiaProgressBar;
    public TextMeshProUGUI levelMalattia;
    public int GuarigioneLevel = 0;
    public int GuarigionePoints = 0;
    public List<ProgressBarValueDictionaryEntry> GuarigioneProgression = new();
    public MeshMaskUI GuarigioneProgressBar;
    public TextMeshProUGUI levelGuarigione;
    public int maxPercMalattia = 100;
    public float currentMalattia = 0;
    [Range(0,1)]
    public float malattiaResistance = 0;
    public bool malattiaImmunity = false;
    public MeshMaskUI IllBar;
    public TextMeshProUGUI percTextIll;
    public float currentCorruzione = 0;
    public MeshMaskUI CorruptionBar;
    public TextMeshProUGUI percTextCorruption;
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
        TakeIllOverTime(1,tipiDiDanno.time);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateIllBar()
    {
        IllBar.setPercentage(currentMalattia);
        percTextIll.text = ((int)currentMalattia).ToString() + "%";
    }
    public void updateCorruptionBar()
    {
        CorruptionBar.setPercentage(currentCorruzione);
        percTextCorruption.text = ((int)currentCorruzione).ToString() + "%";
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
                levelGuarigione.text = GuarigioneLevel.ToString();
                //Debug.Log(((GuarigionePoints - GuarigioneProgression[i - 1].expRequired)*100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired));//(( / 100f) * ();
                GuarigioneProgressBar.setPercentage(((GuarigionePoints - GuarigioneProgression[i - 1].expRequired) * 100) / (GuarigioneProgression[i].expRequired - GuarigioneProgression[i - 1].expRequired));
                return;
            }
        }
        GuarigioneLevel = GuarigioneProgression[GuarigioneProgression.Count - 1].level;
        levelGuarigione.text = "Max";
        GuarigioneProgressBar.setPercentage(100);
    }

    private void modifyMalattiaPoints(int expPoints)
    {
        MalattiaPoints = Math.Max(MalattiaPoints + expPoints, 0);
        for (int i = 1; i < MalattiaProgression.Count; i++)
        {
            if (MalattiaPoints < MalattiaProgression[i].expRequired)
            {
                MalattiaLevel = MalattiaProgression[i - 1].level;
                levelMalattia.text = MalattiaLevel.ToString();
                MalattiaProgressBar.setPercentage(((MalattiaPoints - MalattiaProgression[i - 1].expRequired) * 100) / (MalattiaProgression[i].expRequired - MalattiaProgression[i - 1].expRequired));
                return;
            }
        }
        MalattiaLevel = MalattiaProgression[MalattiaProgression.Count - 1].level;
        levelMalattia.text = "Max";
        MalattiaProgressBar.setPercentage(100);
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

    public void Healing(float heal)
    {
        currentMalattia = (currentMalattia - (int)heal < 0 ? 0 : currentMalattia - (int)heal);
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