using UnityEngine;


[CreateAssetMenu(fileName = "Nemici/Actions/IdleAction", menuName = "Nemici/Actions/IdleAction")]
public class IdleAction : Action, ISubscriber
{
    private void OnEnable()
    {
        Publisher.Subscribe(this, new MessaggioDiProva(""));
    }
    public override void Act(StateMachineController controller)
    {
        Publisher.Publish(new MessaggioDiProva(""));
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        return;
    }

    public void OnPublish(IMessage message)
    {
        if(message is MessaggioDiProva)
        {
            MessaggioDiProva messaggioDiProva = (MessaggioDiProva)message;
            Debug.LogWarning(messaggioDiProva.prova);
        }
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        return;
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        return;
    }
}
