using UnityEngine;


public abstract class Decision : ScriptableObject
{

    public bool gizmosActive = true;
    public Color gizmosColor;
    public abstract bool Decide(StateMachineController controller);
    public abstract void DrawMyGizmos(StateMachineController controller);
}
