using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScattoInfuocato : APowers, IStatusApplier
{
    [field: Header("Status Applier")]
    [field: SerializeField] public GameObject statusPrefab { get; set; }
    [field: SerializeField]public DamageType.DamageTypes statusType { get; set; }
    [field: SerializeField][field: Range(0f, 1f)]public float chance { get; set; }

    [Header("Other Properties")]
    [SerializeField] private float radius;
    private CircleCollider2D objectCollider;
    
    void Awake()
    {
        objectCollider = GetComponent<CircleCollider2D>();
    }

    public override void TriggerOnEvent()
    {
        objectCollider.enabled = true;
        objectCollider.radius = radius;
        
        StartCoroutine(DisableObjectColliderAfterDelay(0.2f));
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Damageable>() != null)
        {
            if(StatusApplierExtensions.ApplyChanceRandomizer(this, chance)) 
                StatusApplierExtensions.ApplyStatus(this, other.gameObject, statusPrefab, statusType);
        }
    }

    private IEnumerator DisableObjectColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        objectCollider.enabled = false;
    }
}
