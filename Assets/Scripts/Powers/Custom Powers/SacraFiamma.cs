using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacraFiamma : APowers
{
    float chanceMultiplayer = 1.5f;

    public override void TriggerOnEvent()
    {
        IncreaseChance();
    }

    public override void TriggerOnEvent(int value)
    {
        
    }

    public override void TriggerOnEvent(float value)
    {
        
    }

    public override void TriggerOnEvent(GameObject value)
    {
        
    }

    protected override void CustomTriggerEvent()
    {
        
    }

    private void IncreaseChance()
    {
        foreach (IStatusApplier statusApplier in transform.parent.GetComponentsInChildren<IStatusApplier>())
        {
            _ = statusApplier.chance < 100f ? statusApplier.chance *= chanceMultiplayer : statusApplier.chance = 100f;
        }
    }

    protected override void Evolution()
    {
        chanceMultiplayer = 2f;
        IncreaseChance();
    }
}
