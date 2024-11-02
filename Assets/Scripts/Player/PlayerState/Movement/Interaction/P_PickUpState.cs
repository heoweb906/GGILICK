using UnityEngine;
public class P_PickUpState : P_InteractionState
{
    public P_PickUpState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.PickUpParameterHash);
        player.isCarryObject = true;
        player.curCarriedObject.rigid.isKinematic = true;
        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.PickUpParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        CheckPutDownObject();
    }

    public void CheckPutDownObject()
    {
        if (player.isCarryObject && !Input.GetButton("Fire1") && !player.playerAnim.IsInTransition(0))
        {
            machine.OnStateChange(machine.PutDownState);

            player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, false);
            player.playerAnim.SetLayerWeight(1, 0);
            machine.OnStateChange(machine.IdleState);
            player.isCarryObject = false;
            player.curCarriedObject.transform.parent = null;
            player.curCarriedObject.rigid.isKinematic = false;
        }
    }

    public override void OnAnimationTransitionEvent()
    {
        player.curCarriedObject.transform.parent = player.CarriedObjectPos;
        player.curCarriedObject.transform.localPosition = Vector3.zero;
        player.curCarriedObject.transform.localRotation = Quaternion.identity;
        player.isHandIK = true;
    }

    public override void OnAnimationExitEvent()
    {
        player.playerAnim.SetLayerWeight(1, 1);
        machine.OnStateChange(machine.IdleState);
    }

}

