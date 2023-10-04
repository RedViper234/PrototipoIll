using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Decisioni/IsInAreaDecision", menuName = "Nemici/Decisioni/IsInAreaDecision")]
public class IsInAreaDecision : Decision
{
    public float areaRadious;
    public LayerMask searchMask;

    public override bool Decide(StateMachineController controller)
    {
        EnemyController enemyController = controller.currentEnemy;
        if (enemyController != null)
        {
            RaycastHit2D hit = Physics2D.CircleCast(enemyController.transform.position,areaRadious, Vector2.zero,10,searchMask);
            if(hit.collider != null)
            {
                if(hit.collider.gameObject.GetComponent<PlayerController>() != null) 
                {
                    controller.target = hit.collider.transform;
                    return true;
                }

            }
        }
        return false;
    }

    public override void DrawMyGizmos(StateMachineController controller)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controller.transform.position, areaRadious);
    }
}
