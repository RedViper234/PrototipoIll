using UnityEngine;


[CreateAssetMenu(fileName = "Nemici/Actions/IdleAction", menuName = "Nemici/Actions/IdleAction")]
public class IdleAction : Action
{
    public override void Act(StateMachineController controller)
    {
        
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }
}
