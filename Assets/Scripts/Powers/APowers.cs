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
    None,
    Sacred, 
    Fire, 
    Research, 
    Depths,
    Infection, 
    Power, 
    Transformation,
    Regeneration
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
    OnPlayerHit,
    OnPlayerDie,
    OnEnemyHit,
    OnEnemyDie,
    OnEndOfDash,
}

public enum Evolution
{
    Base,
    Evo
}

public enum PowerTag
{
    Condizione_Infuocato,
    Attacco_Infuocato,
    Attacco_Speciale_Infuocato,
    Scatto_Infuocato,
    Resistenza_alle_Fiamme,
    Sacra_Fiamma,
    Finale_Esplosivo,
    Espiazione,
    Scia_di_Fiamma,
    Avatar_di_Fiamme,
    Spargere_la_Fiamma,
    Purificazione,
    Fiamme_Potenti,
    Bombe,
    Pianto_del_Dio_del_Fuoco,
    Lanciafiamme
}

public abstract class APowers : MonoBehaviour
{
    [field: Header("Power")]
    [field: SerializeField] public PowerType powerType { get; set;}
    [field: SerializeField] public PowerSubType powerSubType { get; set; }
    [field: SerializeField] public Rarity rarity { get; set;}
    [field: SerializeField] public TriggerType triggerType { get; set; }
    [field: SerializeField] public PowerState powerState { get; set; }
    [field: SerializeField] public Evolution evolution { get; set; }
    
    [field: SerializeField] public int NRooms { get; set; }
    [field: SerializeField] public float NTime { get; set; }

    [field: SerializeField] public PowerTag powerTag { get; set; }
    public abstract void TriggerOnEvent();
    public abstract void TriggerOnEvent(int value);
    public abstract void TriggerOnEvent(float value);
    public abstract void TriggerOnEvent(GameObject value);
    protected abstract void CustomTriggerEvent();

    void Start()
    {
        InitChangePowerType();

        SetTrigger();//TO DELETE
    }

    public virtual void InitChangePowerType()
    {
        if ((int)powerSubType >= 0 && (int)powerSubType < 4)
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

    public void SetTrigger()
    {
        switch (triggerType)
        {
            case TriggerType.Instant:
                TriggerOnEvent();
                break;
            case TriggerType.OnTrigger:
                CustomTriggerEvent();
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
            case TriggerType.OnPlayerHit:
                EventManager.HandleOnPlayerHit += TriggerOnEvent;
                break;
            case TriggerType.OnPlayerDie:
                EventManager.HandleOnPlayerDie += TriggerOnEvent;
                break;
            case TriggerType.OnEnemyHit:
                EventManager.HandleOnEnemyHit += TriggerOnEvent;
                break;
            case TriggerType.OnEnemyDie:
                EventManager.HandleOnEnemyDie += TriggerOnEvent;
                break;
            case TriggerType.OnEndOfDash:
                EventManager.HandleEndOfDash += TriggerOnEvent;
                break;
            default:
                Debug.LogError("Trigger type not found or not implemented: " + triggerType);
                break;
        }
    }

    protected virtual void Evolution()
    {
        
    }
}