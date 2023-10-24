using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlloMalattiaManager : MonoBehaviour
{
    //dovr‡ essere inserita la lista di mutazioni e la mutazione attuale
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
    public Vector2 illResistanceMortalityVariation;
    public int applyMortalityToEnemySet(int totalEnemy, TipologiaEnemySet tipo)
    {
        if (tipo == TipologiaEnemySet.Umani || tipo == TipologiaEnemySet.Bestie)
        {
            return totalEnemy;
        }

        float range = spawnMortalityVariation.y - spawnMortalityVariation.x;
        totalEnemy = Mathf.RoundToInt(spawnMortalityVariation.y - ((range / 100) * actualValue));
        return totalEnemy;
    }

    public void timeIllVulnerabilityOrResistance(MalattiaHandler handler)
    {
        float range = illResistanceMortalityVariation.y - illResistanceMortalityVariation.x;
        float value = Mathf.RoundToInt(illResistanceMortalityVariation.y - ((range / 100) * actualValue)); //si usa y- perchË il rapporto Ë inversamente proporzionale (all'aumentare della barra peggiora il value)
        DamageModifier modifier = new();
        modifier.value = value;
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

    public List<AreaSpawnStandardProbability> calcolaProbabilit‡SpawnStandardCombat(List<AreaSpawnStandardProbability> prob)
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
    public Vector2 AlterationStartMutation;

    public float mutaOndata(float areaMutationVariation)
    {
        float range = ProbabilityMutation.y - ProbabilityMutation.x;
        float value = Mathf.RoundToInt(ProbabilityMutation.x + ((range / 100) * actualValue)); //si usa x- perchË il rapporto Ë direttamente proporzionale (all'aumentare della barra aumenta il value)
        value = Mathf.Clamp(value + areaMutationVariation, 0, 100);
        return MathF.Round(value, 1);
    }
    public float rarit‡Mutazione(float mutationBaseValue, float min, float max)
    {
        float range = AlterationStartMutation.y - AlterationStartMutation.x;
        float value = Mathf.RoundToInt(AlterationStartMutation.x + ((range / 100) * actualValue));
        value = Mathf.Clamp(value + mutationBaseValue, min, max);
        return Mathf.Round(value);
    }
}
[System.Serializable]
public class Resistance : MalattiaStat
{

}
