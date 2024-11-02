using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_ClimbingToTopState : P_ClimbingState
{
    public P_ClimbingToTopState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.ClimbingToTopParameterHash);
        player.playerAnim.applyRootMotion = true;
        player.playerAnim.updateMode = AnimatorUpdateMode.AnimatePhysics;
        player.SetColliderTrigger(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.ClimbingToTopParameterHash);
        //player.playerAnim.applyRootMotion = false;
        player.SetColliderTrigger(true);
        player.SetRootMotion();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnAnimationExitEvent()
     {
        player.transform.position = new Vector3(player.transform.position.x, player.hangingPos.y, player.transform.position.z);

        machine.OnStateChange(machine.IdleState);
    }

    
}


