using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageType : MonoBehaviour
{
    public enum DamageTypes
    {
        Fisico,
        Ustioni,
        Malattia,
        Corruzione,
        Time,
    }
}

[System.Serializable]
public class DamageInstance
{
    public DamageType.DamageTypes type;
    public float value;
    public bool ignoreImmunity = false;
    public bool damageOverTime = false;
    public float durationDamageOverTime = 0;

    public DamageInstance(DamageType.DamageTypes type, float value, bool ignoreImmunity, bool damageOverTime, float durationDamageOverTime)
    {
        this.type = type;
        this.value = value;
        this.ignoreImmunity = ignoreImmunity;
        this.damageOverTime = damageOverTime;
        this.durationDamageOverTime = durationDamageOverTime;
    }

    static public List<DamageInstance> removeZeroDamageInstance(List<DamageInstance> instanceOfDamage)
    {
        return instanceOfDamage.FindAll(f => f.value > 0);
    }
}
