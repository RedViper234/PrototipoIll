using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerListSO", menuName = "Power/NewPowerList")]

public class PowerListSO : ScriptableObject
{
    [SerializeField] List<PowerDictionary> powerList = new List<PowerDictionary>();

    public bool FindObjectByTag(PowerTag powerTagToFind)
    {
        foreach (PowerDictionary power in powerList)
        {
            if (power.powerTag == powerTagToFind)
            {
                return true;
            }
        }

        return false;
    }

    //TODO
    public bool FindObjectByTag(PowerTag[] powerTagToFind)
    {
        foreach (PowerDictionary power in powerList)
        {
            foreach (PowerTag tag in powerTagToFind)
            {
                if (power.powerTag == tag)
                {
                    return true;
                }  
            }
        }

        return false;
    }

    public GameObject ReturnObjectByTag(PowerTag powerTagToFind)
    {
        foreach (PowerDictionary power in powerList)
        {
            if (power.powerTag == powerTagToFind)
            {
                return power.powerPrefab;
            }
        }
        
        return null;
    }
}

[Serializable]
public class PowerDictionary
{
    public PowerTag powerTag;
    public GameObject powerPrefab;
}

