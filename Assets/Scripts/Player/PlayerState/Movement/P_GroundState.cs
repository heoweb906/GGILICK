using UnityEngine;
public class P_GroundState : PlayerMovementState
{
    public P_GroundState(Player player, PlayerStateMachine machine) : base(player, machine) { }

    public override void OnEnter()
    {
        base.OnEnter();
        machine.StartAnimation(player.playerAnimationData.GroundParameterHash);
        player.moveLerpSpeed = player.playerMoveLerpSpeed;
    }

    public override void OnExit()
    {
        base.OnExit();
        machine.StopAnimation(player.playerAnimationData.GroundParameterHash);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (GetCurDirection() != Vector3.zero && (machine.CheckCurrentState(machine.IdleState) || machine.CheckCurrentState(machine.SoftLandingState) 
            || machine.CheckCurrentState(machine.SoftStopState)|| machine.CheckCurrentState(machine.HardStopState)
            || machine.CheckCurrentState(machine.RunningState) || machine.CheckCurrentState(machine.WalkingState)))
        {
            if(player.isRun)
                machine.OnStateChange(machine.RunningState);
            else
                machine.OnStateChange(machine.WalkingState);
        }
        else if (GetCurDirection() == Vector3.zero)
        {
            if(machine.CheckCurrentState(machine.WalkingState))
                machine.OnStateChange(machine.SoftStopState);
            else if(machine.CheckCurrentState(machine.RunningState) || machine.CheckCurrentState(machine.MoveLandingState))
                machine.OnStateChange(machine.HardStopState);

        }


        CheckInputJump();
        InteractWithObject();


        CheckPutDownObject();
    }

    public void CheckInputJump()
    {
        if (Input.GetButtonDown("Jump") && !player.isCarryObject)
        {
            if (GetCurDirection() == Vector3.zero)
            {
                machine.OnStateChange(machine.JumpStartIdleState);
            }
            else
            {
                machine.OnStateChange(machine.JumpStartMoveState);
            }
        }
    }

    public void CheckPutDownObject()
    {
        if (player.isCarryObject && machine.CurrentState is not P_MoveStopState && !Input.GetButton("Fire1") && !player.playerAnim.IsInTransition(0))
        {
            machine.OnStateChange(machine.PutDownState);
        }
    }

    public void InteractWithObject()
    {
        if (player.isCarryObject)
            return;

        if (Input.GetButtonDown("Fire1")) // 좌클릭
        {
            if (!FindClosestInteractableObject())
                return;

                if (player.curInteractableObject.type == InteractableType.ClockWork)
            {
                player.curClockWork = player.curInteractableObject.GetComponent<ClockWork>();

                if (player.curClockWork.GetClockWorkType() == ClockWorkType.Floor)
                {
                    player.targetPos = player.curClockWork.transform.position + (player.transform.position - player.curClockWork.transform.position).normalized * player.clockWorkInteractionDistance;
                }
                else if (player.curClockWork.GetClockWorkType() == ClockWorkType.Wall)
                {
                    float angle = player.curClockWork.transform.eulerAngles.y * Mathf.Deg2Rad;
                    player.targetPos = player.curClockWork.transform.position + new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)).normalized * player.clockWorkInteractionDistance;
                }
            }
            else if (player.curInteractableObject.type == InteractableType.Carrried)
            {
                player.curCarriedObject = player.curInteractableObject.GetComponent<CarriedObject>();
                Debug.Log(player.curCarriedObject.transform.position);
                player.targetPos = player.curCarriedObject.transform.position + (player.transform.position - player.curCarriedObject.transform.position).normalized * player.carriedObjectInteractionDistance;

            }

        }
        else if (Input.GetButton("Fire1") && player.curInteractableObject != null && machine.CurrentState is not P_MoveStopState) // 좌클릭을 누르고 있는 동안
        {
            player.isGoToTarget = true;
            if (Vector3.Distance(new Vector3(player.targetPos.x, 0, player.targetPos.z), new Vector3(player.transform.position.x, 0, player.transform.position.z)) < 0.03f)
            {
                if (player.curInteractableObject.type == InteractableType.ClockWork)
                    machine.OnStateChange(machine.SpinClockWorkState);
                else if(player.curInteractableObject.type == InteractableType.Carrried)
                    machine.OnStateChange(machine.PickUpState);
            }

            //player.closestClockWork.ChargingBattery(); // OnClockWork 함수 호출
        }
        else if (!Input.GetButton("Fire1")) // 마우스를 떼면
        {
            player.curClockWork = null; // 가장 가까운 ClockWork 참조 초기화
            player.curCarriedObject = null; // 가장 가까운 ClockWork 참조 초기화
            player.curInteractableObject = null; // 가장 가까운 ClockWork 참조 초기화
            player.isGoToTarget = false;
        }
    }

    public override void SetDirection()
    {
        if(!player.isGoToTarget)
            base.SetDirection();
        else
        {
            player.curDirection = player.targetPos - player.transform.position;
        }
    }

    public bool FindClosestInteractableObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, player.detectionRadius);
        player.curInteractableObject = null;

        player.curClockWork = null; // 이전 참조 초기화
        player.curCarriedObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            // ClockWork 스크립트가 있는지 확인
            InteractableObject detectedObject = collider.GetComponent<InteractableObject>();
            if (detectedObject != null && detectedObject.canInteract)
            {
                float distance = Vector3.Distance(player.transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    player.curInteractableObject = detectedObject; // 가장 가까운 ClockWork 참조 저장
                }
            }
        }

        if (player.curInteractableObject != null)
        {
            return true;
            // 여기에서 추가적인 로직을 구현할 수 있습니다.
        }
        else
        {
            return false;
        }
    }

    
}


