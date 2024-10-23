using UnityEngine;
public class P_OnAirState : PlayerMovementState
{
    public P_OnAirState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.OnAirParameterHash);
        player.moveLerpSpeed = player.playerMoveLerpSpeedOnJump;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        ControllGravity();
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.OnAirParameterHash);
    }

    public void ControllGravity()
    {
        if (player.rigid.velocity.y < 3 && player.groundList.Count == 0)
        {
            if (player.rigid.velocity.y > -player.maxFallingSpeed)
                player.rigid.velocity -= new Vector3(0, player.fallingPower * Time.fixedDeltaTime, 0);
            else player.rigid.velocity = new Vector3(player.rigid.velocity.x, -player.maxFallingSpeed, player.rigid.velocity.z);
        }

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (machine.CheckCurrentState(machine.JumpStartIdleState) || machine.CheckCurrentState(machine.FallingIdleState))
                machine.OnStateChange(machine.SoftLandingState);
            else if (machine.CheckCurrentState(machine.JumpStartMoveState) || machine.CheckCurrentState(machine.FallingMoveState))
                machine.OnStateChange(machine.MoveLandingState);
        }
        base.OnTriggerEnter(other);
    }
    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
