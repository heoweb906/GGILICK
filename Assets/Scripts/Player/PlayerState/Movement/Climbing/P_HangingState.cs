using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_HangingState : P_ClimbingState
{
    public P_HangingState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.HangingParameterHash);
        Vector3 targetPos = new Vector3(0, player.hangingPosOffset_Height, 0) + player.transform.position
            + new Vector3(player.transform.position.x - player.cliffRayHit.point.x, 0, player.transform.position.z - player.cliffRayHit.point.z).normalized * player.hangingPosOffset_Front;
       // player.transform.DOMove(targetPos, 0.1f);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.HangingParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetButtonUp("Fire1"))
            machine.OnStateChange(machine.FallingIdleState);
        else if (Input.GetButtonDown("Jump") && !player.playerAnim.IsInTransition(0))
        {
            Debug.Log("!!!!!!!!!");
            machine.OnStateChange(machine.ClimbingToTopState);
        }
    }
}
