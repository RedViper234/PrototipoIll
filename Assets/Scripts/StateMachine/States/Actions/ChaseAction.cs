using UnityEngine;



[CreateAssetMenu(fileName = "Nemici/Actions/ChaseAction", menuName = "Nemici/Actions/ChaseAction")]
public class ChaseAction : Action
{
    public override void Act(StateMachineController controller)
    {
        if(controller != null)
        {
            controller.currentAgent.updateRotation = false;
            controller.currentAgent.updateUpAxis= false;
            controller.currentAgent.isStopped = false;
            controller.currentAgent.speed = controller.enemyStats.enemySpeed;
            controller.currentAgent.acceleration= controller.enemyStats.enemyAcceleration;
            controller.currentAgent.SetDestination(controller.target.position);

        }
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        return;
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        return;
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        controller.currentAgent.isStopped = true;
        Debug.Log("CHIAMA EXIT STATE");
        controller.currentAgent.SetDestination( controller.gameObject.transform.position);
    }
}
