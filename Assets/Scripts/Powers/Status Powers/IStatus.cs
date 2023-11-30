using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatus
{
    public float effectDuration {get; set;}
    public int ticks {get; set;}
    public float damage {get; set;}
    public DamageType.DamageTypes statusType {get; set;}
}
