using System.Collections.Generic;
using UnityEngine;

public class P_InteractionState : PlayerMovementState
{
    public P_InteractionState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.InteractionParameterHash);
        player.playerMoveSpeed = 0;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.InteractionParameterHash);
        player.closestClockWork.EndCharging_To_BatteryStart();
        player.isGoToTarget = false;
        player.closestClockWork = null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Interaction();
    }

    public override void SetDirection()
    {
        player.curDirection = player.closestClockWork.transform.position - player.transform.position;
    }

    private void Interaction()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") || Input.GetButtonUp("Fire1"))
        {
            machine.OnStateChange(machine.IdleState);
            return;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            machine.OnStateChange(machine.JumpStartIdleState);
            return;
        }
        player.closestClockWork.ChargingBattery();
    }
}
