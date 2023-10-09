using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Actions/RunAwayFromObject", menuName = "Nemici/Actions/RunAwayFromObject")]
public class RunAwayFromObject : Action
{
    public float fleeDistance = 5f;
    public float distanzaDaPercorrereFuoriArea = 2;
    private Transform playerTransform;
    public override void Act(StateMachineController controller)
    {
        if (playerTransform == null)
        {
            return;
        }

        Vector3 directionFromPlayer = controller.transform.position - playerTransform.position;
        float currentDistance = directionFromPlayer.magnitude;

        if (currentDistance < fleeDistance)
        {
            Vector3 fleePosition = controller.transform.position + directionFromPlayer.normalized * distanzaDaPercorrereFuoriArea;
            controller.currentEnemy.currentAgent.SetDestination(fleePosition);
        }
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        if (playerTransform == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controller.transform.position, playerTransform.position);
        Gizmos.DrawWireSphere(playerTransform.position, fleeDistance);
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        playerTransform = controller.currentEnemy.target;
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        playerTransform = null;
    }
}
