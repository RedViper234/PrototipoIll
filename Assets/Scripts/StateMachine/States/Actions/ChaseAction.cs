using System.Collections;
using UnityEngine;



[CreateAssetMenu(fileName = "Nemici/Actions/ChaseAction", menuName = "Nemici/Actions/ChaseAction")]
public class ChaseAction : Action
{
    [SerializeField,Range(0,10)] private float m_frequenzaCambioTraiettoria = 0f;
    [SerializeField] private float m_attesaPrimaDiRiprendereIlPath;
    [SerializeField, Range(0, 100)] private int m_percentualeSuccessoCambioTraiettoria = 0;

    private bool m_isChangingDirection = false;
    private Vector2 playerPosition;

    private IEnumerator coroutine;
    public override void Act(StateMachineController controller)
    {
        if(!m_isChangingDirection)
            controller.currentEnemy.currentAgent.SetDestination(controller.currentEnemy.target.position);
        if(controller.currentEnemy.target == null){
            controller.currentEnemy.target = FindFirstObjectByType<PlayerController>()?.transform;
        }
    }

    public override void ActionDrawGizmos(StateMachineController controller)
    {
        return;
    }

    public override void ActOnEntryState(StateMachineController controller)
    {
        controller.currentEnemy.currentAgent.updateRotation = false;
        controller.currentEnemy.currentAgent.updateUpAxis = false;
        controller.currentEnemy.currentAgent.isStopped = false;
        controller.currentEnemy.currentAgent.speed = controller.currentEnemy.enemyStats.enemySpeed;
        controller.currentEnemy.currentAgent.acceleration = controller.currentEnemy.enemyStats.enemyAcceleration;
        controller.currentEnemy.currentAgent.SetDestination(controller.currentEnemy.target.position);
        controller.StartCoroutine(CheckForChangeDestination(controller));
        coroutine = CheckForChangeDestination(controller);
    }

    public override void ActOnExitState(StateMachineController controller)
    {
        controller.currentEnemy.currentAgent.isStopped = true;
        Debug.Log("CHIAMA EXIT STATE");
        controller.currentEnemy.currentAgent.SetDestination( controller.gameObject.transform.position);

    }
    public IEnumerator CheckForChangeDestination(StateMachineController controller)
    {
        while (true) 
        {
            m_isChangingDirection = true;
            float randomForCheckDestination = Random.Range(0, 100);
            yield return new WaitForSeconds(m_frequenzaCambioTraiettoria);
            if (randomForCheckDestination > m_percentualeSuccessoCambioTraiettoria) { Debug.Log("Non Passato"); yield return coroutine.MoveNext(); }
            controller.currentEnemy.currentAgent.SetDestination(Vector2.one * 10);
            yield return new WaitForSeconds(m_attesaPrimaDiRiprendereIlPath);
            if(controller == null) {yield return null;}
            controller.currentEnemy.currentAgent.SetDestination(controller.currentEnemy.target.position);
            m_isChangingDirection = false;
        }
    }
}
