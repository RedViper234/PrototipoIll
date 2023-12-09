using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlloMalattiaManager : MonoBehaviour
{
    //dovr� essere inserita la lista di mutazioni e la mutazione attuale
    public Mortality mortality;
    public Infectivity infectivity;
    public Mutability mutability;
    public Resistance resistance;
}

[System.Serializable]
public class MalattiaStat
{
    public string name;
    public string description;
    public int baseValue = 50;
    public int actualValue = 0;
}
[System.Serializable]
public class Mortality : MalattiaStat
{
    public Vector2 spawnMortalityVariation;
    public Vector2 illResistance;
    public int applyMortalityToEnemySet(int totalEnemy, TipologiaEnemySet tipo)
    {
        if (tipo == TipologiaEnemySet.Umani || tipo == TipologiaEnemySet.Bestie)
        {
            return totalEnemy;
        }

        float range = spawnMortalityVariation.y - spawnMortalityVariation.x;
        float percentage = Mathf.RoundToInt(spawnMortalityVariation.x + ((range / 100) * actualValue));
        totalEnemy += Mathf.RoundToInt(((float)totalEnemy / 100) * percentage);
        return totalEnemy;
        //ciao
    }

    public void timeIllVulnerabilityOrResistance(MalattiaHandler handler)
    {
        float range = illResistance.y - illResistance.x;
        float value = Mathf.RoundToInt(illResistance.y - ((range / 100) * actualValue)); //si usa y- perch� il rapporto � inversamente proporzionale (all'aumentare della barra peggiora il value)
        DamageModifier modifier = new();
        modifier.value = MathF.Abs(value);
        modifier.tipo = DamageType.DamageTypes.Time;
        if (value > 0)
        {
            handler.AddResistance(modifier, true);
        }
        else if (value < 0)
        {
            handler.AddVulnerability(modifier, true);
        }
    }
}
[System.Serializable]
public class Infectivity : MalattiaStat
{
    public Vector2 spawnProbabilityInfectivity;

    public List<AreaSpawnStandardProbability> calcolaProbabilitaSpawnStandardCombat(List<AreaSpawnStandardProbability> prob)
    {
        float range = spawnProbabilityInfectivity.y - spawnProbabilityInfectivity.x;
        float CureValue = Mathf.RoundToInt(spawnProbabilityInfectivity.y - ((range / 100) * actualValue));
        if (CureValue == 0)
            return prob;
        if (prob.Find(f=>f.tipo == TipologiaEnemySet.Umani) != null && prob.Find(f => f.tipo == TipologiaEnemySet.Infetti) != null)
        {
            float humanPercentage = prob.Find(f => f.tipo == TipologiaEnemySet.Umani).percentualValue;
            float variation = (humanPercentage / 100) * CureValue;
            prob.Find(f => f.tipo == TipologiaEnemySet.Umani).percentualValue += variation;
            prob.Find(f => f.tipo == TipologiaEnemySet.Infetti).percentualValue -= variation;
        }
        if (prob.Find(f => f.tipo == TipologiaEnemySet.Bestie) != null && prob.Find(f => f.tipo == TipologiaEnemySet.BestieInfette) != null)
        {
            float beastPercentage = prob.Find(f => f.tipo == TipologiaEnemySet.Bestie).percentualValue;
            float variation = (beastPercentage / 100) * CureValue;
            prob.Find(f => f.tipo == TipologiaEnemySet.Bestie).percentualValue += variation;
            prob.Find(f => f.tipo == TipologiaEnemySet.BestieInfette).percentualValue -= variation;
        }
        return prob;
    }

}
[System.Serializable]
public class Mutability : MalattiaStat
{
    public Vector2 ProbabilityMutation;
    public Vector2 QualityStartMutation;

    public float mutaOndata(float areaMutationVariation)
    {
        float range = ProbabilityMutation.y - ProbabilityMutation.x;
        float value = Mathf.RoundToInt(ProbabilityMutation.x + ((range / 100) * actualValue)); //si usa x- perch� il rapporto � direttamente proporzionale (all'aumentare della barra aumenta il value)
        value = Mathf.Clamp(value + areaMutationVariation, 0, 100);
        return MathF.Round(value, 1);
    }
    public float raritaMutazione(float mutationBaseValue, float min, float max)
    {
        float range = QualityStartMutation.y - QualityStartMutation.x;
        float value = Mathf.RoundToInt(QualityStartMutation.x + ((range / 100) * actualValue));
        value = Mathf.Clamp(value + mutationBaseValue, min, max);
        return Mathf.Round(value);
    }
}
[System.Serializable]
public class Resistance : MalattiaStat
{
    public Vector2 fireResistance;
    public Vector2 corruptionIncreaseResistance;

    public void FireVulnerabilityOrResistance(Damageable damager)
    {
        float range = fireResistance.y - fireResistance.x;
        float value = Mathf.RoundToInt(fireResistance.x + ((range / 100) * actualValue));
        DamageModifier modifier = new();
        modifier.value = MathF.Abs(value);
        modifier.tipo = DamageType.DamageTypes.Fuoco;
        if (value > 0)
        {
            damager.AddResistance(modifier, true);
        }
        else if (value < 0)
        {
            damager.AddVulnerability(modifier, true);
        }
    }

    public void CorruptionHealing(MalattiaHandler handler)
    {
        float range = fireResistance.y - fireResistance.x;
        float value = Mathf.RoundToInt(fireResistance.x + ((range / 100) * actualValue));
        HealModifier modifier = new();
        modifier.value = MathF.Abs(value);
        modifier.tipo = HealType.HealTypes.Corruzione;
        if (value > 0)
        {
            handler.AddHealIncrease(modifier, true);
        }
        else if (value < 0)
        {
            handler.AddHealResistance(modifier, true);
        }
    }
}
