using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageType : MonoBehaviour
{
    public enum DamageTypes
    {
        Non_specificato,
        Fisico,
        Fuoco,
        Ustioni,
        Malattia,
        Corruzione,
        Time,
    }
}

public class HealType
{
    public enum HealTypes
    {
        Non_specificato,
        Fisico,
        Ustioni,
        Malattia,
        Corruzione,
    }
}

[System.Serializable]
public class DamageInstance
{
    public DamageType.DamageTypes type;
    public float damageValueAtkOrSec;
    public bool ignoreDamageResistance = false;
    public bool damageOverTime = false;
    public float durationDamageOverTime = 0;
    public bool ignoreImmunityFrame = false;

    public DamageInstance(DamageType.DamageTypes type, float value, bool ignoreImmunity, bool ignoreDamageResistance, bool damageOverTime, float durationDamageOverTime)
    {
        this.type = type;
        this.damageValueAtkOrSec = value;
        this.ignoreImmunityFrame = ignoreImmunity;
        this.ignoreDamageResistance = ignoreDamageResistance;
        this.damageOverTime = damageOverTime;
        this.durationDamageOverTime = durationDamageOverTime;
    }

    static public List<DamageInstance> removeZeroDamageInstance(List<DamageInstance> instanceOfDamage)
    {
        return instanceOfDamage.FindAll(f => f.damageValueAtkOrSec > 0);
    }
}

[System.Serializable]
public class HealInstance
{
    public HealType.HealTypes type;
    public float healValueSingleOrSec;
    public bool healOverTime = false;
    public float durationHealOverTime = 0;

    public HealInstance(HealType.HealTypes type, float value, bool healOverTime, float durationHealOverTime)
    {
        this.type = type;
        this.healValueSingleOrSec = value;
        this.healOverTime = healOverTime;
        this.durationHealOverTime = durationHealOverTime;
    }

    static public List<HealInstance> removeZeroHealInstance(List<HealInstance> instanceOfHeal)
    {
        return instanceOfHeal.FindAll(f => f.healValueSingleOrSec > 0);
    }
}

[System.Serializable]
public class DamageModifier
{
    public DamageType.DamageTypes tipo;
    [Range(0,1)]
    public float value;
}

public class HealModifier
{
    public HealType.HealTypes tipo;
    [Range(0, 1)]
    public float value;
}