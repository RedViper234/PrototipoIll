using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PowerType 
{
    None,
    Healing,
    Illness
}

public enum PowerSubType 
{
    Sacred, 
    Fire, 
    Research, 
    Infection, 
    Power, 
    Transformation
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
    public UnityAction OnPowerTaken { get; set; }
    public PowerSubType powerSubType { get; set; }
    public PowerType powerType { get; set;}
    public Rarity rarity { get; set;}
    public TriggerType triggerType { get; set; }
    public PowerState powerState { get; set; }
    public Evolution evolution { get; set; }
    public int NRooms { get; set; }
    public float NTime { get; set; }
    public abstract void TriggerOnEvent();
    public abstract void TriggerOnEvent(int value);
    public abstract void TriggerOnEvent(float value);

    void Start()
    {
        InitChangePowerType();

        // OnPowerTaken += PowerTaken;
        // OnPowerTaken.Invoke();
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
        // OnPowerTaken -= PowerTaken;
    }

    public void SetTrigger()
    {
        switch (triggerType)
        {
            case TriggerType.Instant:
                TriggerOnEvent();
                break;
            case TriggerType.OnTrigger:
                // Code for OnTrigger case
                break;
            case TriggerType.OnBeginOfRoom:
                EventManager.HandleBeginRoom += TriggerOnEvent;
                break;
            case TriggerType.OnEndOfRoom:
                EventManager.HandleEndRoom += TriggerOnEvent;
                break;
            case TriggerType.OnBeginOfFight:
                EventManager.HandleBeginOfFight += TriggerOnEvent;
                break;
            case TriggerType.OnEndOfFight:
                EventManager.HandleEndOfFight += TriggerOnEvent;
                break;
            case TriggerType.DuringExploration:
                // Code for DuringExploration case
                break;
            case TriggerType.OnDeath:
                EventManager.HandleDeath += TriggerOnEvent;
                break;
            case TriggerType.OnEveryNRoom:
                EventManager.HandleEveryNRoom += TriggerOnEvent;
                break;
            case TriggerType.OnEveryNTime:
                EventManager.HandleEveryNTime += TriggerOnEvent;
                break;
            default:
                Debug.LogError("Trigger type not found or not implemented: " + triggerType);
                break;
        } 
    }
}