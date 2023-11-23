using UnityEditor.PackageManager;
using UnityEngine;

public interface IStatusApplier
{
    public GameObject statusPrefab {get; set;}
    public DamageType.DamageTypes statusType {get; set;}
    public float chance {get; set;}
}

public static class StatusApplierExtensions
{
    public static bool ApplyChanceRandomizer(this IStatusApplier statusApplier, float chance)
    {
        if (chance >= 1f || Random.Range(0f, 1f) < chance)
        {
            return true;
        }

        return false;
    }

    public static void ApplyStatus(this IStatusApplier statusApplier, GameObject objectToBeApply, GameObject statusPrefab, DamageType.DamageTypes statusType)
    {
        Debug.Log("Apply status fun");
        var inst_Status = MonoBehaviour.Instantiate(statusPrefab, objectToBeApply.transform);
    }
}