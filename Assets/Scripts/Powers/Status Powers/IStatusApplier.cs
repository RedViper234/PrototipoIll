using UnityEngine;

public interface IStatusApplier
{
    public GameObject statusPrefab {get{return statusPrefab;} set{}}
    public DamageType.DamageTypes statusType {get{return statusType;} set{}}
    public float chance {get{return chance;} set{}}
}

public static class StatusApplierExtensions
{
    public static void ApplyChanceRandomizer(this IStatusApplier statusApplier, GameObject objectToBeApply, float chance, GameObject statusPrefab, DamageType.DamageTypes statusType)
    {
        if (chance == 1f || Random.Range(0f, 1f) < chance)
        {
            Debug.Log("Apply status fun");
            var inst_Status = MonoBehaviour.Instantiate(statusPrefab, objectToBeApply.transform);
        }
    }
}