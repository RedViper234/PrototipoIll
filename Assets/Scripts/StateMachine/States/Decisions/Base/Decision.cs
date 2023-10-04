using UnityEngine;


public abstract class Decision : ScriptableObject
{
    public Color gizmosColor;
    public abstract bool Decide(StateMachineController controller);
    public abstract void DrawMyGizmos(StateMachineController controller);
}
