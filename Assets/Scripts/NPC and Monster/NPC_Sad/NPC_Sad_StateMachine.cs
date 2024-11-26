using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Sad_StateMachine : StateMachine
{
    public NPC_Sad npc;

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    public NPC_Sad_IDLEState IDLEState { get; private set; }
    public NPC_Sad_WalkState WalkState { get; private set; }
    public NPC_Sad_GrappedState GrappedState { get; private set; }
    public NPC_Sad_ReactionThankState ThankState { get; private set; }
    


    public NPC_Sad_StateMachine(NPC_Sad _npc)
    {
        npc = _npc;
        StateInit();
    }
    private void StateInit()
    {
        IDLEState = new NPC_Sad_IDLEState(npc, this);
        WalkState = new NPC_Sad_WalkState(npc, this);
        GrappedState = new NPC_Sad_GrappedState(npc, this);
        ThankState = new NPC_Sad_ReactionThankState(npc, this);

        CurrentState = npc.bWalking ? IDLEState : WalkState;
        CurrentState.OnEnter();
    }

    public override void OnStateUpdate()
    {
        CurrentState.OnUpdate();
    }

    public override void OnStateFixedUpdate()
    {
        CurrentState.OnFixedUpdate();
    }


    public void OnStateChange(BaseState _nextState)
    {
        if (CurrentState == _nextState)
        {
            return;
        }
        PreState = CurrentState;
        CurrentState.OnExit();
        CurrentState = _nextState;
        CurrentState.OnEnter();
    }

    public bool CheckCurrentState(BaseState _state)
    {
        return CurrentState == _state;
    }

    public bool CheckPreState(BaseState _state)
    {
        return PreState == _state;
    }


    public void StartAnimation(int _parametgerHash)
    {

    }

    public void StopAnimation(int _parametgerHash)
    {

    }

    public void OnAnimationEnterEvent()
    {
        CurrentState?.OnAnimationEnterEvent();
    }

    public void OnAnimationExitEvent()
    {
        CurrentState?.OnAnimationExitEvent();
    }

    public void OnAnimationTransitionEvent()
    {
        CurrentState?.OnAnimationTransitionEvent();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        CurrentState?.OnTriggerEnter(other);
    }

    public virtual void OnTriggerStay(Collider other)
    {
        CurrentState?.OnTriggerStay(other);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        CurrentState?.OnTriggerExit(other);
    }

}
