using UnityEngine;



[CreateAssetMenu(fileName = "Nemici/Actions/ChaseAction", menuName = "Nemici/Actions/ChaseAction")]
public class ChaseAction : Action
{
    public override void Act(StateMachineController controller)
    {
        if(controller != null)
        {
            controller.currentEnemy.currentAgent.updateRotation = false;
            controller.currentEnemy.currentAgent.updateUpAxis= false;
            controller.currentEnemy.currentAgent.isStopped = false;
            controller.currentEnemy.currentAgent.speed = controller.currentEnemy.enemyStats.enemySpeed;
            controller.currentEnemy.currentAgent.acceleration= controller.currentEnemy.enemyStats.enemyAcceleration;
            controller.currentEnemy.currentAgent.SetDestination(controller.currentEnemy.target.position);

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
        controller.currentEnemy.currentAgent.isStopped = true;
        Debug.Log("CHIAMA EXIT STATE");
        controller.currentEnemy.currentAgent.SetDestination( controller.gameObject.transform.position);
        controller.currentEnemy.currentAgent.SetDestination( controller.gameObject.transform.position);
    }
}
