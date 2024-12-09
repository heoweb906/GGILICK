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
        player.angle = 0;
        player.isSetAngleZero = false;
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

    public override void SetDirection()
    {
        if(player.isHandIK)
            base.SetDirection();
        else
            player.curDirection = player.curInteractableObject.transform.position - player.transform.position;
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
            player.curInteractableObject = null;
            player.curCarriedObject.transform.parent = null;
            player.curCarriedObject.rigid.isKinematic = false;
            player.isHandIK = false;
        }
    }

    public override void OnAnimationTransitionEvent()
    {
        player.curCarriedObject.transform.parent = player.CarriedObjectPos;
        BoxCollider _col = player.curCarriedObject.GetComponent<BoxCollider>();
        player.curCarriedObject.transform.localPosition = Vector3.zero;
        Debug.Log(Vector3.Scale(_col.size * 0.5f, _col.transform.lossyScale).y);
        player.curCarriedObject.transform.localPosition -= new Vector3(Vector3.Scale(_col.size * 0.5f, _col.transform.localScale).y,0,0);
        player.curCarriedObject.transform.localRotation = Quaternion.identity;
        player.isHandIK = true;
    }

    public override void OnAnimationExitEvent()
    {
        player.playerAnim.SetLayerWeight(1, 1);
        machine.OnStateChange(machine.IdleState);
    }

}

