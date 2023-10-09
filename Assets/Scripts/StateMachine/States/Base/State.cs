using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;


[CreateAssetMenu(menuName ="Nemici/AI/Stato")]
public class State : ScriptableObject
{
    public List<Action> actions;
    public List<Transition> transitions;
    public void UpdateState(StateMachineController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }
    public void OnExitState(StateMachineController controller)
    {
        OnExitActions(controller);
    }
    public void OnEntryState(StateMachineController controller)
    {
        OnEntryActions(controller);
    }

    public void OnEntryActions(StateMachineController controller)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            var action = actions[i];
            action.ActOnEntryState(controller);
        }
    }
    public void OnExitActions(StateMachineController controller)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            var action = actions[i];
            action.ActOnExitState(controller);
        }
    }

    public void DrawMyGizmos(StateMachineController controller)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            if (transitions[i] != null )
            {
                var decision = transitions[i].decision;
                if(decision.gizmosActive == false) { continue; }
                decision.DrawMyGizmos(controller);

            }
            
        }
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i] != null)
            {
                if (actions[i].gizmosActive == false) { continue; }
                actions[i].ActionDrawGizmos(controller);
            }
        }
    }
   
    private void DoActions(StateMachineController controller)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            var action = actions[i];
            action.Act(controller);

        }
    }
    private void CheckTransitions(StateMachineController controller)
    {
        for (int i = 0; i < transitions.Count; i++)
        {
            bool decisionSucceded = transitions[i].decision.Decide(controller);
            if(decisionSucceded)
            {
                controller.TransitionToState(transitions[i].trueState);
            }
            else
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
       
}
[System.Serializable]
public class Transition
{
    public Decision decision;
    public State trueState;
    public State falseState;
}