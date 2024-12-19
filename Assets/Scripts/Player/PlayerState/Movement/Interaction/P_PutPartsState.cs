using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class P_PutPartsState : P_InteractionState
{
    public P_PutPartsState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.PutPartsParameterHash);
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.PutPartsParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        player.playerAnim.SetLayerWeight(1, player.carryWeight);
    }

    public override void OnAnimationTransitionEvent()
    {

        player.curCarriedObject.transform.parent = player.partOwner.PartsTransform;
        player.isSetAngleZero = true;
        player.partOwner.InsertParts(player.curCarriedObject.gameObject);
        player.SetCarryWeight();
        player.curCarriedObject.transform.parent = player.partOwner.PartsTransform;
        player.curCarriedObject.transform.DOLocalRotate(Vector3.zero, 0.5f);
        player.curCarriedObject.transform.DOLocalMove(Vector3.zero, 0.5f);
        player.curCarriedObject.canInteract = false;

    }

    public override void OnAnimationExitEvent()
    {
        player.SetPlayerPhysicsIgnore(player.curCarriedObject.col, false);
        player.isCarryObject = false;
        player.curInteractableObject = null;
        player.curCarriedObject = null;
        player.isHandIK = false;
        machine.OnStateChange(machine.IdleState);

    }

    public override void SetDirection()
    {
        player.curDirection = player.partOwner.PartsTransform.position - player.transform.position;
    }

    public override void PlayerRotationControll()
    {
        Quaternion targetRotation = Quaternion.LookRotation(player.curDirection);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.rotateLerpSpeed * Time.fixedDeltaTime);

    }
}

