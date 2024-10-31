using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : BaseState
{

    protected Player player;
    protected PlayerStateMachine machine;

    public PlayerMovementState(Player _player, PlayerStateMachine _machine)
    {
        player = _player;
        machine = _machine;
    }

    public virtual void OnEnter()
    {
        Debug.Log("State: " + GetType().Name);
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        if(!player.isCarryObject)
            player.isRun = Input.GetButton("Run");
        SetDirection();
    }

    public virtual void OnFixedUpdate()
    {
        PlayerVelocityControll();
    }


    public virtual void OnAnimationEnterEvent() { }
    public virtual void OnAnimationExitEvent() { }
    public virtual void OnAnimationTransitionEvent() { }


    




    public float _horizontal = 0;
    public float _vertical = 0;

    public Vector3 GetCurDirection() { return player.curDirection; }

    public virtual void SetDirection()
    {
        player.curDirection = new Vector3(_horizontal, 0, _vertical);
        if (_horizontal == 0 && _vertical == 0)
            player.curDirection = Vector3.zero;
    }

    // �÷��̾� �⺻ �̵�
    public void PlayerVelocityControll()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            machine.OnStateChange(machine.IdleState);

        //Debug.Log("State: " + machine.CurrentState.GetType().Name);

        //if (machine.CheckCurrentState(machine.WalkingState))
        //    player.playerMoveSpeed = player.playerWalkSpeed;
        //else if (machine.CheckCurrentState(machine.RunningState))
        //    player.playerMoveSpeed = player.playerRunSpeed;
        player.curDirection.y = 0;
        player.curDirection = player.curDirection.normalized;

        // Rotation �̵� �������� ����
        if (player.curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.curDirection);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, player.playerRotateLerpSpeed * Time.fixedDeltaTime);
        }

        // Rigid�� �ӵ� ������ �̵�, ���� ���
        player.curDirection = Vector3.Lerp(player.preDirection, player.curDirection, player.moveLerpSpeed * Time.fixedDeltaTime);

        Vector3 velocity = CalculateNextFrameGroundAngle(player.playerMoveSpeed) < player.maxSlopeAngle ? player.curDirection :  Vector3.zero;
        Vector3 gravity;


        if (IsOnSlope()) // ���ζ�� ��翡 ���缭 ���Ⱚ ����
        {
            velocity = AdjustDirectionToSlope(player.curDirection);
            gravity = Vector3.zero;
            player.rigid.useGravity = false;
        }
        else if(machine.CurrentState is not P_ClimbingState)
        {
            gravity = new Vector3(0, player.rigid.velocity.y, 0);
            player.rigid.useGravity = true;
        }
        else
        {
            gravity = Vector3.zero;
        }

        player.rigid.velocity = velocity * player.playerMoveSpeed + gravity;

        if (player.curMovingPlatform != null)
        {
            player.platformVelocity = player.curMovingPlatform.GetPlatformVelocity();
        }
        else
        {
            if (player.groundList.Count == 0)
            {
                player.platformVelocity = Vector3.Lerp(player.platformVelocity, Vector3.zero, player.platformVelocityLerp);

            }
            else
                player.platformVelocity = Vector3.zero;
        }
        player.rigid.velocity += player.platformVelocity;
        player.preDirection = player.curDirection;
        //Debug.Log(player.rigid.velocity);
    }

    

    // ���� ��� �ִ� �� ��� üũ
    private bool IsOnSlope()
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, Vector3.down * player.rayDistance, Color.red);
        if (Physics.Raycast(ray, out player.slopeHit, player.rayDistance, player.groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, player.slopeHit.normal);
            return angle != 0f && angle < player.maxSlopeAngle;
        }
        return false;
    }

    // ��� �ִ� �� �������� ���� �缳��
    private Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, player.slopeHit.normal);
    }

    private float CalculateNextFrameGroundAngle(float _moveSpeed)
    {
        var nextFramePlayerPosition = player.raycastOrigin.transform.position + player.curDirection * _moveSpeed * Time.fixedDeltaTime;

        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, 1f))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }

    public void PlayerJump()
    {
        //player.rigid.velocity = new Vector3(player.rigid.velocity.x, 0, player.rigid.velocity.z);
        player.rigid.AddForce(new Vector3(0, player.firstJumpPower, 0), ForceMode.VelocityChange);
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WalkableGround") || other.CompareTag("MovingPlatform"))
        {
            player.groundList.Add(other.gameObject);

            if (other.CompareTag("MovingPlatform"))
            {
                player.curMovingPlatform = other.GetComponent<MovingPlatform>();
            }
        }
    }

    public virtual void OnTriggerStay(Collider other) { }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WalkableGround") || other.CompareTag("MovingPlatform"))
        {
            if (player.groundList.Contains(other.gameObject))
                player.groundList.Remove(other.gameObject);

            if (machine.CurrentState is not P_JumpStartState
                && machine.CurrentState is not P_ClimbingState
                && player.groundList.Count<=0)
                machine.OnStateChange(machine.FallingMoveState);

            if (other.CompareTag("MovingPlatform"))
            {
                player.curMovingPlatform = null;
            }

            if (player.groundList.Count > 0) return;

            
        }
    }


    
}