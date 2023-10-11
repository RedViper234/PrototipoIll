using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TestPower : AbstractPowers
{
    public override UnityAction OnPowerTaken { get; set; }
    [field: SerializeField] public override PowerSubType powerSubType { get; set; }
    [field: ReadOnly] public override PowerType powerType { get; set; }
    [field: SerializeField] public override Rarity rarity { get; set; }
    [field: SerializeField] public override TriggerType triggerType { get; set; }
    [field: SerializeField] public override PowerState powerState { get; set; }
    [field: SerializeField] public override Evolution evolution { get; set; }


}