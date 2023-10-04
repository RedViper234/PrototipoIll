using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Nemici/Actions/AttackAction", menuName = "Nemici/Actions/AttackAction")]
public class AttackAction : Action
{
    public override void Act(StateMachineController controller)
    {
        controller.StartCoroutine(IniziaAttacco(controller));
    }

    private IEnumerator IniziaAttacco(StateMachineController controller)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
        }
        

    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }
}
