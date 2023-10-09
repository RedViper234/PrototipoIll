using UnityEngine;


public abstract class Action : ScriptableObject
{
    public bool gizmosActive = true;
    public abstract void ActOnEntryState(StateMachineController controller);
    public abstract void Act(StateMachineController controller);
    public abstract void ActOnExitState(StateMachineController controller);
    public abstract void ActionDrawGizmos(StateMachineController controller);
}
