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
            controller.currentAgent.isStopped = true;
            controller.currentAgent.speed = controller.enemyStats.enemySpeed;
            controller.currentAgent.acceleration= controller.enemyStats.enemyAcceleration;
            controller.currentAgent.SetDestination(controller.target.position);

        }
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        controller.currentAgent.isStopped = false;
    }
}
