using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttaccoInfuocato : APowers, IStatusApplier
{
    [field: Header("Status Applier")]
    [field: SerializeField]public GameObject statusPrefab { get; set; }
    [field: SerializeField]public DamageType.DamageTypes statusType { get; set; }
    [field: SerializeField][field: Range(0f, 1f)] public float chance { get; set; }
 
    public override void TriggerOnEvent()
    {  
    }

    public override void TriggerOnEvent(int value)
    {
    }

    public override void TriggerOnEvent(float value)
    {
    }

    public override void TriggerOnEvent(GameObject objectToBeApply)
    {
        Debug.Log("Apply Fire Status");

        if(StatusApplierExtensions.ApplyChanceRandomizer(this, chance)) 
            StatusApplierExtensions.ApplyStatus(this, objectToBeApply, statusPrefab, statusType);
    }

    protected override void CustomTriggerEvent()
    {

    }

    protected override void Evolution()
    {
        
    }
}