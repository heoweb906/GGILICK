using UnityEngine;

public class P_GrabState : P_InteractionState
{
    public P_GrabState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GrabParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GrabParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!Input.GetButton("Fire1"))
        {
            machine.OnStateChange(machine.IdleState);
        }
    }

    public override void SetDirection()
    {
        player.curDirection = player.grabPos.position - player.transform.position;
    }

}
