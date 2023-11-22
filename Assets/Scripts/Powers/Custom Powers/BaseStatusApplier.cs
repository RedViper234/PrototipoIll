using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatusApplier: APowers
{
    [Header("Status")]
    [SerializeField] protected GameObject statusPrefab;
    [SerializeField] protected DamageType.DamageTypes statusType;
    [SerializeField, Range(0f, 1f)] protected float chance;

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
    }

    protected void ApplyChanceRandomizer(GameObject value)
    {
        if (chance == 1f || Random.Range(0f, 1f) < chance)
        {
            ApplyStatus(value);
        }
    }

    protected virtual void ApplyStatus(GameObject gameObject)
    {
        Debug.Log("Apply Status");
        var inst_Status = Instantiate(statusPrefab, gameObject.transform);
    }

    protected override void CustomTriggerEvent()
    {
    }
}