using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttaccoInfuocato : BaseStatusApplier
{
    public override void TriggerOnEvent()
    {  
    }

    public override void TriggerOnEvent(int value)
    {
    }

    public override void TriggerOnEvent(float value)
    {
    }

    public override void TriggerOnEvent(GameObject value)
    {
        Debug.Log("Apply Fire Status");

        ApplyChanceRandomizer(value);

    }

    protected override void CustomTriggerEvent()
    {
    }
}