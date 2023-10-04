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
    public void OnExitState()
    {

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
            var decision = transitions[i].decision;
            decision.DrawMyGizmos(controller);
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
                if (transitions[i].decision != controller.remainInState) 
                { 
                    OnExitActions(controller);
                }
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