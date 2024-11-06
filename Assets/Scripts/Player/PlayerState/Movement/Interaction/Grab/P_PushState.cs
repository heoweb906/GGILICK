using UnityEngine;

public class P_PushState : P_GrabState
{
    public P_PushState(Player player, PlayerStateMachine machine) : base(player, machine) { }

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
    }

}
