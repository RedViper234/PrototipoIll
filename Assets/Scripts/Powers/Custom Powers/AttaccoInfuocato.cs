using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttaccoInfuocato : APowers, IStatusApplier
{
    [field: Header("Status")]
    [field: SerializeField] GameObject statusPrefab { get; set; }
    [field: SerializeField] DamageType.DamageTypes statusType { get; set; }
    [field: Range(0f, 1f)] [field: SerializeField] float chance { get; set; }

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

        StatusApplierExtensions.ApplyChanceRandomizer(this, value, chance, statusPrefab, statusType);
    }

    protected override void CustomTriggerEvent()
    {
    }

    protected override void Evolution()
    {
        
    }
}