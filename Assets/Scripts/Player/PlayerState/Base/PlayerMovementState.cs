using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : BaseState
{

    protected Player player;
    protected PlayerStateMachine machine;

    public PlayerMovementState(Player _player, PlayerStateMachine _machine)
    {
        player = _player;
        machine = _machine;
    }

    public virtual void OnEnter()
    {
        Debug.Log("State: " + GetType().Name);
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
        player.PlayerJump();
    }

    public virtual void OnFixedUpdate()
    {
        player.PlayerWalk();
        player.ControllGravity();
    }


    public virtual void OnAnimationEnterEvent() { }
    public virtual void OnAnimationExitEvent() { }
    public virtual void OnAnimationTransitionEvent() { }
    

}