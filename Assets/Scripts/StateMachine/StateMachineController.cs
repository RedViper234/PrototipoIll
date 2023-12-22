using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateMachineController : MonoBehaviour
{
    public bool aiAttiva = true;
    public State currentState;
    public State remainInState;


    [HideInInspector]public EnemyController currentEnemy;

    private void Awake()
    {
        currentEnemy = GetComponent<EnemyController>();
    }
    private void Start()
    {
        currentState?.OnEntryActions(this);
    }
    public void Update()
    {
        if (aiAttiva)
        {
            currentState?.UpdateState(this);
        }
        
    }
    private void OnDrawGizmos() => currentState?.DrawMyGizmos(this);
    public void TransitionToState(State nextState)
    {
        if(nextState != remainInState)
        {
            currentState?.OnExitActions(this);
            currentState = nextState;
            currentState?.OnEntryActions(this);
        }
    }

}
