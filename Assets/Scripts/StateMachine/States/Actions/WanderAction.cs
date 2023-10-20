using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Extensions;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Actions/WanderAction", menuName = "Nemici/Actions/WanderAction")]
public class WanderAction : Action
{
    private float circleRadius = 5f;
    private float lastCallTime = 0f;
    public float intervalMin = 1f; 
    public float intervalMax = 3f;

    private Vector2 randomDestination;
    public override void Act(StateMachineController controller)
    {
        if (controller.currentEnemy.currentAgent.isOnNavMesh)
        {
            float currentTime = Time.time;
            if (currentTime - lastCallTime >= intervalMin && !controller.currentEnemy.currentAgent.pathPending && controller.currentEnemy.currentAgent.remainingDistance <= 0.1f)
            {
                float randomInterval = Random.Range(intervalMin, intervalMax);
                if (currentTime - lastCallTime >= randomInterval)
                {
                    SetRandomDestination(controller);
                    lastCallTime = currentTime;
                }
            }
        }
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(randomDestination, 1f);
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        lastCallTime = Time.time;
        controller.currentEnemy.currentAgent.updateRotation= false;
        controller.currentEnemy.currentAgent.updateUpAxis = false;
        SetRandomDestination(controller);
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }
    void SetRandomDestination(StateMachineController controller)
    {
        int maxAttempts = 1000; // Limite massimo di tentativi per evitare loop infiniti
        float distanceBetweenPoints = 0;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float randomAngle = Random.Range(0f, 360f);
            float xOffset = (circleRadius * 2) * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float yOffset = (circleRadius * 2) * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
            randomDestination = new Vector2(controller.transform.position.x + -xOffset, controller.transform.position.y + -yOffset);
            distanceBetweenPoints = Vector2.Distance(randomDestination, controller.transform.position);

            if (distanceBetweenPoints > 5f)
            {
                NavMeshHit hit;
                bool isDestinationValid = NavMesh.SamplePosition(randomDestination, out hit, 1f, controller.currentEnemy.currentAgent.areaMask);
                if (isDestinationValid)
                {
                    controller.currentEnemy.currentAgent.SetDestination(randomDestination);
                    return; // Uscita dalla funzione se viene trovata una destinazione valida
                }
            }
        }
    }
}
