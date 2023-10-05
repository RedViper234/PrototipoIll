using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    public bool aiAttiva = true;
    public State currentState;
    public State remainInState;
    public Transform target;
    
    [HideInInspector]public NavMeshAgent currentAgent;
    [HideInInspector] public EnemyController currentEnemy;
    [HideInInspector] public EnemyStatsSO enemyStats;
    
    private void Awake()
    {
        currentAgent = GetComponent<NavMeshAgent>();
        currentEnemy = GetComponent<EnemyController>();
        enemyStats = currentEnemy.EnemyStats;
    }

    public void SetUpAI()
    {
        currentAgent.enabled = aiAttiva;
    }
    public void Update()
    {
        if (aiAttiva)
        {
            currentAgent.isStopped = !aiAttiva;
            currentState.UpdateState(this);
        }
        else
        {
            currentAgent.isStopped = !aiAttiva;
        }
    }
    private void OnDrawGizmos() => currentState.DrawMyGizmos(this);
    public void TransitionToState(State nextState)
    {
        if(nextState != remainInState)
        {
            currentState.OnExitActions(this);
            currentState = nextState;
            currentState.OnEntryActions(this);
        }
    }

}
