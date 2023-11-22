using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacraFiamma : APowers
{
    public override void TriggerOnEvent()
    {
        foreach (IStatusApplier statusApplier in transform.parent.GetComponentsInChildren<IStatusApplier>())
        {
            _ = statusApplier.chance < 100f ? statusApplier.chance *= 1.5f : statusApplier.chance = 100f;
        }
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
}
