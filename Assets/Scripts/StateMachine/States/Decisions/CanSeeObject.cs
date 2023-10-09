using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Nemici/Decisioni/CanSeeObject", menuName = "Nemici/Decisioni/CanSeeObject")]
public class CanSeeObject : Decision
{

    public override bool Decide(StateMachineController controller)
    {
        return false;
    }

    public override void DrawMyGizmos(StateMachineController controller)
    {
        throw new System.NotImplementedException();
    }
}
