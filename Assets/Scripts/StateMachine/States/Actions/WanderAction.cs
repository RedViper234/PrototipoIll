using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus.Extensions;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Actions/WanderAction", menuName = "Nemici/Actions/WanderAction")]
public class WanderAction : Action
{
    private float circleRadius = 5f; // Raggio del cerchio
    private float minDestinationChangeTime = 2f;
    private float maxDestinationChangeTime = 5f;

    private Vector2 randomDestination;
    private float timeToChangeDestination;
    public override void Act(StateMachineController controller)
    {
        if (controller.currentAgent.remainingDistance < 0.5f && !controller.currentAgent.pathPending)
        {
            SetRandomDestination(controller);

        }
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(randomDestination, 1f);
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        controller.currentAgent.updateRotation= false;
        controller.currentAgent.updateUpAxis = false;
        Debug.Log(controller.currentAgent.updateRotation);
        timeToChangeDestination = Random.Range(minDestinationChangeTime, maxDestinationChangeTime);
        SetRandomDestination(controller);
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }
    void SetRandomDestination(StateMachineController controller)
    {
        float distanzaTraUnPunto = 0;
        while(true)
        {
            float randomAngle = Random.Range(0f, 360f);
            float x = (circleRadius * 2) * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
            float y = (circleRadius * 2) * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
            randomDestination = new Vector2(controller.transform.position.x + -x, controller.transform.position.y + -y);
            distanzaTraUnPunto = Vector2.Distance(randomDestination, controller.transform.position);
            if(distanzaTraUnPunto > 5f)
            {
                break;
            }
        }
        controller.currentAgent.SetDestination(randomDestination);
        Vector3 nextDestination = controller.currentAgent.nextPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(nextDestination,out hit, 0.1f, NavMesh.AllAreas);
        
        if (hit.hit)
        {
            Debug.Log(hit.hit);
            Debug.Log("Il prossimo punto è su una navmesh valida.");
        }
        else
        {
            Debug.Log(hit.hit);
            Debug.Log("Il prossimo punto non è su una navmesh valida.");
            SetRandomDestination(controller);
        }
        
    }
}
