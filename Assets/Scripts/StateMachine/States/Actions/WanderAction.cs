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
        float distanzaTraUnPunto = 0;
        while (true)
        {
            float randomAngle = Random.Range(0f, 360f);
            float x = (circleRadius * 2) * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float y = (circleRadius * 2) * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
            randomDestination = new Vector2(controller.transform.position.x + -x, controller.transform.position.y + -y);
            distanzaTraUnPunto = Vector2.Distance(randomDestination, controller.transform.position);
            if (distanzaTraUnPunto > 5f)
            {
                break;
            }
        }

        controller.currentEnemy.currentAgent.SetDestination(randomDestination);
        NavMeshHit hit;
        bool isDestinationValid = NavMesh.SamplePosition(randomDestination, out hit, 1f, controller.currentEnemy.currentAgent.areaMask);
        if (!isDestinationValid)
        {
            SetRandomDestination(controller);
        }
    }
}
