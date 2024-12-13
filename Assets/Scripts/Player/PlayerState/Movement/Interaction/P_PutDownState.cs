using Unity.VisualScripting;
using UnityEngine;

public class P_PutDownState : P_InteractionState
{
    public P_PutDownState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.PutDownParameterHash);
        player.SetCarryWeight();
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.PutDownParameterHash);

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.playerAnim.SetLayerWeight(1, player.carryWeight);
    }

    public override void OnAnimationTransitionEvent()
    {

        player.curCarriedObject.transform.parent = null;
        player.curCarriedObject.rigid.isKinematic = false;
        player.isSetAngleZero = true;

    }

    public override void OnAnimationExitEvent()
    {

        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, false);
        machine.OnStateChange(machine.IdleState);
        player.isCarryObject = false;
        player.curInteractableObject = null;
        player.curCarriedObject = null;
        player.isHandIK = false;

    }


}
