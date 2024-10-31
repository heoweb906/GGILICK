using System.Collections.Generic;
using UnityEngine;

public class P_SpinClockWorkState : P_InteractionState
{
    public P_SpinClockWorkState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.SpinClockWorkParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.SpinClockWorkParameterHash);
        player.curClockWork.EndCharging_To_BatteryStart();
        player.curClockWork = null;
        player.curInteractableObject = null;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        Interaction();
    }

    public override void SetDirection()
    {
        player.curDirection = player.curInteractableObject.transform.position - player.transform.position;
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
        if (!player.curClockWork.BoolBatteryFullCharging())
            player.curClockWork.ChargingBattery();
        else
            machine.OnStateChange(machine.IdleState);
    }

}
