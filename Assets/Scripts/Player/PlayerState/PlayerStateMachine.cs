using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    protected Player player;

    public BaseState CurrentState { get; private set; }
    public BaseState PreState { get; private set; }
    public P_GroundState GroundState { get; private set; }
    public P_OnAirState OnAirState { get; private set; }

    public P_IdleState IdleState { get; private set; }
    public P_SoftLandingState SoftLandingState { get; private set; }
    public P_WalkStartState WalkStartState { get; private set; }
    public P_WalkingState WalkingState { get; private set; }
    public P_RunStartState RunStartState { get; private set; }
    public P_RunningState RunningState { get; private set; }
    public P_SoftStopState SoftStopState { get; private set; }

    public P_FallingState FallingState { get; private set; }
    public P_JumpStartState JumpStartState { get; private set; }


    public PlayerStateMachine(Player _player)
    {
        player = _player;
        StateInit();
    }

    private void StateInit()
    {
        GroundState = new P_GroundState(player, this);
        OnAirState = new P_OnAirState(player, this);
        IdleState = new P_IdleState(player, this);
        SoftLandingState = new P_SoftLandingState(player, this);
        WalkStartState = new P_WalkStartState(player, this);
        WalkingState = new P_WalkingState(player, this);
        RunStartState = new P_RunStartState(player, this);
        RunningState = new P_RunningState(player, this);
        SoftStopState = new P_SoftStopState(player, this);
        FallingState = new P_FallingState(player, this);
        JumpStartState = new P_JumpStartState(player, this);
        CurrentState = IdleState;
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
        if(CurrentState == _nextState)
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
        player.playerAnim.SetBool(_parametgerHash, true);
    }

    public void StopAnimation(int _parametgerHash)
    {
        player.playerAnim.SetBool(_parametgerHash, false);
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
}
