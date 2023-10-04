using UnityEngine;


public abstract class Action : ScriptableObject
{
    public abstract void Act(StateMachineController controller);
    public abstract void ActOnExitState(StateMachineController controller);
}
