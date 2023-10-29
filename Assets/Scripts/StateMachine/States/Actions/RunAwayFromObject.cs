using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Actions/RunAwayFromObject", menuName = "Nemici/Actions/RunAwayFromObject")]
public class RunAwayFromObject : Action
{
    public float fleeDistance = 5f;
    public float distanzaDaPercorrereFuoriArea = 2;
    public float approachDistance = 10f;
    private Transform playerTransform;
    public override void Act(StateMachineController controller)
    {
        if (playerTransform == null)
        {
            playerTransform = controller.currentEnemy.target;
            return;
        }

        Vector3 directionFromPlayer = controller.transform.position - playerTransform.position;
        float currentDistance = directionFromPlayer.magnitude;

        if (currentDistance < fleeDistance)
        {
            Vector3 fleePosition = controller.transform.position + directionFromPlayer.normalized * distanzaDaPercorrereFuoriArea;
            controller.currentEnemy.currentAgent.SetDestination(fleePosition);
        }
        else if (currentDistance >= approachDistance)
        {
            Vector3 approachPosition = playerTransform.position + directionFromPlayer.normalized * approachDistance;
            controller.currentEnemy.currentAgent.SetDestination(approachPosition);
        }
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        if (playerTransform == null)
        {
            playerTransform = controller.currentEnemy.target;
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(controller.transform.position, playerTransform.position);
        Gizmos.DrawWireSphere(playerTransform.position, fleeDistance);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(playerTransform.position, approachDistance);

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
