using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PowerType 
{
    None,
    Welfare,
    Illness
}

public enum PowerSubType 
{
    Purification,
    Defense,
    Survival,
    Infection,
    Destruction,
    Mutation
}

public enum Rarity 
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum PowerState 
{
    On,
    Off
}

public enum TriggerType
{
    Instant,
    OnTrigger,
    OnBeginOfRoom,
    OnEndOfRoom,
    OnBeginOfFight,
    OnEndOfFight,
    DuringExploration,
    OnDeath,
    OnEveryNRoom,
    OnEveryNTime,
}

public enum Evolution
{
    Base,
    Evo
}

public abstract class AbstractPowers : MonoBehaviour
{
    public abstract UnityAction OnPowerTaken { get; set; }
    public abstract PowerSubType powerSubType { get; set; }
    public abstract PowerType powerType { get; set;}
    public abstract Rarity rarity { get; set;}
    public abstract TriggerType triggerType { get; set; }
    public abstract PowerState powerState { get; set; }
    public abstract Evolution evolution { get; set; }

    void Start()
    {
        InitChangePowerType();

        OnPowerTaken += PowerTaken;
        OnPowerTaken.Invoke();
    }

    public virtual void InitChangePowerType()
    {
        if (((int)powerSubType) >= 0 && (int)powerSubType < 3)
        {
            powerType = (PowerType)1;
        }
        else
        {
            powerType = (PowerType)2;
        }
    }

    public virtual void PowerTaken()
    {
        SetTrigger();
    }

    void OnDisable()
    {
        OnPowerTaken -= PowerTaken;
    }

    public void SetTrigger()
    {
        switch (triggerType)
        {
            case TriggerType.Instant:
                // Code for Instant case
                break;
            case TriggerType.OnTrigger:
                // Code for OnTrigger case
                break;
            case TriggerType.OnBeginOfRoom:
                // Code for OnBeginOfRoom case
                break;
            case TriggerType.OnEndOfRoom:
                // Code for OnEndOfRoom case
                break;
            case TriggerType.OnBeginOfFight:
                // Code for OnBeginOfFight case
                break;
            case TriggerType.OnEndOfFight:
                // Code for OnEndOfFight case
                break;
            case TriggerType.DuringExploration:
                // Code for DuringExploration case
                break;
            case TriggerType.OnDeath:
                // Code for OnDeath case
                break;
            case TriggerType.OnEveryNRoom:
                // Code for OnEveryNRoom case
                break;
            case TriggerType.OnEveryNTime:
                // Code for OnEveryNTime case
                break;
            default:
                // Code for default case
                break;
        } 
    }
}
